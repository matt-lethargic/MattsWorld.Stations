using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace MattsWorld.Stations.Domain.Models
{
    class PrivateReflectionDynamicObject : DynamicObject
    {
        private static IDictionary<Type, IDictionary<string, IProperty>> _propertiesOnType = new ConcurrentDictionary<Type, IDictionary<string, IProperty>>();

        interface IProperty
        {
            string Name { get; }
            object GetValue(object obj, object[] index);
            void SetValue(object obj, object val, object[] index);
        }

        class Property : IProperty
        {
            internal PropertyInfo PropertyInfo { get; set; }

            string IProperty.Name
            {
                get { return PropertyInfo.Name; }
            }

            object IProperty.GetValue(object obj, object[] index)
            {
                return PropertyInfo.GetValue(obj, index);
            }

            void IProperty.SetValue(object obj, object val, object[] index)
            {
                PropertyInfo.SetValue(obj, val, index);
            }
        }

        class Field : IProperty
        {
            internal FieldInfo FieldInfo { get; set; }

            string IProperty.Name
            {
                get { return FieldInfo.Name; }
            }

            public object GetValue(object obj, object[] index)
            {
                return FieldInfo.GetValue(obj);
            }

            public void SetValue(object obj, object val, object[] index)
            {
                FieldInfo.SetValue(obj, val);
            }
        }

        private object RealObject { get; set; }

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        internal static object WrapObjectIfNeeded(object o)
        {
            if (o == null || o.GetType().IsPrimitive || o is string)
            {
                return o;
            }

            return new PrivateReflectionDynamicObject { RealObject = o };
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            IProperty prop = GetProperty(binder.Name);
            result = prop.GetValue(RealObject, index: null);
            result = WrapObjectIfNeeded(result);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            IProperty prop = GetProperty(binder.Name);
            prop.SetValue(RealObject, value, index: null);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            IProperty prop = GetIndexProperty();
            result = prop.GetValue(RealObject, indexes);
            result = WrapObjectIfNeeded(result);
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            IProperty prop = GetIndexProperty();
            prop.SetValue(RealObject, value, indexes);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = InvokeMemberOnType(RealObject.GetType(), RealObject, binder.Name, args);
            result = WrapObjectIfNeeded(result);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Convert.ChangeType(RealObject, binder.Type);
            return true;
        }

        public override string ToString()
        {
            return RealObject.ToString();
        }

        private IProperty GetIndexProperty()
        {
            return GetProperty("Item");
        }

        private IProperty GetProperty(string propertyName)
        {
            IDictionary<string, IProperty> typeProperties = GetTypeProperties(RealObject.GetType());
            IProperty property;
            if (typeProperties.TryGetValue(propertyName, out property))
            {
                return property;
            }

            var propNames = typeProperties.Keys.Where(name => name[0] != '<').OrderBy(name => name);
            throw new ArgumentException(string.Format("The property {0} doesn't exist on the type {1}. Supported properties are: {2}",
                propertyName, RealObject.GetType(), string.Join(", ", propNames)));
        }

        private static IDictionary<string, IProperty> GetTypeProperties(Type type)
        {
            IDictionary<string, IProperty> typeProperties;
            if (_propertiesOnType.TryGetValue(type, out typeProperties))
            {
                return typeProperties;
            }

            typeProperties = new ConcurrentDictionary<string, IProperty>();

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags).Where(p=>p.DeclaringType == type))
            {
                typeProperties[prop.Name] = new Property {PropertyInfo = prop};
            }

            foreach (FieldInfo field in type.GetFields(BindingFlags).Where(p=>p.DeclaringType == type))
            {
                typeProperties[field.Name] = new Field {FieldInfo = field};
            }

            if (type.BaseType != null)
            {
                foreach (IProperty prop in GetTypeProperties(type.BaseType).Values)
                {
                    typeProperties[prop.Name] = prop;
                }
            }

            _propertiesOnType[type] = typeProperties;

            return typeProperties;
        }

        private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
        {
            try
            {
                return type.InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags, null, target, args);
            }
            catch (MissingMethodException)
            {
                if (type.BaseType != null)
                {
                    return InvokeMemberOnType(type.BaseType, target, name, args);
                }
                return null;
            }
        }
    }

    public static class PrivateReflectionDynamicObjectExtensions
    {
        public static dynamic AsDynamic(this Object o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
}

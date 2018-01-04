using System;
using System.Linq;
using System.Reflection;
using MattsWorld.Stations.Adapters.CosmosRepository.Models;
using MattsWorld.Stations.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MattsWorld.Stations.Adapters.CosmosRepository.Converters
{
    internal class EventDescriptionConverter :JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = JToken.FromObject(value);
            token.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            Guid aggregateId = Guid.Parse(obj["AggregateId"].ToString());
            int version = int.Parse(obj["Version"].ToString());
            string eventType = obj["EventType"].ToString();

            Type eventDataType = Assembly.Load("MattsWorld.Stations.Domain.Models").GetTypes().First(t => t.FullName == eventType);
            Event eventData = (Event) serializer.Deserialize(new JTokenReader(obj.Property("EventData").Value), eventDataType);

            EventDescriptor eventDescriptor = new EventDescriptor(aggregateId, eventData, eventType, version);
            return eventDescriptor;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(EventDescriptor))
                return true;

            return false;
        }
    }
}

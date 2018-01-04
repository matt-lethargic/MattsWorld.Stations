using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Interfaces;

namespace MattsWorld.Stations.Domain
{
    public class LocalBus : ICommandSender
    {
        private readonly Dictionary<Type, List<Func<IMessage, Task>>> _routes = new Dictionary<Type, List<Func<IMessage, Task>>>();

        public void RegisterHandler<T>(Func<T, Task> handler) where T : IMessage
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<IMessage, Task>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(x=> handler((T)x));
        }

        public async Task Send<T>(T command) where T : ICommand
        {
            if (_routes.TryGetValue(typeof(T), out var handlers))
            {
                if (handlers.Count != 1) throw new InvalidOperationException("Cannot send to more than one handler.");
                await handlers[0](command);
            }
            else
            {
                throw new InvalidOperationException("No handler registerd.");
            }
        }
    }
}

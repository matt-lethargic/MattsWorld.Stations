using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Interfaces;

namespace MattsWorld.Stations.Domain
{
    public class LocalPublisher : IEventPublisher
    {
        private readonly Dictionary<Type, List<Func<IEvent, Task>>> _routes = new Dictionary<Type, List<Func<IEvent, Task>>>();

        public void RegisterHandler<T>(Func<T, Task> handler) where T : IMessage
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<IEvent, Task>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(x => handler((T)x));
        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            if (!_routes.TryGetValue(@event.GetType(), out var handlers)) return;

            foreach (Func<IEvent, Task> handler in handlers)
            {
                var actualHandler = handler;
                await actualHandler(@event);
            }
        }
    }
}
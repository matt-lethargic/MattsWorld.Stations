using System;
using MattsWorld.Stations.Domain.Models;

namespace MattsWorld.Stations.Adapters.CosmosRepository.Models
{
    public class EventDescriptor
    {
        public Event EventData;
        public string EventType;
        public Guid AggregateId;
        public int Version;

        public EventDescriptor(Guid aggregateId, Event eventData, string eventType, int version)
        {
            AggregateId = aggregateId;
            EventData = eventData;
            EventType = eventType;
            Version = version;
        }
    }
}

using MattsWorld.Stations.Domain.Interfaces;

namespace MattsWorld.Stations.Domain.Models
{
    public abstract class Event : IEvent
    {
        public int Version { get; set; }
    }
}
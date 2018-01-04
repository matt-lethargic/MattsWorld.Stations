using System;

namespace MattsWorld.Stations.Domain.Models.Events.Stations
{
    public class StationCreated : Event
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Code;

        public StationCreated(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}

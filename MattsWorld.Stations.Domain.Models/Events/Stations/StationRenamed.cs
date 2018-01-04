using System;

namespace MattsWorld.Stations.Domain.Models.Events.Stations
{
    public class StationRenamed : Event
    {
        public readonly Guid Id;
        public readonly string NewName;

        public StationRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}

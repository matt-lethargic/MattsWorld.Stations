using System;

namespace MattsWorld.Stations.Domain.Models.Commands.Stations
{
    public class RenameStation : Command
    {
        public readonly Guid Id;
        public readonly string NewName;
        public readonly int OriginalVersion;

        public RenameStation(Guid id, string newName, int originalVersion)
        {
            Id = id;
            NewName = newName;
            OriginalVersion = originalVersion;
        }
    }
}

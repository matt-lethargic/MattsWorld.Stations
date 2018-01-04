using System;

namespace MattsWorld.Stations.Domain.Models.Views
{
    public class StationDetailView : View
    {
        public string Name;
        public string Code;
        public int Version;

        public StationDetailView(Guid id, string name, string code, int version)
        {
            Id = id;
            Name = name;
            Code = code;
            Version = version;
        }
    }
}

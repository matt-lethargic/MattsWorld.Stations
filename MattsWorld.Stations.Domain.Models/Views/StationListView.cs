using System;

namespace MattsWorld.Stations.Domain.Models.Views
{
    public class StationListView : View
    {
        public string Name;
        public string Code;

        public StationListView(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
using System;
using MattsWorld.Stations.Domain.Models.Events.Stations;

namespace MattsWorld.Stations.Domain.Models.Entities
{
    public class Station : AggregateRoot
    {
        private Guid _id;

        public override Guid Id => _id;

        public string Name { get; set; }
        public string Code { get; set; }

        public Station() { }

        public Station(Guid id, string name, string code)
        {
            ApplyChanges(new StationCreated(id, name, code));
        }

        public void ChangeName(string newName)
        {
            if(string.IsNullOrEmpty(newName)) throw new ArgumentException(nameof(newName));
            ApplyChanges(new StationRenamed(_id, newName));
        }


        private void Apply(StationCreated stationCreated)
        {
            _id = stationCreated.Id;
            Name = stationCreated.Name;
            Code = stationCreated.Code;
        }

        private void Apply(StationRenamed stationRenamed)
        {
            _id = stationRenamed.Id;
            Name = stationRenamed.NewName;
        }
    }
}
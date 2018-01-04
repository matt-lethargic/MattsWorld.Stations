using System;

namespace MattsWorld.Stations.Domain.Models.Commands.Stations
{
    public class CreateStation : Command
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Code { get; }

        public CreateStation(Guid id, string name, string code)
        {
            if (id == Guid.Empty) throw new ArgumentException(nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(code)) throw new ArgumentException(nameof(code));

            Id = id;
            Name = name;
            Code = code;
        }
    }
}

using System;

namespace MattsWorld.Stations.Domain.Models.Commands.Stations
{
    public class ImportStation : Command
    {
        public string Name { get; }
        public string Code { get; }

        public ImportStation(string name, string code)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(code)) throw new ArgumentException(nameof(code));
            
            Name = name;
            Code = code;
        }
    }
}

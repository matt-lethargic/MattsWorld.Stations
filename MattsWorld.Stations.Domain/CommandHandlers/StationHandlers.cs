using System;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Models.Commands.Stations;
using MattsWorld.Stations.Domain.Models.Entities;
using MattsWorld.Stations.Domain.Ports;

namespace MattsWorld.Stations.Domain.CommandHandlers
{
    public class StationHandlers
    {
        private readonly IRepository<Station> _repository;

        public StationHandlers(IRepository<Station> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CreateStation command)
        {
            var item = new Station(command.Id, command.Name, command.Code);
            await _repository.Save(item, -1);
        }

        public async Task Handle(RenameStation command)
        {
            var item = await _repository.GetById(command.Id);
            item.ChangeName(command.NewName);
            await _repository.Save(item, command.OriginalVersion);
        }
    }
}

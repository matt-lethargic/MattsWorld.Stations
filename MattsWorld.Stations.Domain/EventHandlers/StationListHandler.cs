using System;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Interfaces;
using MattsWorld.Stations.Domain.Models.Events.Stations;
using MattsWorld.Stations.Domain.Models.Views;
using MattsWorld.Stations.Domain.Ports;

namespace MattsWorld.Stations.Domain.EventHandlers
{
    public class StationListHandler :
        IHandles<StationCreated>,
        IHandles<StationRenamed>
    {
        private readonly IViewStore _viewStore;

        public StationListHandler(IViewStore viewStore)
        {
            _viewStore = viewStore ?? throw new ArgumentNullException(nameof(viewStore));
        }

        public async Task Handle(StationCreated message)
        {
            await _viewStore.Add(new StationListView(message.Id, message.Name, message.Code));
        }

        public async Task Handle(StationRenamed message)
        {
            var station = _viewStore.Get<StationListView>(message.Id);
            station.Name = message.NewName;

            await _viewStore.Update(station);
        }
    }
}
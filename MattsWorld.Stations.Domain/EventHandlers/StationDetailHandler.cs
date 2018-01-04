using System;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Interfaces;
using MattsWorld.Stations.Domain.Models.Events.Stations;
using MattsWorld.Stations.Domain.Models.Views;
using MattsWorld.Stations.Domain.Ports;

namespace MattsWorld.Stations.Domain.EventHandlers
{
    public class StationDetailHandler : 
        IHandles<StationCreated>,
        IHandles<StationRenamed>
    {
        private readonly IViewStore _viewStore;

        public StationDetailHandler(IViewStore viewStore)
        {
            _viewStore = viewStore ?? throw new ArgumentNullException(nameof(viewStore));
        }
        
        public async Task Handle(StationCreated message)
        {
            await _viewStore.Add(new StationDetailView(message.Id, message.Name, message.Code, message.Version));
        }

        public async Task Handle(StationRenamed message)
        {
            var station = _viewStore.Get<StationDetailView>(message.Id);
            station.Name = message.NewName;

            await _viewStore.Update(station);
        }
    }
}

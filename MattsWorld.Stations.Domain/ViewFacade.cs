using System;
using MattsWorld.Stations.Domain.Models.Views;
using MattsWorld.Stations.Domain.Ports;

namespace MattsWorld.Stations.Domain
{
    public class ViewFacade : IViewFacade
    {
        private readonly IViewStore _viewStore;

        public ViewFacade(IViewStore viewStore)
        {
            _viewStore = viewStore ?? throw new ArgumentNullException(nameof(viewStore));
        }

        public StationDetailView GetStation(Guid id)
        {
            return _viewStore.Get<StationDetailView>(id);
        }
    }
}
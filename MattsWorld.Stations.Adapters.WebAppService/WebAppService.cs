using MattsWorld.Stations.Adapters.CosmosRepository;
using MattsWorld.Stations.Adapters.CosmosViewStore;
using MattsWorld.Stations.Domain;
using MattsWorld.Stations.Domain.CommandHandlers;
using MattsWorld.Stations.Domain.EventHandlers;
using MattsWorld.Stations.Domain.Interfaces;
using MattsWorld.Stations.Domain.Models.Commands.Stations;
using MattsWorld.Stations.Domain.Models.Entities;
using MattsWorld.Stations.Domain.Models.Events.Stations;

namespace MattsWorld.Stations.Adapters.WebAppService
{
    public class WebAppService
    {
        public ICommandSender Bus { get; }
        public IEventPublisher Publisher { get; }
        public IViewFacade ViewFacade { get; }

        public WebAppService(WebAppSettings settings)
        {
            var viewStore = new CosmosStore(settings.CosmosEndpoint, settings.CosmosAuthKey, settings.DatabaseId, settings.ViewCollectionId);
            var stationDetailHandler = new StationDetailHandler(viewStore);
            var stationListHandler = new StationListHandler(viewStore);

            var publisher = new LocalPublisher();
            publisher.RegisterHandler<StationCreated>(stationDetailHandler.Handle);
            publisher.RegisterHandler<StationRenamed>(stationListHandler.Handle);

            var stationRepository = new CosmosEventRepository<Station>(settings.CosmosEndpoint, settings.CosmosAuthKey, settings.DatabaseId, settings.EventCollectionId, publisher);
            var stationCommandHandlers = new StationHandlers(stationRepository);

            var bus = new LocalBus();
            bus.RegisterHandler<CreateStation>(stationCommandHandlers.Handle);
            bus.RegisterHandler<RenameStation>(stationCommandHandlers.Handle);

            Bus = bus;
            Publisher = publisher;
            ViewFacade = new ViewFacade(viewStore);
        }
    }
}
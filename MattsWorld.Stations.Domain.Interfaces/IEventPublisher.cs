using System.Threading.Tasks;

namespace MattsWorld.Stations.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
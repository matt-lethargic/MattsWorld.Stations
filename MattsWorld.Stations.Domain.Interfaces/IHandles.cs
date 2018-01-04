using System.Threading.Tasks;

namespace MattsWorld.Stations.Domain.Interfaces
{
    public interface IHandles<in T>
    {
        Task Handle(T message);
    }
}
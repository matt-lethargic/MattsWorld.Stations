using MattsWorld.Stations.Domain.Interfaces;

namespace MattsWorld.Stations.Domain.Ports
{
    public interface IAppService
    {
        ICommandSender Bus { get; }
    }
}

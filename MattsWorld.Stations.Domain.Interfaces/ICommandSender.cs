using System.Threading.Tasks;

namespace MattsWorld.Stations.Domain.Interfaces
{
    public interface ICommandSender
    {
        Task Send<T>(T command) where T : ICommand;
    }
}
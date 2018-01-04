using System;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Models.Views;

namespace MattsWorld.Stations.Domain.Ports
{
    public interface IViewStore
    {
        T Get<T>(Guid id) where T : View;
        Task Add<T>(T view) where T : View;
        Task Update<T>(T view) where T : View;
    }
}
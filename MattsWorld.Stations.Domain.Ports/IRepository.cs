using System;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Models.Entities;

namespace MattsWorld.Stations.Domain.Ports
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        Task Save(AggregateRoot aggregate, int expectedVersion);
        Task<T> GetById(Guid id);
    }
}
using System;
using MattsWorld.Stations.Domain.Models.Views;

namespace MattsWorld.Stations.Domain
{
    public interface IViewFacade
    {
        StationDetailView GetStation(Guid id);
    }
}
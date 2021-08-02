using FoodTrucks.Api.Models;

namespace FoodTrucks.Api.Services
{
    /// <summary>
    /// Simple key-value store.
    /// </summary>
    public interface IRecentCache
    {
        void CacheTruck(FoodTruck truck);
        FoodTruck GetTruck(long id);
    }
}
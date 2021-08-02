using System.Collections.Generic;
using FoodTrucks.Api.Models;

namespace FoodTrucks.Api.Services
{
    public class RecentCache : IRecentCache
    {
        private Dictionary<long, FoodTruck> _recents = new Dictionary<long, FoodTruck>();

        public FoodTruck GetTruck(long id)
        {
            if (_recents.TryGetValue(id, out var result))
            {
                return result;
            }
            return null;
        }

        public void CacheTruck(FoodTruck truck)
        {
            _recents.Add(truck.LocationId, truck);
        }
    }
}

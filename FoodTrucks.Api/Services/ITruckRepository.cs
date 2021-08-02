using System;
using System.Collections.Generic;
using FoodTrucks.Api.Models;

namespace FoodTrucks.Api.Services
{
    public interface ITruckRepository
    {
        IEnumerable<FoodTruck> GetTrucks();
        FoodTruck FindById(long id);
        FoodTruck FindByAltId(Guid id);
        IEnumerable<FoodTruck> FindByBlock(string block);
        void AddTruck(FoodTruck truck);
    }
}
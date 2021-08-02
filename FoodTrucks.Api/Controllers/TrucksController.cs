using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodTrucks.Api.Models;
using FoodTrucks.Api.Services;

namespace FoodTrucks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrucksController : ControllerBase
    {
        private readonly ILogger<TrucksController> _logger;
        private readonly ITruckRepository _truckRepo;
        private readonly IRecentCache _recentCache;

        public TrucksController(ILogger<TrucksController> logger, ITruckRepository truckRepo, IRecentCache recentCache)
        {
            _logger = logger;
            _truckRepo = truckRepo;
            _recentCache = recentCache;
        }

        [HttpGet]
        public IEnumerable<FoodTruck> Get(int start = 0, int count = 10, string block = null)
        {
            start = start < 0 ? 0 : start;
            count = count < 0 ? 10 : count;

            if (!string.IsNullOrWhiteSpace(block))
            {
                return _truckRepo.FindByBlock(block)
                    .Skip(start)
                    .Take(count);
            }

            return _truckRepo.GetTrucks()
                .Skip(start)
                .Take(count);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<FoodTruck> GetById(long id)
        {
            var found = _recentCache.GetTruck(id);
            if (found != null)
            {
                return found;
            }

            var truck = _truckRepo.FindById(id);
            if (truck == null)
            {
                return NotFound();
            }
            _recentCache.CacheTruck(truck);
            return truck;
        }

        [HttpPost]
        public void AddTruck(FoodTruck truck)
        {
            var found = _truckRepo.FindByAltId(truck.AltId);
            if (found != null)
            {
                // Idempotence check. Chose to overwrite, but could throw instead.
                found.CopyNonIdFields(truck);
            }
            else
            {
                _truckRepo.AddTruck(truck);
            }
        }
    }
}

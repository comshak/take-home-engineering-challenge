using System;
using System.Linq;
using FoodTrucks.Api.Controllers;
using FoodTrucks.Api.Models;
using FoodTrucks.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FoodTrucks.Test
{
    public class ApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RecentCache_Should_Cache_Truck()
        {
            var rnd = new Random();
            var randomId = rnd.Next(1, 100);
            var sut = new RecentCache();

            var found = sut.GetTruck(randomId);
            Assert.IsNull(found);

            sut.CacheTruck(new FoodTruck
            {
                LocationId = randomId,
                AltId = Guid.NewGuid()
            });
            found = sut.GetTruck(randomId);
            Assert.IsNotNull(found);
        }

        [Test]
        [TestCase("badblock", 0)]
        [TestCase("0140", 3)]
        public void Repository_Should_FindByBlock(string block, long expectedCount)
        {
            var sut = new TruckRepository();
            var trucks = sut.FindByBlock(block).ToList();

            Assert.AreEqual(expectedCount, trucks.Count);
        }

        [Test]
        [TestCase(-1, false)]
        [TestCase(812017, true)]
        public void Controller_Should_GetById(long id, bool expected)
        {
            var logger = new NullLogger<TrucksController>();
            var repo = new TruckRepository();
            var cache = new RecentCache();
            var sut = new TrucksController(logger, repo, cache);
            var result = sut.GetById(id);
            if (expected)
            {
                Assert.IsNotNull(result.Value);
            }
            else
            {
                Assert.IsTrue(result.Result.GetType() == typeof(NotFoundResult));
            }
        }
    }
}
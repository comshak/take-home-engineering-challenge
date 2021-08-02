using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FoodTrucks.Api.Models;

namespace FoodTrucks.Api.Services
{
    public class TruckRepository : ITruckRepository
    {
        private static readonly List<FoodTruck> AllTrucks;

        static TruckRepository()
        {
            AllTrucks = ReadCsv();
        }

        public IEnumerable<FoodTruck> GetTrucks()
        {
            return AllTrucks;
        }

        public FoodTruck FindById(long id)
        {
            return AllTrucks.SingleOrDefault(x => x.LocationId == id);
        }

        public IEnumerable<FoodTruck> FindByBlock(string block)
        {
            return AllTrucks.Where(x => x.Block == block);
        }

        public FoodTruck FindByAltId(Guid id)
        {
            return AllTrucks.SingleOrDefault(x => x.AltId == id);
        }

        private static List<FoodTruck> ReadCsv(string filename = "Mobile_Food_Facility_Permit.csv")
        {
            var results = new List<FoodTruck>();
            using (var sr = new StreamReader(filename))
            {
                var headerLine = sr.ReadLine();
                if (headerLine == null)
                {
                    return results;
                }
                var data = headerLine.Split(',');
                var ixLid = FindToken(data, "locationid");
                var ixApp = FindToken(data, "Applicant");
                var ixBlk = FindToken(data, "block");
                var ixTyp = FindToken(data, "FacilityType");
                var ixLat = FindToken(data, "Latitude");
                var ixLon = FindToken(data, "Longitude");

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data = line.Split(',');

                    var truck = new FoodTruck
                    {
                        LocationId = Convert.ToInt64(data[ixLid]),
                        Block = data[ixBlk],
                        Applicant = data[ixApp],
                        Latitude = GetDecimal(data[ixLat], 0.0M),
                        Longitude = GetDecimal(data[ixLon], 0.0M),
                        Type = data[ixTyp],
                        AltId = Guid.NewGuid()
                    };
                    results.Add(truck);
                }
            }
            return results;
        }

        private static decimal GetDecimal(string repr, decimal defaultValue)
        {
            if (!decimal.TryParse(repr, out decimal result))
            {
                result = defaultValue;
            }
            return result;
        }

        private static int FindToken(IEnumerable<string> tokens, string targetToken)
        {
            var index = -1;
            foreach (var token in tokens)
            {
                index++;
                if (string.Equals(token, targetToken))
                {
                    return index;
                }
            }
            return index;
        }

        public void AddTruck(FoodTruck truck)
        {
            var maxLoc = AllTrucks.Max(x => x.LocationId);
            truck.LocationId = maxLoc + 1;

            AllTrucks.Add(truck);
        }
    }
}
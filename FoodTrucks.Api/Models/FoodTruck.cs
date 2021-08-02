using System;

namespace FoodTrucks.Api.Models
{
    public class FoodTruck
    {
        public long LocationId { get; set; }
        public string Applicant { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Type { get; set; }
        public string Block { get; set; }
        public Guid AltId { get; set; }

        public override string ToString()
        {
            return $"{LocationId}. Block={Block}";
        }

        public void CopyNonIdFields(FoodTruck source)
        {
            Block = source.Block;
            Applicant = source.Applicant;
            Longitude = source.Longitude;
            Latitude = source.Latitude;
            Type = source.Type;
        }
    }
}

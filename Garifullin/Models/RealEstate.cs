using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class RealEstate
    {
        public int Id { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet { get; set; }
        public int? AddressHouse { get; set; }
        public int? AddressNumber { get; set; }
        public decimal? CoordinateLatitude { get; set; }
        public decimal? CoordinateLongitude { get; set; }

        public int? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public int? HouseId { get; set; }
        public House House { get; set; }

        public int? LandId { get; set; }
        public Land Land { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }

        public ICollection<Supply> Supplies { get; set; }
    }

}

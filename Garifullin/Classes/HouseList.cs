using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Classes
{
    public class HouseList
    {
        public int Id { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet { get; set; }
        public int? AddressHouse { get; set; }
        public int? AddressNumber { get; set; }
        public int? TotalFloors { get; set; }
        public decimal? TotalArea { get; set; }
        public int? Rooms { get; set; }
    }
}

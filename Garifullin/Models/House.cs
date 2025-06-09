using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Garifullin.Models
{
    public class House
    {
        public int Id { get; set; }
        public int? TotalFloors { get; set; }
        public decimal? TotalArea { get; set; }
        public int? Rooms { get; set; }
        public RealEstate RealEstate { get; set; }
    }
}

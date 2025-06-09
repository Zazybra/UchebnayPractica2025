using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Classes
{
    public class LandDemandList
    {
        public int Id { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet { get; set; }
        public int? AddressHouse { get; set; }
        public int? AddressNumber { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public int AgentId { get; set; }
        public int ClientId { get; set; }
        public int TypeId { get; set; }
    }
}

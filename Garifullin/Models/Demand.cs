using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class Demand
    {
        public int Id { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet { get; set; }
        public int? AddressHouse { get; set; }
        public int? AddressNumber { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

        public int AgentId { get; set; }
        public Agent Agent { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int? ApartmentDemandId { get; set; }
        public ApartmentDemand ApartmentDemand { get; set; }

        public int? HouseDemandId { get; set; }
        public HouseDemand HouseDemand { get; set; }

        public int? LandDemandId { get; set; }
        public LandDemand LandDemand { get; set; }
        public int TypeId { get; set; }
        public RealEstateType RealEstateType { get; set; }

        public Deal Deal { get; set; }
    }

}

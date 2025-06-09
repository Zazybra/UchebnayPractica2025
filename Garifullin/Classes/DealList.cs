using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garifullin.Models;

namespace Garifullin.Classes
{
    public class DealList
    {
        public int Id { get; set; }
        public int DemandId { get; set; } 
        public int SupplyId { get; set; }
        public int RealEstateId { get; set; }
        public string? demandAddress { get; set; }
        public string? RealEstateAddress { get; set; }
    }
}

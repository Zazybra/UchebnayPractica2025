using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public int AgentId { get; set; }
        public Agent Agent { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int RealEstateId { get; set; }
        public RealEstate RealEstate { get; set; }

        public Deal Deal { get; set; }
    }

}

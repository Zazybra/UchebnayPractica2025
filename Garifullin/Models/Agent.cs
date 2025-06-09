using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public decimal? DealShare { get; set; }

        public ICollection<Supply> Supplies { get; set; }
        public ICollection<Demand> Demands { get; set; }


    }
}

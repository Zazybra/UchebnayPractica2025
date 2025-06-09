using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class RealEstateType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Demand> Demands { get; set; }
    }
}

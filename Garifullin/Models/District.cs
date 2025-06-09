using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.Models
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public ICollection<RealEstate> RealEstates { get; set; }
    }
}

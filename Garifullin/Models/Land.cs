using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Garifullin.Models
{
    public class Land
    {
        public int Id { get; set; }
        public decimal? TotalArea { get; set; }
        public RealEstate RealEstate { get; set; }
    }
}

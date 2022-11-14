using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarVersion
    {
        public int ID { get; set; }
        public int SeriesId { get; set; }
        public string Name { get; set; }
        public int ListingYear { get; set; }
        public int HorsePower { get; set; }
        public int MaximumPower { get; set; }
        public int EngineSize { get; set; }
        public decimal EngineCapacity { get; set; }
        public string Url { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarModel
    {
        public int ID { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int? ListingYear { get; set; }
        public int? Generation { get; set; }
        public int? Version { get; set; }
        public int? Model { get; set; }
        public string? ImageUrl { get; set; }
        public string? Url { get; set; }
    }
}
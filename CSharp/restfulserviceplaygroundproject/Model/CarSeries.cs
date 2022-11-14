using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarSeries
    {
        public int ID { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
    }
}
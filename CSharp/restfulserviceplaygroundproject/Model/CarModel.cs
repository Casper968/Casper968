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
        public int? Versions { get; set; }
        public int? Models { get; set; }
    }
}
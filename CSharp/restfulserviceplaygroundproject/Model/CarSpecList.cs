using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarSpecList
    {
        public string Name { get; set; }
        public List<CarEngineSpec> Engine { get; set; }
        public List<CarFuelSpec> Fuel { get; set; }
        public List<CarPrefSpec> Performance { get; set; }
        public List<CarSizeSpec> Size { get; set; }
    }
}
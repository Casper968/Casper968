using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarEngineSpec : CarSpec
    {

    }

    public class CarSizeSpec : CarSpec
    {

    }

    public class CarPrefSpec : CarSpec
    {

    }

    public class CarFuelSpec : CarSpec
    {
        
    }

    public class CarSpec
    {
        public int ID { get; set; }
        public int VersionId { get; set; }
        public string SpecName { get; set; }
        public string SpecValue { get; set; }
    }
}
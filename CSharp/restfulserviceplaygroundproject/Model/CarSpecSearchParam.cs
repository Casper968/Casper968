using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Model
{
    public class CarSpecSearchParam
    {
        public string? name { get; set; }
        public int? speed { get; set; } 
        public int? acceleration { get; set; }
        public int? engineSize { get; set; }
        public int? range { get; set; }
        public int? doors { get; set; }
        public int? seats { get; set; }
        public string? spec { get; set; }
        public string? specValue { get; set; }
    }
}
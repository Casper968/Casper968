using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Infrastructure
{
    public class Result<TValue> : Result
    {
        public TValue? Data { get; }

        protected internal Result(TValue? value, bool success, string? message = null) 
            : base(success, message)
        {
            Data = value;
        }
    }
}
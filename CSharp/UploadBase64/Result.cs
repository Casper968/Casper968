using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.UploadBase64
{
    public class Result
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Result() { }
        public Result(T data)
        {
            Success = true;
            Status = 0;
            Message = "Success";
            Data = data;
        }
    }
}
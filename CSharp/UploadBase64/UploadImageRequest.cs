using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.UploadBase64
{
    public class UploadImageRequest
    {
        public string FileName { get; set; }
        
        public string ImageBase64String { get; set; }
    }
}
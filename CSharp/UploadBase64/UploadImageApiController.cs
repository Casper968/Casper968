using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.UploadBase64
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadImageApiController : ControllerBase
    {
        private readonly ImageFileUploadService _imageUploadService;
        private readonly string _uploadRootPath = "";
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public UploadImageApiController(
            ImageFileUploadService imageFileUploadService,
            IWebHostEnvironment hostEnvironment)
        {
            this._imageUploadService = imageFileUploadService;
            this._hostingEnvironment = hostEnvironment;
            // get application folder or read path from application settings.
            this._uploadRootPath = this._hostingEnvironment.ContentRootPath;
        }

        [HttpPost("UploadBase64String")]
        public async Task<ActionResult> UploadBase64String(UploadImageRequest req)
        {
            if (!string.IsNullOrEmpty(req.ImageBase64String))
            {
                // give default image in case its empty
                string fileName = req.FileName ?? "DefaultImage";
                // middle path if needed
                string folderPath = "Product";
                string _uploadRootPath = Path.Combine(_uploadRootPath, folderPath);
                // start to call image upload service and save image file
                var imageUploadedResponse = await this._imageUploadService.SaveBase64Image(_uploadRootPath, "", req.ImageBase64String, fileName);
                if (!imageUploadedResponse.Success)
                {
                    return BadRequest("Upload failed");
                }

                return Ok("Upload finished");
            }
        }
    }
}
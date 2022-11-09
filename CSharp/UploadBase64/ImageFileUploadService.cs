using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.UploadBase64
{
    public class ImageFileUploadService
    {
        // white list of file type allow to upload
        private readonly static Dictionary<string, string> _allowImageTypes = new Dictionary<string, string>
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" }
        };

        public async Task<Result<string>> SaveBase64Image(string _folder, string prefix, string base64String, string fileName = "")
        {
            var result = new Result<string>();
            try
            {
                #region Check Image Data Format
                // first thing first
                // checkout base 64 string context
                if (!base64String.Contains("base64"))
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "base64 format incorrect",
                    };
                }

                var spilitForBase64 = base64String.Split(";base64,");
                if (spilitForBase64.Count() < 2)
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "base64 format incorrect",
                    };
                }

                // check string format can obtain image/{file type}
                var spilitForImageType = spilitForBase64[0].Split("/");
                if (spilitForImageType.Count() < 2)
                {
                    result = new Result<string>
                    {
                        Success = false,
                        Message = "base64 format incorrect",
                    };

                    return result;
                }

                // whether file type is allow or not
                if (!_contentTypes.ContainsKey($".{spilitForImageType[1]}".ToLower()))
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "file type not allow"
                    };
                }
                #endregion

                // assemble outer file name and file type from base 64 string
                var fullFilename = fileName + "." + spilitForImageType[1];

                // start to write image into physic folder
                byte[] bytes = Convert.FromBase64String(spilitForBase64[1]);
                using (MemoryStream ms = new MemoryStream(bytes))
                using (Image image = Image.Load(ms))
                {
                    // assemble middle path
                    var directionPath = Path.Combine(_folder, prefix);

                    // check whether need to create folder for the first time
                    bool exists = Directory.Exists(directionPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(directionPath);
                    }
                    var filePath = Path.Combine(directionPath, fullFilename);

                    // save the image at last
                    await image.SaveAsync(filePath);
                }                
                result = new Result<string>(fullFilename);
            }
            catch (Exception ex)
            {
                return new Result<string>
                {
                    Success = false,
                    Message = "upload failed"
                };
            }
            return result;
        }
    }
}
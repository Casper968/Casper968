using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Cryptography.MD5
{
    public static class CryptographyMD5
    {
        public static string ConvertToMD5(this string source)
        {
            using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
            {
                // convert source string to utf8 bytes
                var bytes = Encoding.UTF8.GetBytes(source);

                // convert bytes to hash
                var hash = cryptoMD5.ComputeHash(bytes);

                // obtain exca MD5
                var md5 = BitConverter.ToString(hash).Replace("-", String.Empty).ToUpper();
            }
        }

        // var password = "unicorn";
        // var encrypted = password.ConvertToMD5();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public  class DecodeBase64
    {
        public static byte[] Decode(string data) {

            byte[] decodeData;

            try
            {
                decodeData = Convert.FromBase64String(data);

            }
            catch (FormatException e)
            {

                // If the not base64 string is invalid, reject the request.
                // and return a 400 Bad Request status code.
                throw new ArgumentException("Invalid Base64 string.", nameof(data));
            }
            return decodeData;

        }
    }
}

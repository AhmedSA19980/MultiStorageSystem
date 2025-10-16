using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Utills
{
    public  class BuildStringToSign
    {
        public static string  stringToSign(string algorithm , string amzDate ,string credentialScope ,string hashCanonicalRequest) =>
            $"{algorithm}\n{amzDate}\n{credentialScope}\n{hashCanonicalRequest}";
    }
}

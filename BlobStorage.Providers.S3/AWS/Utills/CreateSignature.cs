using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Utills
{
    public  class Signature
    {
       
        public static string CreateSignature(string stringToSign, string secretKey ,string dateStamp , string region ,string service)
        {
            
            byte[] kDate = AwsV4Signer.HmacSHA256(Encoding.UTF8.GetBytes("AWS4" + secretKey), dateStamp);
            byte[] kRegion = AwsV4Signer.HmacSHA256(kDate, region);
            byte[] kService = AwsV4Signer.HmacSHA256(kRegion, service);
            byte[] kSigning = AwsV4Signer.HmacSHA256(kService, "aws4_request");
            string signature = BitConverter.ToString(AwsV4Signer.HmacSHA256(kSigning, stringToSign)).Replace("-", "").ToLower();
            return signature;

        }

    }
}

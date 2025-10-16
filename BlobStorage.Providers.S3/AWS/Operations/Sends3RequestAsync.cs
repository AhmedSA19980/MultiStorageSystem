using BlobStorage.Providers.S3.AWS.Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Frozen;
namespace BlobStorage.Providers.S3.AWS.Operations
{
    public class Sends3RequestAsync
    {
        public static async Task<HttpResponseMessage> SendS3RequestAsync(
            string bucket,
            string objectKey,
            string accessKey,
            string secretKey,
            HttpMethod method,
            string region= "us-east-1",
            byte[] data = null,
             string endpoint = "http://localhost:9000",
             bool usePathStyle = true
          
            ) {

        
            //var host = $"{bucket}.s3.{region}.amazonaws.com";
            //var  host = "localhost:9000";// // for Minio
            //var endpoint = $"https://{host}/{objectKey}";
            //  var endpoint = $"http://{host}/{bucket}/{objectKey}"; // for Minio


            var service = "s3";
            var uri = new Uri(endpoint);
            var host = uri.Host + (uri.IsDefaultPort ? "" : $":{uri.Port}");


            string requestUrl = usePathStyle
            ? $"{endpoint.TrimEnd('/')}/{bucket}/{objectKey}"
            : $"{uri.Scheme}://{bucket}.{host}/{objectKey}";


            var now = DateTime.UtcNow;
            var amzDate = now.ToString("yyyyMMddTHHmmssZ");
            var dateStamp = now.ToString("yyyyMMdd");


        
            var canonicalUri =usePathStyle ? $"/{bucket}/{objectKey}" : $"/{objectKey}";
            var canonicalQueryString = "";

            var payload = data ?? Array.Empty<byte>();
            var payloadHash = AwsV4Signer.HashHex(payload);
            
            var canonicalHeaders = $"host:{host}\nx-amz-content-sha256:{payloadHash}\nx-amz-date:{amzDate}\n";
            var signedHeaders = "host;x-amz-content-sha256;x-amz-date";

         
            var canonicalRequest = CanonicalRequest.BuildCanonicalRequest(method.Method, canonicalUri, canonicalQueryString, canonicalHeaders, signedHeaders, payloadHash);

            var algorithm = "AWS4-HMAC-SHA256";
            var credentialScope = $"{dateStamp}/{region}/{service}/aws4_request";

            byte[] encodeByte = Encoding.UTF8.GetBytes(canonicalRequest);
            var hashCanonicalRequest = AwsV4Signer.HashHex(encodeByte);
            
            var stringToSign = BuildStringToSign.stringToSign(algorithm, amzDate, credentialScope, hashCanonicalRequest);
            var signature = Signature.CreateSignature(stringToSign, secretKey, dateStamp, region, service);
            var authHeader = AuthorizationHeader.BuildAuthorizationHeader(algorithm, accessKey, credentialScope, signedHeaders, signature);



            using var client = new HttpClient();
            var request = new HttpRequestMessage(method, requestUrl);
            if(data != null && data.Length > 0)
            {
                request.Content = new ByteArrayContent(data);
            }
         


            request.Headers.Add("x-amz-date", amzDate);
            request.Headers.Add("x-amz-content-sha256", payloadHash);
            request.Headers.TryAddWithoutValidation("Authorization", authHeader);


            var response = await client.SendAsync(request);
            //Console.WriteLine($"🔑 Authorization Header: {request.Headers.GetValues("Authorization").FirstOrDefault()}");

            return response;

        }
    }
}

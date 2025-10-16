using BlobStorage.Providers.S3.AWS.Utills;
using System.Net.Http.Headers;

namespace BlobStorage.Providers.S3.AWS.Operations
{
    public class UploadTos3Async
    {

        public static async Task UploadToS3Async(string bucket ,
            string objectKey ,
            byte[] data,
            string accessKey,
            string secretKey,
            string region = "us-ease-1") {


          

            var response = await Sends3RequestAsync.SendS3RequestAsync( bucket, objectKey , accessKey ,secretKey, HttpMethod.Put, region, data);
            Console.WriteLine( response );
          
            if (!response.IsSuccessStatusCode) { throw new Exception($"S3 Upload failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"); }


        }
    }
    
}

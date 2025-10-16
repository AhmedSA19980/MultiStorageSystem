using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Operations
{
    public  class DeleteFroms3Async
    {
        public static async Task DeleteFromS3Async(string bucket, string key, string accessKey, string secretKey, string region = "us-east-1")
        {
            var response = await Sends3RequestAsync.SendS3RequestAsync(bucket, key, accessKey, secretKey,  HttpMethod.Delete , region);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"S3 DELETE failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            
        }
    }
}

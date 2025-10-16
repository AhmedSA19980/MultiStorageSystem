using BlobStorage.Providers.S3.AWS.Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlobStorage.Providers.S3.AWS.Operations
{
    public  class GetFroms3Async
    {

        public static async Task<byte[]> GetFromS3Async(string bucket,
            string objectKey,
            string accessKey,
            string secretKey,
            string region = "us-east-1" 
            ) {

         

            var response = await Sends3RequestAsync.SendS3RequestAsync(bucket , objectKey ,accessKey , secretKey, HttpMethod.Get ,region);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"S3 GET failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            return  await response.Content.ReadAsByteArrayAsync();

        }


    }
}

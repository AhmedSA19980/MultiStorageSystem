using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Providers.S3.AWS.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Operations
{
    public  class MetadataFroms3Async
    {
      
          public static async Task<RetrieveBlobDTO> GetMetadataFroms3Async(string bucket,
                string objectKey,
                string accessKey,
                string secretKey,
                string region = "us-east-1" 
                )
            {


             
                var response = await Sends3RequestAsync.SendS3RequestAsync(bucket, objectKey, accessKey, secretKey, HttpMethod.Get, region);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"S3 GET failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();

                string base64 = Convert.ToBase64String( bytes );


                long size = 0;
                DateTimeOffset datetime = DateTimeOffset.Now;
                if (response.Content.Headers.ContentLength.HasValue) size = response.Content.Headers.ContentLength.Value;
                if(response.Content.Headers.LastModified.HasValue) datetime = response.Content.Headers.LastModified.Value;

                return new RetrieveBlobDTO
                {
                    Id =ConvertToGuid.ConvertStringToGuid( objectKey),
                    Data = bytes,
                    Size = size,
                    CreatedAtUtc = datetime


                };

            }


    }

    
}

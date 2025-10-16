using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Providers.S3.AWS.Operations;
using Microsoft.Extensions.Configuration;
using static BlobStorage.Core.Global.RefrencesName;

namespace BlobStorage.Providers.S3
{

    public class S3BlobProvider : IObjectStorage
    {
        public string Name => "S3";

        public EnRefrencesName StorageType =>EnRefrencesName.S3;

        public string Reference =>$"{_endpoint}/{_bucket}/{_objectKey}";

        private string? _objectKey;
        private readonly string _bucket;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _region;
        private readonly string _endpoint;

        public S3BlobProvider(IConfiguration config)
        {
            _bucket = config["Storage:S3:Bucket"];
            _accessKey = config["Storage:S3:AccessKey"];
            _secretKey = config["Storage:S3:SecretKey"];
            _region = config["Storage:S3:Region"] ?? "us-east-1";
            _endpoint = config["Storage:S3:Endpoint"] ?? "http://localhost:9000"; // change endpoint according to you config
        }


        public async Task<bool> DeleteAsync(Guid key)
        {
           
            try
            {
                Guid? id = key;
                string strId = id.HasValue ? id.Value.ToString() : "";
                await DeleteFroms3Async.DeleteFromS3Async(_bucket, strId, _accessKey, _secretKey, _region);
                return true;

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);    
                return false;
            }

        }

        public async Task<byte[]> GetAsync(Guid key)
        {
          
            Guid? id = key;
            string strId = id.HasValue ? id.Value.ToString() : "";
            var blob =  await GetFroms3Async.GetFromS3Async(_bucket, strId, _accessKey, _secretKey, _region);

            if (blob == Array.Empty<byte>()) return Array.Empty<byte>();
            return blob;
            
        }

        public async Task UploadAsync(Guid key, byte[] data)
        {
            try
            {
                string id = key.ToString();
                await UploadTos3Async.UploadToS3Async(_bucket, id , data ,_accessKey, _secretKey, _region);
                _objectKey = id;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Upload failed for key {key}: {ex}");
                throw ;

            }
        }

        public async Task<RetrieveBlobDTO> RetrieveBlobAsync(Guid Id)
        {
            Guid? id = Id;
            string strId = id.HasValue ? id.Value.ToString() : "";
            var blob = await MetadataFroms3Async.GetMetadataFroms3Async(_bucket, strId, _accessKey, _secretKey, _region);
           
            return new RetrieveBlobDTO
            {
                Id = Id,
                Data = blob.Data,
                Size = blob.Size,
                CreatedAtUtc=blob.CreatedAtUtc
            };


        }
    }
}
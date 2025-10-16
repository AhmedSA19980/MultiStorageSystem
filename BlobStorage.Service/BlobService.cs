using AutoMapper;
using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.Factory;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.Blob;
using BlobStorage.Service.DTOs.Blobmetadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Service
{
    public class BlobService
    {

        private readonly IObjectStorage   _provider;
        private readonly BlobMetadataService _blobMetadataService;
        private readonly IMapper _Mapper;  
        public BlobService(IObjectStorageProviderFactory factory,IConfiguration config, BlobMetadataService BlobMetadataService, IMapper mapper)
        {
            var providerType = config["Storage:Provider"];
            _provider = factory.GetProvider(providerType);
            _blobMetadataService = BlobMetadataService;
            _Mapper = mapper;
        }

      

        public async Task<BlobDTO> UploadAsync(string data , int userId)
        {
            byte[] decodeFile = DecodeBase64.Decode(data);
            var fileType = FileType.GetFileMimeTypeFromBytes(decodeFile);
            var uniqueKey = UniKey.GenerateUniqueKey();

            var uploadedBlobId = Guid.Empty;
      
            try {

             
                await _provider.UploadAsync(uniqueKey, decodeFile);
                uploadedBlobId = uniqueKey; 

                var BlobmetadataDto = new AddBlobMetadataDTO()
                {
                    SizeInBytes = decodeFile.Length,
                    UserId = userId,
                    StorageLocationName = _provider.StorageType,
                    LocationReference = _provider.Reference, 
                    MimeType = fileType,
                    BlobId = uniqueKey,
                };


                await _blobMetadataService.AddBlobmetadataAsync(BlobmetadataDto);

                return new BlobDTO { Id = uniqueKey , Data = decodeFile.ToString()  } ;
            }
            catch (Exception ex) {


                if (uploadedBlobId != Guid.Empty) {

                    Console.WriteLine($"Metadata failed for Blob ID {uploadedBlobId}. Executing compensation...");

                   await HandleCompensation(uploadedBlobId , _provider.StorageType.ToString() ,ex );
                }

                throw new Exception("File upload failed due to a stoage object/metadata error.", ex);
            }

        }

        public async Task<byte[]> GetAsync(Guid id)
        {
            var blob = await _provider.GetAsync(id);
            if(blob == null)
            {
                return null;
            }
            
            return blob;
        }


        public async Task<RetrieveBlobDTO> RetrieveBlobAsyn(Guid id)
        {
                var Blob = await _provider.RetrieveBlobAsync(id);

            return Blob;
        }

        private async Task HandleCompensation(Guid uploadedBlobId, string storageType, Exception originalEx)
        {
            try
            {
                Console.WriteLine($"Metadata failed for Blob ID {uploadedBlobId}. Executing compensation...");
                bool DeleteBlob = await _provider.DeleteAsync(uploadedBlobId);

            }
            catch (Exception compensationEx)
            {
                Console.Error.WriteLine($"CRITICAL ERROR: Compensation failed for {uploadedBlobId}. Orphaned file created. Reason: {compensationEx.Message}");
               
            }

        }
    }
}

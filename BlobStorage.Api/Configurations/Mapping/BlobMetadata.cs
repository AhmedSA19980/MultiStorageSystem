using AutoMapper;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs;
using BlobStorage.Service.DTOs.Blob;
using BlobStorage.Service.DTOs.Blobmetadata;

namespace BlobStorage.Api.Configurations.Mapping
{
    public class BlobMetadataMappingProfile : Profile
    {
        public BlobMetadataMappingProfile()
        {
          
      
            CreateMap<AddBlobMetadataDTO, BlobMetadata>().ReverseMap();
       
        }


    }
}

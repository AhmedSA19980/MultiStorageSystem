using AutoMapper;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs;
using BlobStorage.Service.DTOs.Blob;

namespace BlobStorage.Api.Configurations.Mapping
{
    public class BlobMappingProfile : Profile
    {
        public BlobMappingProfile()
        {
            CreateMap<BlobDTO, Blob>().ReverseMap();
          

        }


    }
}

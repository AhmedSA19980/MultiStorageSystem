using AutoMapper;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs;
using BlobStorage.Service.DTOs.User;

namespace BlobStorage.Api.Configurations.Mapping
{
    public class UserMappingProfile :Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserDTO, User>().ReverseMap();
            CreateMap<UpdateUserDTO, User>().ReverseMap();
            CreateMap<LoginDTO, User>().ReverseMap();
            CreateMap<ChangePasswordDTO, User>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
           
        }
            
        
    }
}

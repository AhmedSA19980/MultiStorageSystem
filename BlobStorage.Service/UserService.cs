using AutoMapper;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.User;

namespace BlobStorage.Service {

    public class UserService 
    
    {


    
        private readonly IUserRepository<User> _userRepository;

        private readonly IMapper _Mapper;

        public UserService(IUserRepository<User> userRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _Mapper = mapper;   
        }

        public async Task<int> AddAsync(CreateUserDTO UserDto)
        {

            var exisitingUser = await _userRepository.GetUserByEmailAsync(UserDto.Email);
            if (exisitingUser !=null ) {

                throw new Exception($"This {UserDto.Email} Email  is exist ");
            }


            var userEntity = _Mapper.Map<User>(UserDto);

        
            userEntity.Password = HashPass.hashPassword(userEntity.Password) ;
            UserDto.Password =  HashPass.hashPassword(userEntity.Password);
           
            await _userRepository.AddAsync(userEntity);

            return userEntity.Id;
  

        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var UserEntity = await _userRepository.GetByIdAsync(id);
            return UserEntity == null ? null : _Mapper.Map<UserDTO>(UserEntity);
           
        }

        public async Task UpdateAsync(UpdateUserDTO UserDto)
        {
            var userEntity = await _userRepository.GetByIdAsync(UserDto.ID);
            if (userEntity == null)
            {
                throw new KeyNotFoundException($"User with ID {UserDto.ID} not found.");
            }

            _Mapper.Map(UserDto, userEntity);
            await _userRepository.UpdateAsync(userEntity);
        }



        public async Task ChangePasswordAsync(ChangePasswordDTO UserDto)
        {
            var userEntity = await _userRepository.GetByIdAsync(UserDto.ID);
            if (userEntity == null)
            {
                throw new KeyNotFoundException($"User with ID {UserDto.ID} not found.");
            }

            _Mapper.Map(UserDto, userEntity);
        
            userEntity.Password = HashPass.hashPassword(userEntity.Password);
            UserDto.Password = HashPass.hashPassword(userEntity.Password);

            await _userRepository.ChangePasswordAsync(userEntity);
        }


    }
}


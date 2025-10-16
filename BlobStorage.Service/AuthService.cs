using AutoMapper;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.Auth;
using BlobStorage.Service.DTOs.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace BlobStorage.Service
{
    public class AuthService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<List<Claim>> GetUserClaims(int UserId  ,string Email)
        {
            var Claims = new List<Claim>() {

                    new Claim(ClaimTypes.NameIdentifier,UserId.ToString()),
                    new Claim(ClaimTypes.Name, Email)
              
            };

            return Claims;
        }
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var accessExpirationMinutes = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(accessExpirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<AuthResponseDTO> AuthenticateUserAndGenerateJwtToken(LoginDTO LoginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(LoginDto.Email);
            
            if (user == null || !VerifyPass.VerifyPassword(user.Password, LoginDto.Password)) return null;


            var claims = await GetUserClaims(user.Id, user.Email);
            var accessToken = GenerateAccessToken(claims);


            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                ExpiresIn = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes") * 60
            };


        }
    }
}

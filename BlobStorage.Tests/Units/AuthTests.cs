using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using BlobStorage.Service;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.User;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Global;



public class AuthServiceTests
{
    private readonly Mock<IUserRepository<User>> _userRepoMock;
    private readonly IConfiguration _config;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        //  Setup mock user repository
        _userRepoMock = new Mock<IUserRepository<User>>();

        //  Setup in-memory configuration
        var settings = new Dictionary<string, string>
        {
            ["Jwt:Key"] = "this_is_a_super_secure_32_char_secret_key!",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:AccessTokenExpirationMinutes"] = "7"
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        // 3️⃣ Initialize AuthService
        _authService = new AuthService(_userRepoMock.Object, _config);
    }

    [Fact]
    public async Task AuthenticateUserAndGenerateJwtToken_ShouldReturnToken_ForValidUser()
    {
    
      
        var login = new LoginDTO { Email = "test21@gmail.com", Password = "12345" };
        var user = new User { Id = 2, Email = login.Email, Password = HashPass.hashPassword(login.Password) };

        _userRepoMock.Setup(r => r.GetUserByEmailAsync(login.Email))
                     .ReturnsAsync(user);

       
        var result = await _authService.AuthenticateUserAndGenerateJwtToken(login);

    
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.True(result.ExpiresIn > 0);

        // Optional: decode token and verify claims
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(result.AccessToken);

        Assert.Equal("TestIssuer", jwt.Issuer);
        Assert.Contains(jwt.Claims, c => c.Type == "unique_name" && c.Value == login.Email);
        Assert.Contains(jwt.Claims, c => c.Type == "nameid" && c.Value == user.Id.ToString());

    }

    [Fact]
    public async Task AuthenticateUserAndGenerateJwtToken_ShouldReturnNull_ForInvalidPassword()
    {
       
        var login = new LoginDTO { Email = "user@x.com", Password = "wrong" };
        var user = new User { Id = 1, Email = "user@x.com", Password = "correct" };

        _userRepoMock.Setup(r => r.GetUserByEmailAsync(login.Email))
                     .ReturnsAsync(user);

     
        var result = await _authService.AuthenticateUserAndGenerateJwtToken(login);

    
        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateUserAndGenerateJwtToken_ShouldReturnNull_IfUserNotFound()
    {
        
        var login = new LoginDTO { Email = "missing@user.com", Password = "123" };
        _userRepoMock.Setup(r => r.GetUserByEmailAsync(login.Email))
                     .ReturnsAsync((User)null);

     
        var result = await _authService.AuthenticateUserAndGenerateJwtToken(login);

       
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserClaims_ShouldReturnExpectedClaims()
    {
       
        int userId = 1;
        string email = "test@example.com";

     
        var claims = await _authService.GetUserClaims(userId, email);

       
        Assert.Contains(claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
        Assert.Contains(claims, c => c.Type == ClaimTypes.Name && c.Value == email);
    }
}

using System.Threading.Tasks;
using BlobStorage.Core.Models;
using BlobStorage.Providers.Sql;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserRepTests
{
    private readonly AppDbContext _context;
    private readonly UserRep _repository;

    public UserRepTests()
    {
        // Setup EF Core InMemory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "set_your_db_connection_address")
            .Options;

        _context = new AppDbContext(options);
        _repository = new UserRep(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
    
        var user = new User { Id = 1,Username= "test" , Email = "test@example.com", Password = "12345" };

   
        await _repository.AddAsync(user);
        var result = await _repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnCorrectUser()
    {
       
        var user = new User { Id = 2, Username = "test2", Email = "email@test.com", Password = "pwd" };
        await _repository.AddAsync(user);

     
        var result = await _repository.GetUserByEmailAsync("email@test.com");

      
        Assert.NotNull(result);
        Assert.Equal("email@test.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyUser()
    {
     
        var user = new User { Id = 3, Username = "test3", Email = "old@mail.com", Password = "oldpwd" };
        await _repository.AddAsync(user);

       
        user.Email = "new@mail.com";
        await _repository.UpdateAsync(user);

        var updated = await _repository.GetByIdAsync(3);

       
        Assert.Equal("new@mail.com", updated.Email);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldUpdatePassword()
    {
       
        var user = new User { Id = 4, Username = "test4", Email = "changepwd@mail.com", Password = "oldpwd" };
        await _repository.AddAsync(user);

       
        user.Password = "newpwd";
        await _repository.ChangePasswordAsync(user);

        var updated = await _repository.GetByIdAsync(4);

       
        Assert.Equal("newpwd", updated.Password);
    }
}

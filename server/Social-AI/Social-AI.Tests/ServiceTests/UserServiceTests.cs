using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Social_AI.Models.Entities;
using Social_AI.Services;
using FluentAssertions;
using Social_AI.Models.DTOs;

namespace Social_AI.Tests;

public class UserServiceTests
{ 
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private async Task<SocialAiContext> SetupDbContext()
    {
        var options = new DbContextOptionsBuilder<SocialAiContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var databaseContext = new SocialAiContext(options);
        await databaseContext.Database.EnsureCreatedAsync();
        
        for (int i = 1; i <= 5; i++)
        {
            string password = "Test123";
            var hashedPassword = _passwordHasher.HashPassword(null, password);
            databaseContext.Add(
                new User
                {
                    ID = i,
                    Name = $"User{i}",
                    Email = $"user{i}@test.com",
                    Description = "Hi!",
                    Role = "User",
                    Password = hashedPassword,
                }
            );
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    
    private async Task<IUserService> SetupUserService()
    {
        var dbContext = await SetupDbContext();

        IUserService userService = new UserService(dbContext,_passwordHasher);

        return userService;
    }
    
    [Fact]
    public async Task UserService_ReturnsCorrectUserById()
    {
        var userService = await SetupUserService();
        long userId = 1;

        var result = await userService.GetUserById(userId);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(userId);
    }
    
    [Fact]
    public async Task UserService_ReturnsNullForIncorrectId()
    {
        var userService = await SetupUserService();
        long userId = 999;

        var result = await userService.GetUserById(userId);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UserService_AddsUserCorrectlyToDb()
    {
        var userService = await SetupUserService();
        User newUser = new User()
        {
            ID = 6,
            Name = "User6",
            Email = "user6@test.com",
            Password = "test123",
            Role = "User",
            Description = "I'm a user"
        };

        await userService.Add(newUser);
        var result = await userService.GetUserById(newUser.ID);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(newUser.ID);
        result.Should().BeEquivalentTo(newUser);
    }
    
    [Fact]
    public async Task UserService_ReturnsAllUsers()
    {
        var userService = await SetupUserService();

        var result = await userService.GetAll();
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<User>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task UserService_UpdatesUserCorrectly()
    {
        var userService = await SetupUserService();
        EditUserDTO DTO = new EditUserDTO()
        {
            Name = "EditUser",
            Password = "EditPass",
            Description = "EditDescription",
            Picture = null
        };
        long userId = 1;

        var result = await userService.Update(userId,DTO);

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task UserService_Update_ReturnsFalseWithIncorrectId()
    {
        var userService = await SetupUserService();
        EditUserDTO DTO = new EditUserDTO()
        {
            Name = "EditUser",
            Password = "EditPass",
            Description = "EditDescription",
            Picture = null
        };
        long userId = 999;

        var result = await userService.Update(userId,DTO);

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserService_DeletesUserWithCorrectId()
    {
        var userService = await SetupUserService();
        long userId = 1;

        var result = await userService.Delete(userId);
        var deletedUser = await userService.GetUserById(userId);
        result.Should().BeTrue();
        deletedUser.Should().BeNull();
    }
    
    [Fact]
    public async Task UserService_Delete_ReturnFalseWithIncorrectId()
    {
        var userService = await SetupUserService();
        long userId = 999;

        var result = await userService.Delete(userId);
        
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserService_GetByLogin_ReturnsCorrectUser()
    {
        var userService = await SetupUserService();
        string email = "user1@test.com";
        string password = "Test123";
        long userId = 1;

        var result = await userService.GetByLogin(email, password);
        var correctUser = await userService.GetUserById(userId);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(correctUser);
    }
    
    [Fact]
    public async Task UserService_GetByLogin_ReturnsFalseWithIncorrectCredentials()
    {
        var userService = await SetupUserService();
        string email = "user99@test.com";
        string password = "Test1234";
        long userId = 1;

        var result = await userService.GetByLogin(email, password);

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UserService_UserExistsByEmail_ReturnsTrueWithCorrectEmail()
    {
        var userService = await SetupUserService();
        string email = "user1@test.com";

        var result = userService.UserExistsByEmail(email);

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task UserService_UserExistsByEmail_ReturnsFalseWithInCorrectEmail()
    {
        var userService = await SetupUserService();
        string email = "user99@test.com";

        var result = userService.UserExistsByEmail(email);

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserService_UserExistsById_ReturnsFalseWithInCorrectId()
    {
        var userService = await SetupUserService();
        long userId = 999;

        var result = userService.UserExistsById(userId);

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserService_UserExistsById_ReturnsTrueWithCorrectId()
    {
        var userService = await SetupUserService();
        long userId = 1;

        var result = userService.UserExistsById(userId);

        result.Should().BeTrue();
    }
}
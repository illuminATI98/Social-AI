using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Tests;

public class LikeServiceTests
{
    private async Task<SocialAiContext> SetupDbContext()
    {
        var options = new DbContextOptionsBuilder<SocialAiContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var databaseContext = new SocialAiContext(options);
        await databaseContext.Database.EnsureCreatedAsync();
        
        var user1 = new User()
        {
            ID = 1,
            Name = "TestUser1",
            Email = "user1@test.com",
            Password = "Test123",
            Description = "Hi I'm a user1",
            Role = "User"
        };
        var user2 = new User()
        {
            ID = 2,
            Name = "TestUser2",
            Email = "user2@test.com",
            Password = "Test1233",
            Description = "Hi I'm a user2",
            Role = "User"
        };
        List<Post> posts = new List<Post>();

        for (int i = 1; i <= 5; i++)
        {
            var newPost = new Post()
            {
                ID = i,
                Prompt = $"prompt{i}",
                UserId = 1,
                User = user1,
                CreatedAt = DateTime.Now,
                Image = $"url{i}",
                Description = "text"
            };
            posts.Add(newPost);
            databaseContext.Add(newPost);
            
            await databaseContext.SaveChangesAsync();
        }
        for (int i = 1; i <= 5; i++)
        {
            databaseContext.Add(
                new Like()
                {
                    ID = i,
                    UserId = 2,
                    User = user2,
                    PostId = i,
                    Post = posts.FirstOrDefault(p => p.ID == i)
                }
            );
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    
    private async Task<ILikeService> SetupLikeService()
    {
        var dbContext = await SetupDbContext();

        ILikeService likeService = new LikeService(dbContext);

        return likeService;
    }
    
    [Fact]
    public async Task LikeService_ReturnsCorrectLikeById()
    {
        var likeService = await SetupLikeService();
        long likeId = 1;

        var result = await likeService.Get(likeId);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(likeId);
    }
    
    [Fact]
    public async Task LikeService_ReturnsNullForIncorrectId()
    {
        var likeService = await SetupLikeService();
        long likeId = 99;

        var result = await likeService.Get(likeId);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task LikeService_AddsLikeCorrectlyToDb()
    {
        var likeService = await SetupLikeService();
        var user = new User()
        {
            ID = 3,
            Name = "TestUser3",
            Email = "user3@test.com",
            Password = "Test123",
            Description = "Hi I'm a user3",
            Role = "User"
        };
        var post = new Post()
        {
            ID = 6,
            Prompt = $"prompt",
            Description = "Hi!",
            UserId = 3,
            Image = $"url",
            User = user
        };
        
        Like newLike = new Like()
        {
            ID = 6,
            UserId = 3,
            User = user,
            PostId = 6,
            Post = post
        };

        await likeService.Add(newLike);
        var result = await likeService.Get(newLike.ID);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(newLike.ID);
        result.Should().BeEquivalentTo(newLike);
    }
    
    [Fact]
    public async Task LikeService_GetAllUserLikes_ReturnsListOfLikes()
    {
        var likeService = await SetupLikeService();
        long userId = 2;
        
        var result = await likeService.GetUserLikes(userId);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Like>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task LikeService_GetAll_ReturnsListOfLikes()
    {
        var likeService = await SetupLikeService();
        
        var result = await likeService.GetAll();
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Like>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task LikeService_DeletesLikeWithCorrectId()
    {
        var likeService = await SetupLikeService();
        long likeId = 1;

        var result = await likeService.Delete(likeId);
        var deletedLike = await likeService.Get(likeId);
        
        result.Should().BeTrue();
        deletedLike.Should().BeNull();
    }
    
    [Fact]
    public async Task LikeService_Delete_ReturnFalseWithInCorrectId()
    {
        var likeService = await SetupLikeService();
        long likeId = 99;

        var result = await likeService.Delete(likeId);

        result.Should().BeFalse();
    }
}
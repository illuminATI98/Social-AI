using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Tests;

public class PostServiceTests
{
    private async Task<SocialAiContext> SetupDbContext()
    {
        var options = new DbContextOptionsBuilder<SocialAiContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var databaseContext = new SocialAiContext(options);
        await databaseContext.Database.EnsureCreatedAsync();
        var user = new User()
        {
            ID = 1,
            Name = "TestUser",
            Email = "user1@test.com",
            Password = "Test123",
            Description = "Hi I'm a user",
            Role = "User"
        };
        
        for (int i = 1; i <= 5; i++)
        {
            databaseContext.Add(
                new Post()
                {
                    ID = i,
                    Prompt = $"prompt{i}",
                    Description = "Hi!",
                    UserId = 1,
                    Image = $"url{i}",
                    User = user
                }
            );
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    
    private async Task<IPostService> SetupPostService()
    {
        var dbContext = await SetupDbContext();

        IPostService postService = new PostService(dbContext);

        return postService;
    }
    
    [Fact]
    public async Task PostService_ReturnsCorrectPostById()
    {
        var postService = await SetupPostService();
        long postId = 1;

        var result = await postService.Get(postId);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(postId);
    }
    
    [Fact]
    public async Task PostService_ReturnsNullForIncorrectId()
    {
        var postService = await SetupPostService();
        long postId = 99;

        var result = await postService.Get(postId);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task PostService_AddsPostCorrectlyToDb()
    {
        var postService = await SetupPostService();
        var user = new User()
        {
            ID = 2,
            Name = "TestUser",
            Email = "user2@test.com",
            Password = "Test123",
            Description = "Hi I'm a user",
            Role = "User"
        };
        
        Post newPost = new Post()
        {
            ID = 6,
            Image = "url",
            Prompt = "prompt",
            UserId = 2,
            Description = "I'm a post",
            User = user
        };

        await postService.Add(newPost);
        var result = await postService.Get(newPost.ID);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(newPost.ID);
        result.Should().BeEquivalentTo(newPost);
    }
    
    [Fact]
    public async Task PostService_ReturnsAllPosts()
    {
        var postService = await SetupPostService();

        var result = await postService.GetAll();
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Post>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task PostService_GetUserPosts_ReturnsListOfPostsByUserId()
    {
        var postService = await SetupPostService();
        long userId = 1;
        
        var result = await postService.GetUserPosts(userId);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Post>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task PostService_GetAll_ReturnsListOfPosts()
    {
        var postService = await SetupPostService();
        long userId = 1;
        
        var result = await postService.GetUserPosts(userId);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Post>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task PostService_UpdatesPostCorrectly()
    {
        var postService = await SetupPostService();
        string editedDescription = "Edited";
        long postId = 1;

        var result = await postService.Update(postId,editedDescription);

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task PostService_Update_ReturnFalseWithIncorrectId()
    {
        var postService = await SetupPostService();
        string editedDescription = "Edited";
        long postId = 99;

        var result = await postService.Update(postId,editedDescription);

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task PostService_DeletesPostWithCorrectId()
    {
        var postService = await SetupPostService();
        long postId = 1;

        var result = await postService.Delete(postId);
        var deletedPost = await postService.Get(postId);
        
        result.Should().BeTrue();
        deletedPost.Should().BeNull();
    }
    
    [Fact]
    public async Task PostService_Delete_ReturnFalseWithInCorrectId()
    {
        var postService = await SetupPostService();
        long postId = 99;

        var result = await postService.Delete(postId);

        result.Should().BeFalse();
    }
}
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Tests;

public class CommentServiceTests
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
        var post = new Post()
        {
            ID = 1,
            Prompt = $"prompt",
            Description = "Hi!",
            UserId = 1,
            Image = $"url",
            User = user
        };
        for (int i = 1; i <= 5; i++)
        {
            databaseContext.Add(
                new Comment()
                {
                    ID = i,
                    Text = $"comment",
                    UserId = 1,
                    User = user,
                    PostId = 1,
                    Post = post,
                    CreatedAt = DateTime.Now
                }
            );
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    
    private async Task<ICommentService> SetupCommentService()
    {
        var dbContext = await SetupDbContext();

        ICommentService commentService = new CommentService(dbContext);

        return commentService;
    }
    
    [Fact]
    public async Task CommentService_ReturnsCorrectCommentById()
    {
        var commentService = await SetupCommentService();
        long commentId = 1;

        var result = await commentService.Get(commentId);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(commentId);
    }
    
    [Fact]
    public async Task CommentService_ReturnsNullForIncorrectId()
    {
        var commentService = await SetupCommentService();
        long commentId = 99;

        var result = await commentService.Get(commentId);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CommentService_AddsCommentCorrectlyToDb()
    {
        var commentService = await SetupCommentService();
        var user = new User()
        {
            ID = 2,
            Name = "TestUser",
            Email = "user2@test.com",
            Password = "Test123",
            Description = "Hi I'm a user",
            Role = "User"
        };
        var post = new Post()
        {
            ID = 2,
            Prompt = $"prompt",
            Description = "Hi!",
            UserId = 2,
            Image = $"url",
            User = user
        };
        
        Comment newComment = new Comment()
        {
            ID = 6,
            UserId = 2,
            User = user,
            PostId = 2,
            Post = post,
            Text = "new comment",
            CreatedAt = DateTime.Now
        };

        await commentService.Add(newComment);
        var result = await commentService.Get(newComment.ID);
        
        result.Should().NotBeNull(); 
        result.ID.Should().Be(newComment.ID);
        result.Should().BeEquivalentTo(newComment);
    }
    
    [Fact]
    public async Task CommentService_GetAllPostComments_ReturnsListOfComments()
    {
        var commentService = await SetupCommentService();
        long postId = 1;
        
        var result = await commentService.GetAllPostComments(postId);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Comment>>();
        result.Count().Should().Be(5);
    }
    
    [Fact]
    public async Task CommentService_UpdatesCommentCorrectly()
    {
        var commentService = await SetupCommentService();
        string editedComment = "Edited";
        long commentId = 1;

        var result = await commentService.Update(commentId,editedComment);

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CommentService_Update_ReturnsFalseWithIncorrectId()
    {
        var commentService = await SetupCommentService();
        string editedComment = "Edited";
        long commentId = 99;

        var result = await commentService.Update(commentId,editedComment);

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task CommentService_DeletesCommentWithCorrectId()
    {
        var commentService = await SetupCommentService();
        long commentId = 1;

        var result = await commentService.Delete(commentId);
        var deletedComment = await commentService.Get(commentId);
        
        result.Should().BeTrue();
        deletedComment.Should().BeNull();
    }
    
    [Fact]
    public async Task CommentService_Delete_ReturnFalseWithInCorrectId()
    {
        var commentService = await SetupCommentService();
        long commentId = 99;

        var result = await commentService.Delete(commentId);

        result.Should().BeFalse();
    }
}
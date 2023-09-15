using System.Security.Claims;
using Social_AI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;

[Authorize]
[ApiController, Route("/api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    private readonly IUserService _userService;

    public PostController(PostService postService, UserService userService)
    { 
        _postService = postService;
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
    {
        long userId = GetUserIdFromToken();
        
        User user = await _userService.GetUserById(userId);
        
        if (user == null) return NotFound("User not found");
        
        if (postDto == null) return BadRequest();
        
        var newPost = new Post
        {
            UserId = userId,
            User = user,
            Prompt = postDto.Prompt,
            Description = postDto.Description,
            Image = postDto.Image,
            CreatedAt = DateTime.UtcNow
        };
        await _postService.Add(newPost);
        
        return Ok();
    }
    
    [HttpGet("/api/posts/{postId}")]
    public async Task<IActionResult> Get(long postId)
    {
        var post = await _postService.Get(postId);
        return Ok(post);
    }
    
    [HttpGet("/api/posts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postService.GetAll();
        return Ok(posts);
    }
    
    [HttpPut("/api/posts/update/{postId}")]
    public async Task<IActionResult> UpdatePost(long postId,[FromBody] string description)
    {
        long userId = GetUserIdFromToken();
        Post post = await _postService.Get(postId);
        if (post.UserId != userId)
        {
            return Forbid();
        }
        return Ok(_postService.Update(postId,description));
    }
    
    [HttpDelete("/api/posts/delete/{postId}")]
    public async Task<IActionResult> DeletePost(long postId)
    {
        long userId = GetUserIdFromToken();
        Post post = await _postService.Get(postId);
        if (post.UserId != userId)
        {
            return Forbid();
        }
        await _postService.Delete(postId);
        return Ok();
    }
    
    [NonAction] 
    private long GetUserIdFromToken()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
            return long.Parse(userId);
        return 0;
    }
}

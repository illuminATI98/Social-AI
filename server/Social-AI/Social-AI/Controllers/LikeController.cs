using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;

[Authorize]
[ApiController, Route("/api/likes")]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    private readonly IUserService _userService;

    private readonly IPostService _postService;

    public LikeController(ILikeService likeService, IUserService userService, IPostService postService)
    {
        _likeService = likeService;
        _userService = userService;
        _postService = postService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Like([FromBody] long postId)
    {
        long userId = GetUserIdFromToken();
        
        User user = await _userService.GetUserById(userId);

        Post post = await _postService.Get(postId);
        
        if (user == null) return NotFound("User not found");
        if (post == null) return NotFound("Post not found");

        var newLike = new Like()
        {
            UserId = userId,
            User = user,
            PostId = postId,
            Post = post,
        };
        await _likeService.Add(newLike);
        
        return Ok();
    }
    
    [HttpGet("/api/likes/user/{userId}")]
    public async Task<IActionResult> GetAllUserLikes(long userId)
    {
        var likes = await _likeService.GetUserLikes(userId);
        return Ok(likes);
    }
    
    [HttpDelete("/api/likes/delete/{likeId}")]
    public async Task<IActionResult> DisLike(long likeId)
    {
        long userId = GetUserIdFromToken();
        Like like = await _likeService.Get(likeId);
        if (like.UserId != userId)
        {
            return Forbid();
        }
        await _likeService.Delete(likeId);
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
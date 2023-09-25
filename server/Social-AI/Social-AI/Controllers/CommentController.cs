using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;
[Authorize]
[ApiController, Route("/api/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    private readonly IUserService _userService;

    public CommentController(CommentService commentService, UserService userService)
    {
        _commentService = commentService;
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO commentDto)
    {
        long userId = GetUserIdFromToken();
        
        User user = await _userService.GetUserById(userId);
        
        if (user == null) return NotFound("User not found");
        
        if (commentDto == null) return BadRequest();

        var newComment = new Comment()
        {
            Text = commentDto.Text,
            UserId = userId,
            User = user,
            PostId = commentDto.postId,
            CreatedAt = DateTime.Now
        };
        await _commentService.Add(newComment);
        
        return Ok();
    }
    
    [HttpGet("/api/comments/{commentId}")]
    public async Task<IActionResult> Get(long commentId)
    {
        var comment = await _commentService.Get(commentId);
        return Ok(comment);
    }
    
    [HttpGet("/api/comments/post/{postId}")]
    public async Task<IActionResult> GetAllPostComments(long postId)
    {
        var comments = await _commentService.GetAllPostComments(postId);
        return Ok(comments);
    }
    
    [HttpPut("/api/comments/update/{commentId}")]
    public async Task<IActionResult> UpdatePost(long commentId,[FromBody] string text)
    {
        long userId = GetUserIdFromToken();
        Comment comment = await _commentService.Get(commentId);
        if (comment.UserId != userId)
        {
            return Forbid();
        }
        return Ok(_commentService.Update(commentId,text));
    }
    
    [HttpDelete("/api/comments/delete/{commentId}")]
    public async Task<IActionResult> DeletePost(long commentId)
    {
        long userId = GetUserIdFromToken();
        Comment comment = await _commentService.Get(commentId);
        if (comment.UserId != userId)
        {
            return Forbid();
        }
        await _commentService.Delete(commentId);
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
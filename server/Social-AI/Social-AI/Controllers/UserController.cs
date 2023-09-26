using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;

[Authorize]
[ApiController, Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(UserService userService)
    {
        _service = userService;
    }
    
    [HttpGet("/users/{id}")]
    public async Task<IActionResult> Get(long id)
    {
        return Ok(await _service.GetUserById(id));
    }
    
    [HttpGet("/users")]
    public async Task<IEnumerable<User>> GetAllUser()
    {
        return await _service.GetAll();
    }
    
    [HttpPut("/users/update/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] EditUserDTO userDto)
    {
        var userId = GetUserIdFromToken();
        if (_service.UserExistsById(userId))
        {
            await _service.Update(userId,userDto);
            return Ok();
        }

        return BadRequest("User not found");
    }
    
    [HttpDelete("/users/delete/{id}")]
    public async Task DeleteUser(long id)
    {
        await _service.Delete(id);
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
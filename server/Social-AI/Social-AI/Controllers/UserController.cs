using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;

[Authorize]
[ApiController, Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService userService)
    {
        _service = userService;
    }
    
    [HttpGet("/{id}")]
    public Task<User> Get(long id)
    {
        return _service.Get(id);
    }
    
    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUser()
    {
        return await _service.GetAll();
    }
    
    [HttpPut("/update/{id}")]
    public async Task UpdateUser(long id, [FromBody] User user)
    {
        if (_service.UserExistsById(id))
        {
            await _service.Update(user);
        }
    }
    
    [HttpDelete("/delete/{id}")]
    public async Task DeleteUser(long id)
    {
        await _service.Delete(id);
    }
}
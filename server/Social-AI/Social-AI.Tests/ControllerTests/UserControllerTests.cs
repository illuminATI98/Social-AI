using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;

[assembly: InternalsVisibleTo("UserControllerTests")]

namespace Social_AI.Tests.ControllerTests;
public class UserControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    private readonly string _mockToken;

    public UserControllerTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>{});
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "Test User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("oSdk65HL5kok23bmMxEt932KSsaf256KigIElgoKDglzODbkG86543kG"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );
        
        _mockToken = new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task UserController_ShouldGiveStatusUnauthorized_WhenUserIsNotAuthorized()
    {
        var client = _factory.CreateClient();

        var result = await client.GetAsync("users");
        
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task UserController_GetAllUsers_ReturnsListOfUsers()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_mockToken}");
        
        var response = await client.GetAsync("users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
        
        users.Should().NotBeNull();
        users.Should().BeAssignableTo<IEnumerable<User>>();
    }
    
    [Fact]
    public async Task UserController_GetUser_ReturnsNoContentWithIncorrectId()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_mockToken}");
        
        var result = await client.GetAsync($"users/{0}");

        Assert.Equal(HttpStatusCode.NoContent,result.StatusCode);
    }
    
    [Fact]
    public async Task UserController_Update_UpdatesUser()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_mockToken}");

        EditUserDTO userDto = new EditUserDTO()
        {
            Name = "Edited",
            Description = "Edited",
            Password = "Edited"
        };
        var jsonPayload = JsonConvert.SerializeObject(userDto);

        // Create a StringContent with JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var result = await client.PutAsync("users/update", content);

        Assert.Equal(HttpStatusCode.OK,result.StatusCode);
    }
    [Fact]
    public async Task UserController_Update_ReturnsBadRequestWhenWrongBodyPassed()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_mockToken}");

        string wrongContent = "wrong";
        
        var jsonPayload = JsonConvert.SerializeObject(wrongContent);

        // Create a StringContent with JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var result = await client.PutAsync("users/update", content);

        Assert.Equal(HttpStatusCode.BadRequest,result.StatusCode);
    }
}
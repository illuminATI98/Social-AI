using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;
using Social_AI.Services;

namespace Social_AI.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
        private readonly IConfiguration _configuration;
        private readonly IUserService _service;
        private readonly PasswordHasher<User> _hasher;

        public AuthController(IConfiguration configuration, IUserService service, PasswordHasher<User> hasher)
        {
            _configuration = configuration;
            _service = service;
            _hasher = hasher;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO dto)
        {
            bool userExists = _service.UserExistsByEmail(dto.Email);
            
            if (userExists)
            {
                return BadRequest(new RegisterResultDTO(){ Error = "Email is already taken." });
            }
            
            User user = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Role = "User"
            };
            var hashedPassword = _hasher.HashPassword(user, dto.Password);
            user.Password = hashedPassword;
            await _service.Add(user);
            
            return Ok(new RegisterResultDTO(){ Message = "Registration successful." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDTO>> Login(LoginDTO dto)
        {
            var user = await _service.GetByLogin(dto.Email, dto.Password);

            if (user == null)
            {
                return BadRequest( new LoginResultDTO{Error = "Invalid credentials"});
            }
            
            string token = GenerateJwtToken(user);
            
            return Ok(new LoginResultDTO{Token = token});
        } 
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(
                _configuration["JwtSecretKey"]); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.DTOs;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class UserService : IUserService
{
    private ISocialAiContext _context { get; set; }
    private PasswordHasher<User> _hasher { get; set; }

    public UserService(ISocialAiContext context, PasswordHasher<User> hasher)
    {
        _context = context;
        _hasher = hasher;
    }
    
    
    public async Task Add(User entity)
    {
       await _context.Users.AddAsync(entity);
       await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetUserById(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> Update(long userId, EditUserDTO userDto)
    {
        User user = await GetUserById(userId);
        if (user != null)
        {
            var hashedPassword = _hasher.HashPassword(user, userDto.Password);
            user.Password = hashedPassword;
            user.Name = userDto.Name;
            user.Picture = userDto.Picture;
            user.Description = userDto.Description;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> Delete(long id)
    {
        var user = await GetUserById(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
    public async Task<User?> GetByLogin(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user != null)
        {
            var result = _hasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
        }
        return null;
    }

    public bool UserExistsByEmail(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public bool UserExistsById(long id)
    {
        return _context.Users.Any(u => u.ID == id);
    }
}
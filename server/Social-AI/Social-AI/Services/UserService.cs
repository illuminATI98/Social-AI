using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class UserService : IUserService
{
    private SocialAiContext _context { get; set; }
    private PasswordHasher<User> _hasher { get; set; }

    public UserService(SocialAiContext context, PasswordHasher<User> hasher)
    {
        _context = context;
        _hasher = hasher;
    }
    
    
    public async Task Add(User entity)
    {
       await _context.Users.AddAsync(entity);
       await _context.SaveChangesAsync();
    }
    
    public async Task<User?> Get(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task Update(User entity)
    {
        var user = await _context.Users.FindAsync(entity.ID);
        if (user != null)
        {
            user.Name = entity.Name;
            user.Password = entity.Password;
            user.Email = entity.Email;
            user.Picture = entity.Picture;
            user.Description = entity.Description;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(long id)
    { 
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
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
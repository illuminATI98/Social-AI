using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class UserService : IService<User>
{
    private SocialAIContext _Context { get; set; }
    private PasswordHasher<User> _PasswordHasher { get; set; }

    public UserService(SocialAIContext context, PasswordHasher<User> passwordHasher)
    {
        _Context = context;
        _PasswordHasher = passwordHasher;
    }
    
    
    public async Task Add(User entity)
    {
       await _Context.Users.AddAsync(entity);
       await _Context.SaveChangesAsync();
    }

    public async Task<User?> Get(long id)
    {
        return await _Context.Users.FirstOrDefaultAsync(u => u.ID == id);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _Context.Users.ToListAsync();
    }

    public async Task Update(User entity)
    {
        var user = await _Context.Users.FindAsync(entity.ID);
        if (user != null)
        {
            user.Name = entity.Name;
            user.Password = entity.Password;
            user.Email = entity.Email;
            user.Role = entity.Role;
            await _Context.SaveChangesAsync();
        }
    }

    public async Task Delete(long id)
    {
        var user = await _Context.Users.FindAsync(id);
        if (user != null)
        {
            _Context.Users.Remove(user);
            await _Context.SaveChangesAsync();
        }
    }
    
    public async Task<User?> GetByLogin(string email, string password)
    {
        var user = await _Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user != null)
        {
            var result = _PasswordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
        }
        return null;
    }
}
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public interface IUserService 
{
    public Task Add(User entity);
    
    public Task<User> GetUserById(long id);
    
    public Task<IEnumerable<User>> GetAll();
    
    public Task Update(User entity);
    
    public Task Delete(long id);
    bool UserExistsByEmail(string email);
    Task<User> GetByLogin(string email, string password);
    bool UserExistsById(long id);
}
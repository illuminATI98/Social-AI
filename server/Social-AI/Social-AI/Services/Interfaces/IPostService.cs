using Microsoft.AspNetCore.Http.HttpResults;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public interface IPostService 
{
    public Task Add(Post entity, long userId);
    
    public Task<Post> Get(long id);
    
    public Task<IEnumerable<Post>> GetAll();

    public Task<IEnumerable<Post>> GetUserPosts(long userId);
    
    public Task Update(Post entity);
    
    public Task Delete(long id);
}
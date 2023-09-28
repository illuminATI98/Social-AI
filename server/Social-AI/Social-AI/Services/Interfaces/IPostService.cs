using Microsoft.AspNetCore.Http.HttpResults;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public interface IPostService 
{
    public Task Add(Post entity);
    
    public Task<Post> Get(long postId);
    
    public Task<IEnumerable<Post>> GetAll();

    public Task<IEnumerable<Post>> GetUserPosts(long userId);
    
    public Task<bool> Update(long postId,string description);
    
    public Task<bool> Delete(long postId);
}
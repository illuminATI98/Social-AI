using Social_AI.Models.Entities;

namespace Social_AI.Services;

public interface ILikeService
{
    public Task Add(Like entity);
    
    public Task<Like> Get(long likeId);
    
    public Task<IEnumerable<Like>> GetAll();

    public Task<IEnumerable<Like>> GetUserLikes(long userId);

    public Task<bool> Delete(long likeId);
}
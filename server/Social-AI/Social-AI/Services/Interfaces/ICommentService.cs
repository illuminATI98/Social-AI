using Social_AI.Models.Entities;

namespace Social_AI.Services;

public interface ICommentService
{
    public Task Add(Comment comment);
    
    public Task<Comment> Get(long commentId);
    
    public Task<IEnumerable<Comment>> GetAllPostComments(long postId);

    public Task<bool> Update(long commentId,string text);
    
    public Task<bool> Delete(long commentId);
}
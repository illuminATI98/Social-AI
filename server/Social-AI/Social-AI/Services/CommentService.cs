using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class CommentService : ICommentService
{
    private SocialAiContext _context { get; set; }

    public CommentService(SocialAiContext context)
    {
        _context = context;
    }
    
    public async Task Add(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> Get(long commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<IEnumerable<Comment>> GetAllPostComments(long postId)
    {
        return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
    }

    public async Task Update(long commentId, string text)
    {
        var comment = await Get(commentId);
        if (comment != null)
        {
            comment.Text = text;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(long commentId)
    {
        var comment = await Get(commentId);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
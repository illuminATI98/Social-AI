using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class PostService : IPostService
{
    private ISocialAiContext _context { get; set; }

    public PostService(ISocialAiContext context)
    {
        _context = context;
    }
    
    public async Task Add(Post entity)
    {
        _context.Posts.Add(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Post> Get(long postId)
    {
        return await _context.Posts.FindAsync(postId);
    }

    public async Task<IEnumerable<Post>> GetAll()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetUserPosts(long userId)
    {
        return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<bool> Update(long postId,string description)
    {
        var post = await Get(postId);
        if (post != null)
        {
            post.Description = description;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> Delete(long postId)
    {
        var post = await Get(postId);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
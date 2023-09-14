using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class PostService : IPostService
{
    private SocialAiContext _context { get; set; }

    public PostService(SocialAiContext context)
    {
        _context = context;
    }
    
    public async Task Add(Post entity)
    {
        _context.Posts.Add(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Post> Get(long id)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.ID == id);
    }

    public async Task<IEnumerable<Post>> GetAll()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetUserPosts(long userId)
    {
        return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task Update(long postId,string description)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post != null)
        {
            post.Description = description;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(long id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
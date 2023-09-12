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
    
    public async Task Add(Post entity, long userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.ID == userId);

        if (user == null)
        {
            return;
        }

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

    public async Task Update(Post entity)
    {
        var post = await _context.Posts.FindAsync(entity.ID);
        if (post != null)
        {
            post.Description = entity.Description;
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
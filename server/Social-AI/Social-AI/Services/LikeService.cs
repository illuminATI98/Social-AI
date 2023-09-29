using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI.Services;

public class LikeService : ILikeService
{
    private ISocialAiContext _context { get; set; }

    public LikeService(ISocialAiContext context)
    {
        _context = context;
    }
    
    public async Task Add(Like entity)
    {
        _context.Likes.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Like> Get(long likeId)
    {
        return await _context.Likes.FindAsync(likeId);
    }

    public async Task<IEnumerable<Like>> GetAll()
    {
        return await _context.Likes.ToListAsync();
    }

    public async Task<IEnumerable<Like>> GetUserLikes(long userId)
    {
        return await _context.Likes.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<bool> Delete(long likeId)
    {
        var like = await Get(likeId);
        if (like != null)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
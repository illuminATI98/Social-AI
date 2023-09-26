using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI;

public class SocialAiContext : DbContext , ISocialAiContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Like> Likes { get; set; }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public SocialAiContext(DbContextOptions<SocialAiContext> options) : base(options)
    {
        
    }
}
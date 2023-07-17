using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI;

public class SocialAIContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public SocialAIContext(DbContextOptions<SocialAIContext> options) : base(options)
    {
        
    }
}
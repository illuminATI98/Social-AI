using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI;

public class SocialAiContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public SocialAiContext(DbContextOptions<SocialAiContext> options) : base(options)
    {
        
    }
}
using Microsoft.EntityFrameworkCore;
using Social_AI.Models.Entities;

namespace Social_AI;

public interface ISocialAiContext
{
    DbSet<User> Users { get; set; }

    DbSet<Post> Posts { get; set; }

    DbSet<Comment> Comments { get; set; }

    DbSet<Like> Likes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
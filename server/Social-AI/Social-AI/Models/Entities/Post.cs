using System.ComponentModel.DataAnnotations.Schema;

namespace Social_AI.Models.Entities;

public class Post
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    
    public long UserId { get; set; }

    public User User { get; set; }

    public string Prompt { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
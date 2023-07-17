using System.ComponentModel.DataAnnotations.Schema;

namespace Social_AI.Models.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }
    
    public string Email { get; set; }

    public byte[]? Picture { get; set; }
    public string Role { get; set; }

    public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();

    public virtual ICollection<Post> LikedPosts { get; set; } = new List<Post>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();


}
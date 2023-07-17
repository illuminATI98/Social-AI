using System.ComponentModel.DataAnnotations.Schema;

namespace Social_AI.Models.Entities;

public class Post
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Prompt { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }

    public int Likes { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Social_AI.Models.Entities;

public class Comment
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Text { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public long UserId { get; set; }

    public User User { get; set; }
    
    public long PostId { get; set; }

    public Post Post { get; set; }
}
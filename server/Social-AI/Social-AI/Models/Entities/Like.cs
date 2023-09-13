namespace Social_AI.Models.Entities;

public class Like
{
    public long Id { get; set; }
    
    public long UserId { get; set; }

    public User User { get; set; }
    
    public long PostId { get; set; }

    public Post Post { get; set; }
}
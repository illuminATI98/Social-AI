namespace Social_AI.Models.DTOs;

public class CreateCommentDTO
{
    public string Text { get; set; }
    
    public long postId { get; set; }
}
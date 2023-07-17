using System.ComponentModel.DataAnnotations.Schema;

namespace Social_AI.Models.Entities;

public class Comment
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Text { get; set; }
    
}
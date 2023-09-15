namespace Social_AI.Models.DTOs;

public class EditUserDTO
{
    public string Name { get; set; }

    public string Password { get; set; }

    public byte[]? Picture { get; set; }

    public string Description { get; set; }
}
namespace ShadowOfGoldBot.Models;

public class Ending
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Condition { get; set; }
}
using Telegram.Bot.Types;

namespace ShadowOfGoldBot.Models;

public class Scene
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? VoiceUrl { get; set; }
    public bool IsEnding { get; set; } = false;

    public List<Choice> Choices { get; set; } = new();
}
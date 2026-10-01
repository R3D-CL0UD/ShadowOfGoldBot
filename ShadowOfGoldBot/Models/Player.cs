using System.Text;

namespace ShadowOfGoldBot.Models;

public class Player
{
    public int Id { get; set; }
    public long TelegramUserId { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }

    public int CurrentSceneId { get; set; }
    public Scene? CurrentScene { get; set; }

    public bool IsGameFinished { get; set; } = false;
    public int? EndingId { get; set; }
    public Ending? Ending { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastPlayedAt { get; set; } = DateTime.UtcNow;

    public List<PlayerFlag> Flags { get; set; } = new();
    public PlayerState? State { get; set; }
}
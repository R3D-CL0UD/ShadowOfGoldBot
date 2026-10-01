namespace ShadowOfGoldBot.Models;

public class PlayerFlag
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }

    public string FlagName { get; set; } = string.Empty;
    public string FlagValue { get; set; } = string.Empty;
    public DateTime SetAt { get; set; } = DateTime.UtcNow;
}
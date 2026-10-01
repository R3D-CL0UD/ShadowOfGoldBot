namespace ShadowOfGoldBot.Models;

public class Choice
{
    public int Id { get; set; }
    public int SceneId { get; set; }
    public Scene? Scene { get; set; }

    public string Text { get; set; } = string.Empty;
    public int NextSceneId { get; set; }

    public int ReputationTownChange { get; set; } = 0;
    public int ReputationSheriffChange { get; set; } = 0;
    public int ReputationGangChange { get; set; } = 0;
    public int TrustMayChange { get; set; } = 0;
    public int KarmaGreedChange { get; set; } = 0;

    public string? FlagToSet { get; set; }
    public string? RequiredFlag { get; set; }
    public string? RequiredItem { get; set; }
}
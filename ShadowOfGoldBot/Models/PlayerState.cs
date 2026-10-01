namespace ShadowOfGoldBot.Models;

public class PlayerState
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }

    public int ReputationTown { get; set; } = 0;
    public int ReputationSheriff { get; set; } = 0;
    public int ReputationGang { get; set; } = 0;
    public int TrustMay { get; set; } = 0;
    public int KarmaGreed { get; set; } = 0;

    public bool HasGold { get; set; } = false;
    public bool HasRifle { get; set; } = false;
    public bool HasAmmo { get; set; } = false;
}
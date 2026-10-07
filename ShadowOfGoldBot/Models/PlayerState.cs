namespace ShadowOfGoldBot.Models;

public class PlayerState
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }

    // ===== بقا =====
    public int Health { get; set; } = 100;
    public int Morale { get; set; } = 100;

    // ===== آمار اصلی =====
    public int ReputationTown { get; set; } = 0;
    public int ReputationSheriff { get; set; } = 0;
    public int ReputationGang { get; set; } = 0;
    public int ReputationOutlaws { get; set; } = 0;

    public int TrustMartha { get; set; } = 0;
    public int TrustSheriff { get; set; } = 0;
    public int TrustHartley { get; set; } = 0;
    public int TrustTom { get; set; } = 0;
    public int TrustSara { get; set; } = 0;
    public int TrustCole { get; set; } = 0;

    public int KarmaGreed { get; set; } = 0;
    public int KarmaHonor { get; set; } = 0;
    public int KarmaMercy { get; set; } = 0;

    // ===== مهارت‌ها (0 تا 10) =====
    public int SkillPerception { get; set; } = 2;
    public int SkillPersuasion { get; set; } = 2;
    public int SkillIntimidation { get; set; } = 2;
    public int SkillEmpathy { get; set; } = 2;
    public int SkillCombat { get; set; } = 2;

    // ===== آیتم‌ها =====
    public bool HasGold { get; set; } = false;
    public bool HasRifle { get; set; } = false;
    public bool HasAmmo { get; set; } = false;
}
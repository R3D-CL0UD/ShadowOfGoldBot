namespace ShadowOfGoldBot.Models;

public class Choice
{
    public int Id { get; set; }
    public int SceneId { get; set; }
    public Scene? Scene { get; set; }

    public string Text { get; set; } = string.Empty;
    public int NextSceneId { get; set; }

    // ===== اعتبار =====
    public int ReputationTownChange { get; set; } = 0;
    public int ReputationSheriffChange { get; set; } = 0;
    public int ReputationGangChange { get; set; } = 0;
    public int ReputationOutlawsChange { get; set; } = 0;

    // ===== اعتماد =====
    public int TrustMarthaChange { get; set; } = 0;
    public int TrustSheriffChange { get; set; } = 0;
    public int TrustHartleyChange { get; set; } = 0;
    public int TrustTomChange { get; set; } = 0;
    public int TrustMayChange { get; set; } = 0;
    public int TrustColeChange { get; set; } = 0;

    // ===== کارما =====
    public int KarmaGreedChange { get; set; } = 0;
    public int KarmaHonorChange { get; set; } = 0;
    public int KarmaMercyChange { get; set; } = 0;

    // ===== مهارت‌ها =====
    public int SkillPerceptionChange { get; set; } = 0;
    public int SkillPersuasionChange { get; set; } = 0;
    public int SkillIntimidationChange { get; set; } = 0;
    public int SkillEmpathyChange { get; set; } = 0;
    public int SkillCombatChange { get; set; } = 0;

    // ===== بقا =====
    public int HealthChange { get; set; } = 0;
    public int MoraleChange { get; set; } = 0;

    // ===== فلگ و شرط =====
    public string? FlagToSet { get; set; }
    public string? RequiredFlag { get; set; }
    public string? RequiredItem { get; set; }

    // ===== شرط مهارت =====
    public string? RequiredSkill { get; set; }  // "Perception", "Persuasion", "Intimidation", "Empathy", "Combat"
    public int RequiredSkillLevel { get; set; } = 0;
}
using ShadowOfGoldBot.Models;
using ShadowOfGoldBot.Repositories;

namespace ShadowOfGoldBot.Services;

public class GameService : IGameService
{
    private readonly IPlayerRepository _playerRepo;
    private readonly ISceneRepository _sceneRepo;

    public GameService(IPlayerRepository playerRepo, ISceneRepository sceneRepo)
    {
        _playerRepo = playerRepo;
        _sceneRepo = sceneRepo;
    }

    public async Task<Player> GetOrCreatePlayerAsync(long telegramUserId, string firstName, string? username)
    {
        var player = await _playerRepo.GetWithDetailsAsync(telegramUserId);

        if (player is null)
        {
            player = new Player
            {
                TelegramUserId = telegramUserId,
                FirstName = firstName,
                Username = username,
                CurrentSceneId = 1,
                State = new PlayerState()
            };
            await _playerRepo.CreateAsync(player);
        }

        return player;
    }

    public async Task<Scene?> GetCurrentSceneAsync(long telegramUserId)
    {
        var player = await _playerRepo.GetWithDetailsAsync(telegramUserId);
        if (player is null) return null;

        return await _sceneRepo.GetWithChoicesAsync(player.CurrentSceneId);
    }

    public async Task<List<Choice>> GetCurrentChoicesAsync(long telegramUserId)
    {
        var player = await _playerRepo.GetWithDetailsAsync(telegramUserId);
        if (player is null) return new();

        return await _sceneRepo.GetAvailableChoicesAsync(player.CurrentSceneId, player);
    }

    public async Task<Scene?> MakeChoiceAsync(long telegramUserId, int choiceIndex)
    {
        var player = await _playerRepo.GetWithDetailsAsync(telegramUserId);
        if (player is null) return null;

        var choices = await _sceneRepo.GetAvailableChoicesAsync(player.CurrentSceneId, player);
        if (choiceIndex < 0 || choiceIndex >= choices.Count) return null;

        var chosen = choices[choiceIndex];

        if (player.State is not null)
        {
            // بقا
            player.State.Morale = Math.Clamp(player.State.Morale, 0, 100);
            player.State.Health = Math.Clamp(player.State.Health, 0, 100);

            // اعتبار
            player.State.ReputationTown += chosen.ReputationTownChange;
            player.State.ReputationSheriff += chosen.ReputationSheriffChange;
            player.State.ReputationGang += chosen.ReputationGangChange;

            // کارما
            player.State.KarmaGreed += chosen.KarmaGreedChange;
            player.State.KarmaHonor += chosen.KarmaHonorChange;
            player.State.KarmaMercy += chosen.KarmaMercyChange;

            // اعتماد
            player.State.TrustMartha += chosen.TrustMarthaChange;
            player.State.TrustSheriff += chosen.TrustSheriffChange;
            player.State.TrustHartley += chosen.TrustHartleyChange;
            player.State.TrustTom += chosen.TrustTomChange;
            player.State.TrustSara += chosen.TrustMayChange;
            player.State.TrustCole += chosen.TrustColeChange;

            // مهارت‌ها (بین ۰ تا ۱۰)
            player.State.SkillPerception = Math.Clamp(player.State.SkillPerception + chosen.SkillPerceptionChange, 0, 10);
            player.State.SkillPersuasion = Math.Clamp(player.State.SkillPersuasion + chosen.SkillPersuasionChange, 0, 10);
            player.State.SkillIntimidation = Math.Clamp(player.State.SkillIntimidation + chosen.SkillIntimidationChange, 0, 10);
            player.State.SkillEmpathy = Math.Clamp(player.State.SkillEmpathy + chosen.SkillEmpathyChange, 0, 10);
            player.State.SkillCombat = Math.Clamp(player.State.SkillCombat + chosen.SkillCombatChange, 0, 10);
        }

        if (!string.IsNullOrEmpty(chosen.FlagToSet))
        {
            var exists = player.Flags.Any(f => f.FlagName == chosen.FlagToSet);
            if (!exists)
            {
                player.Flags.Add(new PlayerFlag
                {
                    FlagName = chosen.FlagToSet,
                    FlagValue = "true"
                });
            }
        }

        player.CurrentSceneId = chosen.NextSceneId;
        player.LastPlayedAt = DateTime.UtcNow;

        await _playerRepo.UpdateAsync(player);

        return await _sceneRepo.GetWithChoicesAsync(player.CurrentSceneId);
    }

    public async Task<Player> RestartAsync(long telegramUserId)
    {
        var player = await _playerRepo.GetWithDetailsAsync(telegramUserId);
        if (player is null)
            return await GetOrCreatePlayerAsync(telegramUserId, "Player", null);

        player.CurrentSceneId = 1;
        player.IsGameFinished = false;
        player.EndingId = null;
        player.Flags.Clear();

        if (player.State is not null)
        {
            player.State.Health = 100;
            player.State.Morale = 100;

            player.State.ReputationTown = 0;
            player.State.ReputationSheriff = 0;
            player.State.ReputationGang = 0;
            player.State.ReputationOutlaws = 0;

            player.State.TrustMartha = 0;
            player.State.TrustSheriff = 0;
            player.State.TrustHartley = 0;
            player.State.TrustTom = 0;
            player.State.TrustSara = 0;
            player.State.TrustCole = 0;

            player.State.KarmaGreed = 0;
            player.State.KarmaHonor = 0;
            player.State.KarmaMercy = 0;

            player.State.SkillPerception = 2;
            player.State.SkillPersuasion = 2;
            player.State.SkillIntimidation = 2;
            player.State.SkillEmpathy = 2;
            player.State.SkillCombat = 2;

            player.State.HasGold = false;
            player.State.HasRifle = false;
            player.State.HasAmmo = false;
        }

        await _playerRepo.UpdateAsync(player);
        return player;
    }
}
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
            player.State.ReputationTown += chosen.ReputationTownChange;
            player.State.ReputationSheriff += chosen.ReputationSheriffChange;
            player.State.ReputationGang += chosen.ReputationGangChange;
            player.State.TrustMay += chosen.TrustMayChange;
            player.State.KarmaGreed += chosen.KarmaGreedChange;
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
            player.State.ReputationTown = 0;
            player.State.ReputationSheriff = 0;
            player.State.ReputationGang = 0;
            player.State.TrustMay = 0;
            player.State.KarmaGreed = 0;
        }

        await _playerRepo.UpdateAsync(player);
        return player;
    }
}
using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Services;

public interface IGameService
{
    Task<Player> GetOrCreatePlayerAsync(long telegramUserId, string firstName, string? username);
    Task<Scene?> GetCurrentSceneAsync(long telegramUserId);
    Task<List<Choice>> GetCurrentChoicesAsync(long telegramUserId);
    Task<Scene?> MakeChoiceAsync(long telegramUserId, int choiceIndex);
    Task<Player> RestartAsync(long telegramUserId);
}
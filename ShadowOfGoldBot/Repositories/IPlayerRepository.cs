using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByTelegramIdAsync(long telegramUserId);
    Task<Player> CreateAsync(Player player);
    Task UpdateAsync(Player player);
    Task<Player?> GetWithDetailsAsync(long telegramUserId);
}
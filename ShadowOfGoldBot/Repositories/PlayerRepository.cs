using Microsoft.EntityFrameworkCore;
using ShadowOfGoldBot.Data;
using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _db;

    public PlayerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Player?> GetByTelegramIdAsync(long telegramUserId)
    {
        return await _db.Players
            .FirstOrDefaultAsync(p => p.TelegramUserId == telegramUserId);
    }

    public async Task<Player> CreateAsync(Player player)
    {
        _db.Players.Add(player);
        await _db.SaveChangesAsync();
        return player;
    }

    public async Task UpdateAsync(Player player)
    {
        _db.Players.Update(player);
        await _db.SaveChangesAsync();
    }

    public async Task<Player?> GetWithDetailsAsync(long telegramUserId)
    {
        return await _db.Players
            .Include(p => p.State)
            .Include(p => p.Flags)
            .FirstOrDefaultAsync(p => p.TelegramUserId == telegramUserId);
    }
}
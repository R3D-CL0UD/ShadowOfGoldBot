using Microsoft.EntityFrameworkCore;
using ShadowOfGoldBot.Data;
using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Repositories;

public class SceneRepository : ISceneRepository
{
    private readonly AppDbContext _db;

    public SceneRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Scene?> GetByIdAsync(int id)
    {
        return await _db.Scenes
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Scene?> GetWithChoicesAsync(int id)
    {
        return await _db.Scenes
            .Include(s => s.Choices)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Choice>> GetAvailableChoicesAsync(int sceneId, Player player)
    {
        var choices = await _db.Choices
            .Where(c => c.SceneId == sceneId)
            .ToListAsync();

        var playerFlags = player.Flags.Select(f => f.FlagName).ToHashSet();

        return choices.Where(c =>
        {
            if (string.IsNullOrEmpty(c.RequiredFlag))
                return true;

            return playerFlags.Contains(c.RequiredFlag);
        }).ToList();
    }

    public async Task<List<Scene>> GetAllAsync()
    {
        return await _db.Scenes.ToListAsync();
    }

    public async Task AddAsync(Scene scene)
    {
        _db.Scenes.Add(scene);
        await _db.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
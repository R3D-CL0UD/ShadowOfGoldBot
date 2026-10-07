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
        var state = player.State;

        return choices.Where(c =>
        {
            // شرط فلگ
            if (!string.IsNullOrEmpty(c.RequiredFlag) && !playerFlags.Contains(c.RequiredFlag))
                return false;

            // شرط آیتم
            if (!string.IsNullOrEmpty(c.RequiredItem) && state is not null)
            {
                var hasItem = c.RequiredItem switch
                {
                    "Gold" => state.HasGold,
                    "Rifle" => state.HasRifle,
                    "Ammo" => state.HasAmmo,
                    _ => false
                };
                if (!hasItem) return false;
            }

            // شرط مهارت
            if (!string.IsNullOrEmpty(c.RequiredSkill) && state is not null)
            {
                var skillLevel = c.RequiredSkill switch
                {
                    "Perception" => state.SkillPerception,
                    "Persuasion" => state.SkillPersuasion,
                    "Intimidation" => state.SkillIntimidation,
                    "Empathy" => state.SkillEmpathy,
                    "Combat" => state.SkillCombat,
                    _ => 0
                };
                if (skillLevel < c.RequiredSkillLevel) return false;
            }

            return true;
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
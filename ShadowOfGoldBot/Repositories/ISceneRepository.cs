using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Repositories;

public interface ISceneRepository
{
    Task<Scene?> GetByIdAsync(int id);
    Task<Scene?> GetWithChoicesAsync(int id);
    Task<List<Choice>> GetAvailableChoicesAsync(int sceneId, Player player);
    Task<List<Scene>> GetAllAsync();
    Task AddAsync(Scene scene);
    Task SaveChangesAsync();
}
using Microsoft.EntityFrameworkCore;
using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerState> PlayerStates => Set<PlayerState>();
    public DbSet<PlayerFlag> PlayerFlags => Set<PlayerFlag>();
    public DbSet<Scene> Scenes => Set<Scene>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<Ending> Endings => Set<Ending>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasIndex(p => p.TelegramUserId)
            .IsUnique();

        modelBuilder.Entity<Player>()
            .HasOne(p => p.State)
            .WithOne(s => s.Player)
            .HasForeignKey<PlayerState>(s => s.PlayerId);

        modelBuilder.Entity<Choice>()
            .HasOne(c => c.Scene)
            .WithMany(s => s.Choices)
            .HasForeignKey(c => c.SceneId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
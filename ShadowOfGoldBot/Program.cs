using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using ShadowOfGoldBot.Data;
using ShadowOfGoldBot.Handlers;
using ShadowOfGoldBot.Repositories;
using ShadowOfGoldBot.Services;
using ShadowOfGoldBot.Telegram;

DotNetEnv.Env.TraversePath().Load();

var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? "";

if (string.IsNullOrEmpty(botToken))
{
    Console.WriteLine("❌ BOT_TOKEN is not set.");
    return;
}

var proxy = new System.Net.WebProxy("http://127.0.0.1:2080");
var httpClient = new HttpClient(new HttpClientHandler
{
    Proxy = proxy,
    UseProxy = true
});

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging => logging.ClearProviders())
    .ConfigureServices(services =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=ShadowOfGoldBot.db"));

        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ISceneRepository, SceneRepository>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<GameHandler>();
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken, httpClient));
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DataSeeder.SeedAsync(db);
}

var bot = host.Services.GetRequiredService<ITelegramBotClient>();
var me = await bot.GetMe();
Console.WriteLine($"🤖 Bot: @{me.Username}");
Console.WriteLine("Ctrl+C to stop.");

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery }
};

bot.StartReceiving(
    updateHandler: async (botClient, update, ct) =>
    {
        using var scope = host.Services.CreateScope();
        var gameHandler = scope.ServiceProvider.GetRequiredService<GameHandler>();
        var telegramHandler = new TelegramHandler(botClient, gameHandler);
        await telegramHandler.HandleUpdateAsync(update, ct);
    },
    errorHandler: async (botClient, ex, ct) =>
    {
        Console.WriteLine($"❌ {ex.Message}");
        await Task.CompletedTask;
    },
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

await Task.Delay(-1, cts.Token);
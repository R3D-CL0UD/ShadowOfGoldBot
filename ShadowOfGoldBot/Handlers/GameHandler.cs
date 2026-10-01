using System.Net;
using ShadowOfGoldBot.Models;
using ShadowOfGoldBot.Services;

namespace ShadowOfGoldBot.Handlers;

public class GameHandler
{
    private readonly IGameService _gameService;

    public GameHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<string> HandleStartAsync(long userId, string firstName, string? username)
    {
        var player = await _gameService.GetOrCreatePlayerAsync(userId, firstName, username);
        return await GetCurrentSceneTextAsync(userId);
    }

    public async Task<string> GetCurrentSceneTextAsync(long userId)
    {
        var scene = await _gameService.GetCurrentSceneAsync(userId);
        if (scene is null) return "Scene not found.";

        if (scene.IsEnding)
        {
            var player = await _gameService.GetOrCreatePlayerAsync(userId, "", null);
            return GetEndingText(player);
        }

        var text = $"<b>{scene.Title}</b>\n\n{WebUtility.HtmlEncode(scene.Description)}\n\n━━━━━━━━━━━━━━━━\n\n<b>What do you do?</b>";
        return text;
    }

    public async Task<List<Choice>> GetCurrentChoicesAsync(long userId)
    {
        return await _gameService.GetCurrentChoicesAsync(userId);
    }

    public async Task<string> HandleChoiceAsync(long userId, int choiceNumber)
    {
        var scene = await _gameService.MakeChoiceAsync(userId, choiceNumber - 1);
        if (scene is null) return "Invalid choice.";

        return await GetCurrentSceneTextAsync(userId);
    }

    public async Task<string> HandleRestartAsync(long userId)
    {
        await _gameService.RestartAsync(userId);
        return await GetCurrentSceneTextAsync(userId);
    }

    public async Task<string> HandleStatsAsync(long userId)
    {
        var player = await _gameService.GetOrCreatePlayerAsync(userId, "Player", null);
        if (player.State is null) return "No stats available.";

        return $@"📊 <b>Your Stats</b>

Reputation (Town): {player.State.ReputationTown}
Reputation (Sheriff): {player.State.ReputationSheriff}
Reputation (Gang): {player.State.ReputationGang}
Trust (May): {player.State.TrustMay}
Karma (Greed): {player.State.KarmaGreed}

Flags: {player.Flags.Count}";
    }

    // ==================== ENDING LOGIC ====================

    private static string GetEndingText(Player player)
    {
        var flags = player.Flags.Select(f => f.FlagName).ToHashSet();
        var s = player.State;

        // 1. Traitor — gave bag or surrendered
        if (flags.Contains("GaveBagToGang") || flags.Contains("ChoseSurrender"))
            return EndingTraitor;

        // 2. Neutrality — ran away
        if (flags.Contains("ChoseRun"))
            return EndingNeutrality;

        // 3. Judge — took gold to Sheriff and told Sheriff
        if (flags.Contains("GaveToSheriff") && flags.Contains("ToldSheriff"))
            return EndingJudge;

        // 4. Redemption — gave gold to May and promised to help
        if (flags.Contains("GaveGoldToMay") && flags.Contains("PromisedToHelp"))
            return EndingRedemption;

        // 5. Quiet Sacrifice — promised to help and chose to fight
        if (flags.Contains("PromisedToHelp") && flags.Contains("ChoseFight"))
            return EndingSacrifice;

        // 6. The Fall — fought, kept gold, greedy
        if (flags.Contains("ChoseFight") && flags.Contains("HasGold"))
            return EndingFall;

        // 7. Reluctant Hero — fought
        if (flags.Contains("ChoseFight"))
            return EndingHero;

        return EndingFall;
    }

    private const string EndingRedemption = @"🏆 <b>Ending: Redemption</b>

The gold returns to the Patterson family. A U.S. Marshal — already tracking the gang — arrests them.

May and her child leave for California and start a new life.

You stay in Canyon Creek. A simple shopkeeper. But at peace.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingFall = @"💀 <b>Ending: The Fall</b>

The gold is yours. You fight the gang and win. But two men die. One of them was a young outlaw, barely eighteen.

The town turns against you. Your store thrives, but you are alone.

The gold is heavy. Heavier than you imagined.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingNeutrality = @"🚶 <b>Ending: Neutrality</b>

You take the gold and flee Canyon Creek in the middle of the night. You change your name. You move west.

But the gang hunts you. You start a new life — but always in fear. Always looking over your shoulder.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingTraitor = @"🐍 <b>Ending: The Traitor</b>

You hand the gold to the gang. They leave. You survive.

But May and her child are left with nothing. The Sheriff finds out. The town casts you out.

You never open a store again. You live in shame.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingHero = @"⭐ <b>Ending: The Reluctant Hero</b>

You fight the gang and win. You return the gold to the town.

You become a legend. Children tell stories about you.

But you never wanted this. You just wanted a quiet life.

A simple man forced to be a hero.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingSacrifice = @"🕊️ <b>Ending: The Quiet Sacrifice</b>

You give the gold to May and tell her to leave town. You stay behind to face the gang alone.

You die. But May and her child escape.

The town remembers you as a coward for a year. Then the truth comes out.

Too late, but it comes out. You are honored.

━━━━━━━━━━━━━━━━
Type /restart to play again.";

    private const string EndingJudge = @"⚖️ <b>Ending: The Judge</b>

You take the gold to the Sheriff. The gang is arrested. A trial is held.

May testifies. The Patterson family gets their justice.

You are asked to testify. You do. You tell the truth.

The town respects you. Cole's name is cleared.

━━━━━━━━━━━━━━━━
Type /restart to play again.";
}
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

        bool isNewGame = player.CurrentSceneId == 1 && !player.Flags.Any();

        if (isNewGame)
        {
            var welcome = @"🌵 <b>سایه‌ی طلا</b>

کانیون کریک، ۱۸۸۷.
عصر یاغی‌ها داره تموم می‌شه. راه‌آهن داره میاد، قانون داره گسترش پیدا می‌کنه. ولی هنوز جاهایی هست که تفنگ حرف اول رو می‌زنه.

تو <b>جان</b> هستی. یه مغازه‌دار ساده. نه هفت‌تیرکش ماهری، نه قهرمانی.

امشب یه غریبه میاد. یه کیسه. یه انتخاب.

آماده‌ای؟

━━━━━━━━━━━━━━━━

";
            var sceneText = await GetCurrentSceneTextAsync(userId);
            return welcome + sceneText;
        }

        return await GetCurrentSceneTextAsync(userId);
    }

    public async Task<string> GetCurrentSceneTextAsync(long userId)
    {
        var scene = await _gameService.GetCurrentSceneAsync(userId);
        if (scene is null) return "صحنه پیدا نشد.";

        if (scene.IsEnding)
        {
            var player = await _gameService.GetOrCreatePlayerAsync(userId, "", null);
            return GetEndingText(player);
        }

        var choices = await _gameService.GetCurrentChoicesAsync(userId);

        var text = $"<b>{scene.Title}</b>\n\n{WebUtility.HtmlEncode(scene.Description)}\n\n━━━━━━━━━━━━━━━━\n\n<b>چه می‌کنی؟</b>\n\n";

        for (int i = 0; i < choices.Count; i++)
        {
            text += $"<b>{i + 1}.</b> {WebUtility.HtmlEncode(choices[i].Text)}\n";
        }

        return text;
    }

    public async Task<List<Choice>> GetCurrentChoicesAsync(long userId)
    {
        return await _gameService.GetCurrentChoicesAsync(userId);
    }

    public async Task<string> HandleChoiceAsync(long userId, int choiceNumber)
    {
        var playerBefore = await _gameService.GetOrCreatePlayerAsync(userId, "", null);
        var stateBefore = SnapshotState(playerBefore.State);

        var scene = await _gameService.MakeChoiceAsync(userId, choiceNumber - 1);
        if (scene is null) return "انتخاب نامعتبر.";

        var playerAfter = await _gameService.GetOrCreatePlayerAsync(userId, "", null);
        var changes = GetChanges(stateBefore, playerAfter.State);

        var sceneText = await GetCurrentSceneTextAsync(userId);

        if (!string.IsNullOrEmpty(changes))
            return $"<i>{changes}</i>\n\n{sceneText}";

        return sceneText;
    }

    public async Task<string> HandleRestartAsync(long userId)
    {
        await _gameService.RestartAsync(userId);
        return await GetCurrentSceneTextAsync(userId);
    }

    public async Task<string> HandleStatsAsync(long userId)
    {
        var player = await _gameService.GetOrCreatePlayerAsync(userId, "Player", null);
        if (player.State is null) return "آماری موجود نیست.";

        var s = player.State;

        return $@"📊 <b>وضعیت جان</b>

❤️ سلامت: {s.Health}
🧠 روحیه: {s.Morale}

<b>اعتبار</b>
🏘 شهر: {FormatNum(s.ReputationTown)}
👮 کلانتر: {FormatNum(s.ReputationSheriff)}
🤠 دار و دسته: {FormatNum(s.ReputationGang)}
🔫 یاغی‌ها: {FormatNum(s.ReputationOutlaws)}

<b>کارما</b>
💰 طمع: {FormatNum(s.KarmaGreed)}
⚔️ شرافت: {FormatNum(s.KarmaHonor)}
🕊 رحم: {FormatNum(s.KarmaMercy)}

<b>مهارت‌ها</b>
👁 ادراک: {s.SkillPerception}/10
🗣 متقاعدسازی: {s.SkillPersuasion}/10
😠 تهدید: {s.SkillIntimidation}/10
💗 همدلی: {s.SkillEmpathy}/10
🔫 نبرد: {s.SkillCombat}/10

<b>پرچم‌ها:</b> {player.Flags.Count}";
    }

    // ==================== HELPERS ====================

    private static string FormatNum(int n) => n > 0 ? $"+{n}" : n.ToString();

    private static (int Health, int Morale, int RepTown, int RepSheriff, int RepGang, int RepOutlaws,
        int TrustMartha, int TrustSheriff, int TrustHartley, int TrustTom, int TrustSara, int TrustCole,
        int KarmaGreed, int KarmaHonor, int KarmaMercy,
        int SkillPerception, int SkillPersuasion, int SkillIntimidation, int SkillEmpathy, int SkillCombat)
        SnapshotState(PlayerState? s)
    {
        if (s is null) return default;
        return (s.Health, s.Morale,
            s.ReputationTown, s.ReputationSheriff, s.ReputationGang, s.ReputationOutlaws,
            s.TrustMartha, s.TrustSheriff, s.TrustHartley, s.TrustTom, s.TrustSara, s.TrustCole,
            s.KarmaGreed, s.KarmaHonor, s.KarmaMercy,
            s.SkillPerception, s.SkillPersuasion, s.SkillIntimidation, s.SkillEmpathy, s.SkillCombat);
    }

    private static string GetChanges(
        (int Health, int Morale, int RepTown, int RepSheriff, int RepGang, int RepOutlaws,
        int TrustMartha, int TrustSheriff, int TrustHartley, int TrustTom, int TrustSara, int TrustCole,
        int KarmaGreed, int KarmaHonor, int KarmaMercy,
        int SkillPerception, int SkillPersuasion, int SkillIntimidation, int SkillEmpathy, int SkillCombat) before,
        PlayerState? after)
    {
        if (after is null) return string.Empty;

        var parts = new List<string>();

        void Add(string text, int diff)
        {
            if (diff == 0) return;
            var sign = diff > 0 ? "+" : "-";
            parts.Add($"{text} {sign}{Math.Abs(diff)}");
        }

        Add("🏘 اعتبارت پیش مردم شهر", after.ReputationTown - before.RepTown);
        Add("👮 اعتبارت پیش کلانتر", after.ReputationSheriff - before.RepSheriff);
        Add("🤠 اعتبارت پیش دار و دسته", after.ReputationGang - before.RepGang);
        Add("💰 طمعت", after.KarmaGreed - before.KarmaGreed);
        Add("⚔️ شرافتت", after.KarmaHonor - before.KarmaHonor);
        Add("🕊 رحمت", after.KarmaMercy - before.KarmaMercy);
        Add("💗 اعتماد سارا بهت", after.TrustSara - before.TrustSara);
        Add("👁 ادراک", after.SkillPerception - before.SkillPerception);
        Add("🗣 متقاعدسازی", after.SkillPersuasion - before.SkillPersuasion);
        Add("😠 تهدید", after.SkillIntimidation - before.SkillIntimidation);
        Add("💗 همدلی", after.SkillEmpathy - before.SkillEmpathy);
        Add("🔫 نبرد", after.SkillCombat - before.SkillCombat);

        return parts.Count == 0 ? string.Empty : "📌 " + string.Join("\n📌 ", parts);
    }

    // ==================== ENDINGS ====================

    private static string GetEndingText(Player player)
    {
        var flags = player.Flags.Select(f => f.FlagName).ToHashSet();

        if (flags.Contains("GaveBagToGang") || flags.Contains("ChoseSurrender"))
            return EndingTraitor;

        if (flags.Contains("ChoseRun"))
            return EndingNeutrality;

        if (flags.Contains("GaveToSheriff") && flags.Contains("ToldSheriff"))
            return EndingJudge;

        if (flags.Contains("GaveGoldToMay") && flags.Contains("PromisedToHelp"))
            return EndingRedemption;

        if (flags.Contains("PromisedToHelp") && flags.Contains("ChoseFight"))
            return EndingSacrifice;

        if (flags.Contains("ChoseFight") && flags.Contains("HasGold"))
            return EndingFall;

        if (flags.Contains("ChoseFight"))
            return EndingHero;

        return EndingFall;
    }

    private const string EndingRedemption = @"🏆 <b>پایان: رستگاری</b>

طلا به خانواده‌ی پترسون برمی‌گردد. یک مارشال ایالات متحده — که از قبل دنبال دار و دسته بود — آن‌ها را دستگیر می‌کند.

سارا و بچه‌اش به کالیفرنیا می‌روند و زندگی جدیدی را شروع می‌کنند.

تو در کانیون کریک می‌مانی. یک مغازه‌دار ساده. ولی در آرامش.

جان، تو کار درست را انجام دادی.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingFall = @"💀 <b>پایان: سقوط</b>

طلا مال تو می‌شود. با دار و دسته می‌جنگی و پیروز می‌شوی. ولی دو مرد می‌میرند. یکی از آن‌ها یک یاغی جوان بود.

شهر علیه تو می‌شود. مغازه‌ات رونق می‌گیرد، ولی تنها می‌مانی.

طلا سنگین است. سنگین‌تر از آنچه تصور می‌کردی.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingNeutrality = @"🚶 <b>پایان: بی‌طرفی</b>

طلا را برمی‌داری و در نیمه‌شب از کانیون کریک فرار می‌کنی. نامت را عوض می‌کنی. به سمت غرب می‌روی.

ولی دار و دسته دنبالت هستند. زندگی جدیدی شروع می‌کنی — ولی همیشه در ترس.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingTraitor = @"🐍 <b>پایان: خائن</b>

طلا را به دار و دسته می‌دهی. می‌روند. زنده می‌مانی.

ولی سارا و بچه‌اش را فروختی. کلانتر می‌فهمد. شهر طردت می‌کند.

در شرم زندگی می‌کنی.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingHero = @"⭐ <b>پایان: قهرمان ناخواسته</b>

با دار و دسته می‌جنگی و پیروز می‌شوی. طلا را به شهر برمی‌گردانی.

به یک افسانه تبدیل می‌شوی.

ولی تو هرگز این را نمی‌خواستی. تو فقط یک زندگی آرام می‌خواستی.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingSacrifice = @"🕊️ <b>پایان: فداکاری خاموش</b>

طلا را به سارا می‌دهی و می‌گویی از شهر برود. خودت می‌مانی تا تنها با دار و دسته روبرو شوی.

می‌میری. ولی سارا و بچه‌اش فرار می‌کنند.

شهر یک سال تو را ترسو می‌داند. بعد حقیقت آشکار می‌شود.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";

    private const string EndingJudge = @"⚖️ <b>پایان: قاضی</b>

طلا را به کلانتر می‌بری. دار و دسته دستگیر می‌شوند. محاکمه برگزار می‌شود.

سارا شهادت می‌دهد. خانواده‌ی پترسون عدالتشان را می‌گیرند.

شهر به تو احترام می‌گذارد. نام ری پاک می‌شود.

━━━━━━━━━━━━━━━━
برای بازی مجدد /restart بزن.";
}
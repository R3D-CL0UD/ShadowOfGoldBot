using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ShadowOfGoldBot.Handlers;

namespace ShadowOfGoldBot.Telegram;

public class TelegramHandler
{
    private const string ChannelUsername = "@DISC0ELYS1UM";
    private const string ChannelUrl = "https://t.me/DISC0ELYS1UM";

    private readonly ITelegramBotClient _bot;
    private readonly GameHandler _gameHandler;

    public TelegramHandler(ITelegramBotClient bot, GameHandler gameHandler)
    {
        _bot = bot;
        _gameHandler = gameHandler;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken ct)
    {
        try
        {
            if (update.Type == UpdateType.Message && update.Message?.Text is { } text)
                await HandleMessageAsync(update.Message, text, ct);
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is { } cb)
                await HandleCallbackAsync(cb, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task<bool> IsUserMemberAsync(long userId, CancellationToken ct)
    {
        try
        {
            var member = await _bot.GetChatMember(ChannelUsername, userId, ct);
            return member.Status is ChatMemberStatus.Member
                or ChatMemberStatus.Administrator
                or ChatMemberStatus.Creator;
        }
        catch
        {
            return false;
        }
    }

    private async Task SendJoinRequestAsync(long chatId, CancellationToken ct)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithUrl("📢 عضویت در کانال", ChannelUrl) },
            new[] { InlineKeyboardButton.WithCallbackData("✅ عضو شدم", "check_membership") }
        });

        await _bot.SendMessage(chatId,
            "🌵 برای استفاده از <b>سایه‌ی طلا</b>، اول باید عضو کانال ما بشی:\n\n" +
            $"{ChannelUsername}\n\n" +
            "بعد از عضویت، روی دکمه‌ی «✅ عضو شدم» بزن.",
            parseMode: ParseMode.Html,
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    private async Task HandleMessageAsync(Message message, string text, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var userId = message.From!.Id;
        var firstName = message.From.FirstName;
        var username = message.From.Username;

        if (!await IsUserMemberAsync(userId, ct))
        {
            await SendJoinRequestAsync(chatId, ct);
            return;
        }

        switch (text)
        {
            case "/start":
                var startText = await _gameHandler.HandleStartAsync(userId, firstName, username);
                await SendSceneAsync(chatId, userId, startText, ct);
                break;

            case "/stats":
                await _bot.SendMessage(chatId, await _gameHandler.HandleStatsAsync(userId),
                    parseMode: ParseMode.Html, cancellationToken: ct);
                break;

            case "/restart":
                var r = await _gameHandler.HandleRestartAsync(userId);
                await SendSceneAsync(chatId, userId, r, ct);
                break;

            case "/help":
                await _bot.SendMessage(chatId,
                    "<b>📖 سایه‌ی طلا</b>\n\n" +
                    "/start — شروع بازی جدید\n" +
                    "/restart — شروع از اول\n" +
                    "/stats — دیدن اعتبار\n" +
                    "/help — همین پیام\n" +
                    "/about — درباره‌ی بازی",
                    parseMode: ParseMode.Html, cancellationToken: ct);
                break;

            case "/about":
                await _bot.SendMessage(chatId,
                    "<b>🌵 سایه‌ی طلا</b>\n\n" +
                    "یک داستان تعاملی غرب وحشی در کانیون کریک، ۱۸۸۷.",
                    parseMode: ParseMode.Html, cancellationToken: ct);
                break;

            default:
                await _bot.SendMessage(chatId, "برای شروع /start بزن، برای راهنما /help.",
                    cancellationToken: ct);
                break;
        }
    }

    private async Task HandleCallbackAsync(CallbackQuery callback, CancellationToken ct)
    {
        await _bot.AnswerCallbackQuery(callback.Id, cancellationToken: ct);

        if (callback.Message is null || callback.Data is null) return;

        var chatId = callback.Message.Chat.Id;
        var messageId = callback.Message.MessageId;
        var userId = callback.From.Id;

        if (callback.Data == "check_membership")
        {
            if (await IsUserMemberAsync(userId, ct))
            {
                await _bot.EditMessageText(chatId, messageId,
                    "✅ عضویتت تایید شد.\n\nبرای شروع بازی /start بزن.",
                    cancellationToken: ct);
            }
            else
            {
                await _bot.SendMessage(chatId,
                    "❌ هنوز عضو کانال نشدی. اول عضو شو، بعد دوباره امتحان کن.",
                    cancellationToken: ct);
            }
            return;
        }

        if (callback.Data.StartsWith("choice_"))
        {
            var choiceNum = int.Parse(callback.Data.Substring(7));
            var result = await _gameHandler.HandleChoiceAsync(userId, choiceNum);

            if (result == "انتخاب نامعتبر.")
            {
                await _bot.SendMessage(chatId, "⚠ انتخاب نامعتبر.", cancellationToken: ct);
                return;
            }

            await EditSceneAsync(chatId, messageId, userId, result, ct);
        }
    }

    private async Task SendSceneAsync(long chatId, long userId, string sceneText, CancellationToken ct)
    {
        var choices = await _gameHandler.GetCurrentChoicesAsync(userId);

        if (choices.Count == 0)
        {
            await _bot.SendMessage(chatId, sceneText, parseMode: ParseMode.Html, cancellationToken: ct);
            return;
        }

        var keyboard = BuildKeyboard(choices);
        await _bot.SendMessage(chatId, sceneText, parseMode: ParseMode.Html,
            replyMarkup: keyboard, cancellationToken: ct);
    }

    private async Task EditSceneAsync(long chatId, int messageId, long userId, string sceneText, CancellationToken ct)
    {
        var choices = await _gameHandler.GetCurrentChoicesAsync(userId);

        if (choices.Count == 0)
        {
            await _bot.EditMessageText(chatId, messageId, sceneText,
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(Array.Empty<InlineKeyboardButton[]>()),
                cancellationToken: ct);
            return;
        }

        var keyboard = BuildKeyboard(choices);
        await _bot.EditMessageText(chatId, messageId, sceneText, parseMode: ParseMode.Html,
            replyMarkup: keyboard, cancellationToken: ct);
    }

    private static InlineKeyboardMarkup BuildKeyboard(List<Models.Choice> choices)
    {
        var buttons = choices.Select((c, i) =>
            InlineKeyboardButton.WithCallbackData($"{i + 1}", $"choice_{i + 1}")
        ).ToList();

        var rows = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < buttons.Count; i += 5)
        {
            rows.Add(buttons.Skip(i).Take(5).ToArray());
        }

        return new InlineKeyboardMarkup(rows);
    }
}
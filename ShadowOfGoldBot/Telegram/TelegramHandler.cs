using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ShadowOfGoldBot.Handlers;

namespace ShadowOfGoldBot.Telegram;

public class TelegramHandler
{
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

    private async Task HandleMessageAsync(Message message, string text, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var userId = message.From!.Id;
        var firstName = message.From.FirstName;
        var username = message.From.Username;

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
                    "<b>📖 Shadow of Gold</b>\n\n" +
                    "/start — Begin a new game\n" +
                    "/restart — Start over\n" +
                    "/stats — View your reputation\n" +
                    "/help — Show this message\n" +
                    "/about — About the game",
                    parseMode: ParseMode.Html, cancellationToken: ct);
                break;

            case "/about":
                await _bot.SendMessage(chatId,
                    "<b>🌵 Shadow of Gold</b>\n\n" +
                    "An interactive western tale set in Canyon Creek, 1887.",
                    parseMode: ParseMode.Html, cancellationToken: ct);
                break;

            default:
                await _bot.SendMessage(chatId, "Use /start to begin, /help for commands.",
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

        if (callback.Data.StartsWith("choice_"))
        {
            var choiceNum = int.Parse(callback.Data.Substring(7));
            var result = await _gameHandler.HandleChoiceAsync(userId, choiceNum);

            if (result == "Invalid choice.")
            {
                await _bot.SendMessage(chatId, "⚠ Invalid choice.", cancellationToken: ct);
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
            // حذف کیبورد قبلی
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
        var rows = choices.Select((c, i) =>
            new[] { InlineKeyboardButton.WithCallbackData($"{i + 1}. {c.Text}", $"choice_{i + 1}") }
        ).ToList();

        return new InlineKeyboardMarkup(rows);
    }
}
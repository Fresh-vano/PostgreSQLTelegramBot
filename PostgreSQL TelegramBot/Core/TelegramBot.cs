using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using PostgreSQL_TelegramBot.Actions;
using PostgreSQL_TelegramBot.Views;
using MongoDB.Driver;

namespace PostgreSQL_TelegramBot.Core
{
	public class UserSection
	{
		public ChatId? ChatId { get; set; }
		public DatabaseListener Listener { get; set; }
	}

	public class TelegramBot
	{
		private readonly TelegramBotClient _botClient;
		private readonly CancellationTokenSource _cts;
		private readonly ReceiverOptions _receiverOptions;
		private readonly Dictionary<string, UserSection> _sections;

		public TelegramBot(string token, List<DatabaseInfo> databases)
		{
			_botClient = new TelegramBotClient(token);

			_sections = new Dictionary<string, UserSection>();

			foreach (var db in databases)
			{
				var listener = new DatabaseListener(db);
				listener.ProblemDetected += HandleProblemDetected!;
				_sections[db.Owner] = new UserSection { Listener = listener, ChatId = null };
			}

			_cts = new CancellationTokenSource();
			_receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { },
			};
		}

		public async void Start()
		{
			_botClient.StartReceiving(
				HandleUpdateAsync,
				HandleErrorAsync,
				_receiverOptions,
				_cts.Token
			);

			foreach (var value in _sections.Values)
			{
				await value.Listener.Run();
			}
		}

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
			if (update.Type == UpdateType.Message)
			{
				var message = update.Message;
				var user = update.Message.Chat.Username;
				var chat = message.Chat;

				if (!await ValidateUserAsync(user, chat))
					return;

				_sections[update.Message.Chat.Username].ChatId = update.Message.Chat;

				if (message?.Text?.ToLower() == "/start")
				{
					await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать");
					return;
				}
				else if (message?.Text?.ToLower() == "/status")
				{
					await HandleStatusGet(message.Chat, _sections[message.Chat.Username].Listener);
					return;
				}
				else if (message?.Text?.ToLower() == "/metrics")
				{
					await HandleMetricsGet(message.Chat, _sections[message.Chat.Username].Listener);
					return;
				}

				await botClient.SendTextMessageAsync(message.Chat, "Не выбрана ни одна команда");
			}
			else if (update.Type == UpdateType.CallbackQuery)
			{
				var user = update.CallbackQuery.From.Username;
				var chat = update.CallbackQuery.From.Id;

				if (!await ValidateUserAsync(user, chat))
					return;

				_sections[user].ChatId = chat;

				var listener = _sections[user].Listener;
				var callbackData = update.CallbackQuery.Data;
				var type = ViewActions.Descriptions.First(s => s.Value == callbackData).Key;

				var result = await ActionsPool.ExecuteAsync(type, listener.Database);

				await _botClient.SendTextMessageAsync(update.Message.Chat, result);
			}
		}
		public async Task HandleStatusGet(ChatId chat, DatabaseListener listener)
		{
			string messageText = $"Статус баз данных: {listener.Database.DatabaseName} - {((bool)listener.Metrics[Metrics.MetricType.STATE_METRIC].CurrentValue == true ? "✅" : "❌")}";
			await _botClient.SendTextMessageAsync(chat, messageText.ToString(), parseMode: ParseMode.Html);
		}

		public async Task HandleMetricsGet(ChatId chat, DatabaseListener listener)
		{
			var messageText = new StringBuilder();

			foreach (var metric in listener.Metrics.Values)
			{
				var name		= ViewMetrics.Names[metric.Type];
				var statusText	= ViewMetrics.Results[metric.Result];
				var text		= metric.CurrentValue is null 
					? string.Empty 
					: metric.CurrentValue.ToString();

				messageText.AppendLine($"{name} - <b>{statusText}</b>: {text}");
			}

			await _botClient.SendTextMessageAsync(chat, messageText.ToString(), parseMode: ParseMode.Html);
		}

		public async void HandleProblemDetected(object sender, ProblemDetectedEventArgs e)
		{
			var actions = e.Problem.Actions;
			var inlineKeyboardButtons = actions.Select(act =>
				InlineKeyboardButton.WithCallbackData(ViewActions.Descriptions[act])).ToArray();

			var inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

			var chatId = _sections[e.Database.Owner].ChatId;

			if (chatId is null)
			{
				return;
			}

			await _botClient.SendTextMessageAsync(chatId, GetProblemMessage(e), replyMarkup: inlineKeyboardMarkup);
		}

		public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
		}

		private async Task<bool> ValidateUserAsync(string username, ChatId chatId)
		{
			if (!_sections.ContainsKey(username))
			{
				await _botClient.SendTextMessageAsync(chatId, "Вы не являетесь администратором ни одной базы данных");
				return false;
			}

			return true;
		}

		private string GetProblemMessage(ProblemDetectedEventArgs e)
		{
			StringBuilder res = new StringBuilder();
			return res
				.Append("Обнаружена предполагаемая ошибка.\n")
				.Append($"База данных: {e.Database.DatabaseName}\n")
				.Append(e.Problem.ToString())
				.ToString();
		}
	}
}
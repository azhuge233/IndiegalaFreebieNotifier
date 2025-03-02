using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using IndiegalaFreebieNotifier.Notifier;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Module {
	class NotifyOP : IDisposable {
		private readonly ILogger<NotifyOP> _logger;
		private readonly IServiceProvider services = DI.BuildDiNotifierOnly();

		#region debug strings
		private readonly string debugNotify = "Notify";
		private readonly string debugEnabledFormat = "Sending notifications to {0}";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		private readonly string debugNoNewNotifications = "No new notifications! Skipping";
		#endregion

		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		public async Task Notify(NotifyConfig config, List<FreeGameRecord> pushList) {
			if (pushList.Count == 0) {
				_logger.LogInformation(debugNoNewNotifications);
				return;
			}

			try {
				_logger.LogDebug(debugNotify);

				var notifyTasks = new List<Task>();

				// Telegram notifications
				if (config.EnableTelegram) {
					_logger.LogInformation(debugEnabledFormat, "Telegram");
					notifyTasks.Add(services.GetRequiredService<TgBot>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark) {
					_logger.LogInformation(debugEnabledFormat, "Bark");
					notifyTasks.Add(services.GetRequiredService<Barker>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "Bark");

				// QQ Http notifications
				if (config.EnableQQHttp) {
					_logger.LogInformation(debugEnabledFormat, "QQ Http");
					notifyTasks.Add(services.GetRequiredService<QQHttp>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "QQ Http");

				// QQ WebSocket notifications
				if (config.EnableQQWebSocket) {
					_logger.LogInformation(debugEnabledFormat, "QQ WebSocket");
					notifyTasks.Add(services.GetRequiredService<QQWebSocket>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "QQ WebSocket");

				// PushPlus notifications
				if (config.EnablePushPlus) {
					_logger.LogInformation(debugEnabledFormat, "PushPlus");
					notifyTasks.Add(services.GetRequiredService<PushPlus>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "PushPlus");

				// DingTalk notifications
				if (config.EnableDingTalk) {
					_logger.LogInformation(debugEnabledFormat, "DingTalk");
					notifyTasks.Add(services.GetRequiredService<DingTalk>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "DingTalk");

				// PushDeer notifications
				if (config.EnablePushDeer) {
					_logger.LogInformation(debugEnabledFormat, "PushDeer");
					notifyTasks.Add(services.GetRequiredService<PushDeer>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "PushDeer");

				// Discord notifications
				if (config.EnableDiscord) {
					_logger.LogInformation(debugEnabledFormat, "Discord");
					notifyTasks.Add(services.GetRequiredService<Discord>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "Discord");

				// Email notifications
				if (config.EnableEmail) {
					_logger.LogInformation(debugEnabledFormat, "Email");
					notifyTasks.Add(services.GetRequiredService<Email>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "Email");

				// Meow notifications
				if (config.EnableMeow) {
					_logger.LogInformation(debugEnabledFormat, "Meow");
					notifyTasks.Add(services.GetRequiredService<Meow>().SendMessage(config, pushList));
				} else _logger.LogInformation(debugDisabledFormat, "Meow");

				await Task.WhenAll(notifyTasks);

				_logger.LogDebug($"Done: {debugNotify}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugNotify}");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
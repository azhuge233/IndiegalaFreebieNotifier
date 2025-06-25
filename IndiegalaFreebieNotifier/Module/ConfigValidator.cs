using IndiegalaFreebieNotifier.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IndiegalaFreebieNotifier.Module {
	class ConfigValidator : IDisposable {
		private readonly ILogger<ConfigValidator> _logger;

		private readonly string CookieTestUrl = "https://www.indiegala.com/developers/ajax/add-to-library/6ba4b90d-ec12-4343-bd6f-415f0881ebae/die-young-prologue/freebies";

		private readonly HashSet<string> validResults = ["ok", "added"];

		#region debug strings
		private readonly string debugCheckValid = "Check config file validation";
		#endregion

		public ConfigValidator(ILogger<ConfigValidator> logger) {
			_logger = logger;
		}

		public void CheckValid(Config config) {
			try {
				_logger.LogDebug(debugCheckValid);

				//Telegram
				if (config.EnableTelegram) {
					if (string.IsNullOrEmpty(config.TelegramToken))
						throw new Exception(message: "No Telegram Token provided!");
					if (string.IsNullOrEmpty(config.TelegramChatID))
						throw new Exception(message: "No Telegram ChatID provided!");
				}

				//Bark
				if (config.EnableBark) {
					if (string.IsNullOrEmpty(config.BarkAddress))
						throw new Exception(message: "No Bark Address provided!");
					if (string.IsNullOrEmpty(config.BarkToken))
						throw new Exception(message: "No Bark Token provided!");
				}

				//Email
				if (config.EnableEmail) {
					if (string.IsNullOrEmpty(config.FromEmailAddress))
						throw new Exception(message: "No from email address provided!");
					if (string.IsNullOrEmpty(config.ToEmailAddress))
						throw new Exception(message: "No to email address provided!");
					if (string.IsNullOrEmpty(config.SMTPServer))
						throw new Exception(message: "No SMTP server provided!");
					if (string.IsNullOrEmpty(config.AuthAccount))
						throw new Exception(message: "No email auth account provided!");
					if (string.IsNullOrEmpty(config.AuthPassword))
						throw new Exception(message: "No email auth password provided!");
				}

				//QQ Http
				if (config.EnableQQHttp) {
					if (string.IsNullOrEmpty(config.QQHttpAddress))
						throw new Exception(message: "No QQ http address provided!");
					if (string.IsNullOrEmpty(config.QQHttpPort))
						throw new Exception(message: "No QQ http port provided!");
					if (string.IsNullOrEmpty(config.ToQQID))
						throw new Exception(message: "No QQ ID provided!");
					if (string.IsNullOrEmpty(config.QQHttpToken))
						_logger.LogInformation("No QQ Http token provided, make sure to set it right if token is enabled in your server settings.");
				}

				//QQ WebSocket
				if (config.EnableQQWebSocket) {
					if (string.IsNullOrEmpty(config.QQWebSocketAddress))
						throw new Exception(message: "No QQ WebSocket address provided!");
					if (string.IsNullOrEmpty(config.QQWebSocketPort))
						throw new Exception(message: "No QQ WebSocket port provided!");
					if (string.IsNullOrEmpty(config.QQWebSocketToken))
						throw new Exception(message: "No QQ WebSocket token provided!");
					if (string.IsNullOrEmpty(config.ToQQID))
						throw new Exception(message: "No QQ ID provided!");
					if (string.IsNullOrEmpty(config.QQWebSocketToken))
						_logger.LogInformation("No QQ WebSocket token provided, make sure to set it right if token is enabled in your server settings.");
				}

				//PushPlus
				if (config.EnablePushPlus) {
					if (string.IsNullOrEmpty(config.PushPlusToken))
						throw new Exception(message: "No PushPlus token provided!");
				}

				//DingTalk
				if (config.EnableDingTalk) {
					if (string.IsNullOrEmpty(config.DingTalkBotToken))
						throw new Exception(message: "No DingTalk token provided!");
				}

				//PushDeer
				if (config.EnablePushDeer) {
					if (string.IsNullOrEmpty(config.PushDeerToken))
						throw new Exception(message: "No PushDeer token provided!");
				}

				//Discord
				if (config.EnableDiscord) {
					if (string.IsNullOrEmpty(config.DiscordWebhookURL))
						throw new Exception(message: "No Discord Webhook provided!");
				}

				//Meow
				if (config.EnableMeow) {
					if (string.IsNullOrEmpty(config.MeowAddress))
						throw new Exception(message: "No Meow address provided!");
					if (string.IsNullOrEmpty(config.MeowNickname))
						throw new Exception(message: "No Meow nickname provided!");
				}

				//AutoClaim
				if (config.EnableAutoClaim) { 
					if(string.IsNullOrEmpty(config.Cookies))
						throw new Exception(message: "No cookies provided for auto claim!");
				}

				//Cookie Auto Refresh
				if (config.EnableCookieAutoRefresh) { 
					if(string.IsNullOrEmpty(config.TwoCaptchaApiKey))
						throw new Exception(message: "No 2Captcha API key provided for cookie auto refresh!");
					if(string.IsNullOrEmpty(config.Username))
						throw new Exception(message: "No username provided for cookie auto refresh!");
					if(string.IsNullOrEmpty(config.Password))
						throw new Exception(message: "No password provided for cookie auto refresh!");
				}

				_logger.LogDebug($"Done: {debugCheckValid}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugCheckValid}");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task<bool> CheckCookie(string cookie) {
			try {
				_logger.LogDebug("Check cookie validity");

				using var client = new HttpClient();

				client.DefaultRequestHeaders.Add("Cookie", cookie);

				var resp = await client.GetAsync(CookieTestUrl);

				if (resp.IsSuccessStatusCode) {
					var jsonString = await resp.Content.ReadAsStringAsync();

					var jsonData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

					if (jsonData == null || !jsonData.TryGetValue("status", out _) || jsonData["status"] == "login") {
						_logger.LogError("Cookie is invalid or expired");
						return false;
					} else {
						_logger.LogDebug("Cookie is valid");
						return true;
					}
				} else _logger.LogError("HTTP request failed");

				return false;
			} catch (Exception) {
				_logger.LogError("Error: Check cookie validity");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
﻿using IndiegalaFreebieNotifier.Model;
using IndiegalaFreebieNotifier.Models.PostContent;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IndiegalaFreebieNotifier.Notifier {
	public class Discord : INotifiable {
		private readonly ILogger<Discord> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Discord";
		#endregion

		public Discord(ILogger<Discord> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = config.DiscordWebhookURL;
				var content = new DiscordPostContent() {
					Content = records.Count > 1 ? "New Free Games - Indiegala" : "New Free Game - Indiegala"
				};

				foreach (var record in records) {
					content.Embeds.Add(
						new Embed() {
							Title = record.Title,
							Url = record.Url,
							Description = record.ToDiscordMessage(),
							Footer = new Footer() { Text = NotifyFormatStrings.projectLink }
						}
					);
				}

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				var resp = await new HttpClient().PostAsync(url, data);
				_logger.LogDebug(await resp.Content.ReadAsStringAsync());

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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

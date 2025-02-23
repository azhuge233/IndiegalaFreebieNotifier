using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;
using IndiegalaFreebieNotifier.Model.PostContent;

namespace IndiegalaFreebieNotifier.Notifier {
	class DingTalk: INotifiable {
		private readonly ILogger<DingTalk> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to DingTalk";
		#endregion

		public DingTalk(ILogger<DingTalk> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkUrlFormat, config.DingTalkBotToken).ToString();
				var content = new DingTalkPostContent();

				var client = new HttpClient();

				foreach (var record in records) {
					content.Text.Content_ = $"{record.ToDingTalkMessage()}{NotifyFormatStrings.projectLink}";
					var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					var resp = await client.PostAsync(url, data);
					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
				}

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

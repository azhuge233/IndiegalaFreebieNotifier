using IndiegalaFreebieNotifier.Model;
using IndiegalaFreebieNotifier.Model.String;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IndiegalaFreebieNotifier.Module {
	internal class AutoClaimer : IDisposable {
		private readonly ILogger<AutoClaimer> _logger;

		private readonly string claimUrlPattern = "https://www.indiegala.com/developers/ajax/add-to-library/{0}/{1}/freebies";

		private HttpClient Client { get; set; } = new();

		public AutoClaimer(ILogger<AutoClaimer> logger) {
			_logger = logger;
		}

		public async Task Claim(Config config, List<FreeGameRecord> records) {
			if (!config.EnableAutoClaim) {
				_logger.LogInformation(AutoClaimerStrings.debugAutoClaimDisabled);
				return;
			}

			if (string.IsNullOrEmpty(config.Cookies)) {
				_logger.LogWarning(AutoClaimerStrings.debugNoCookie);
				return;
			}

			try {
				_logger.LogDebug(AutoClaimerStrings.debugAutoClaim);

				foreach (var record in records) { 
					_logger.LogInformation(AutoClaimerStrings.debugClaiming, record.Title);

					string urlName = record.Url.Split('/').Last();

					var resquest = new HttpRequestMessage() {
						Method = HttpMethod.Post,
						RequestUri = new Uri(string.Format(claimUrlPattern, record.ID, urlName)),
						Headers = {
							{ AutoClaimerStrings.CookieKey, config.Cookies },
							{ AutoClaimerStrings.UserAgentKey, AutoClaimerStrings.UserAgentValue }
						}
					};

					_logger.LogDebug(AutoClaimerStrings.debugFormattedUrl, resquest.RequestUri.ToString());

					var resp = await Client.SendAsync(resquest);

					var jsonString = await resp.Content.ReadAsStringAsync();

					_logger.LogDebug(jsonString);

					if (resp.IsSuccessStatusCode) {
						var jsonData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

						if (jsonData["status"] == "ok") _logger.LogInformation(AutoClaimerStrings.debugClaimed, record.Title);
						else _logger.LogWarning(AutoClaimerStrings.warningClaimFailed, record.Title, jsonData["status"], jsonData["code"]);
					} else _logger.LogWarning(AutoClaimerStrings.warningHttpFailed, record.Title, resp.StatusCode);
				}

				_logger.LogInformation($"Done: {AutoClaimerStrings.debugAutoClaim}");
			} catch (Exception) {
				_logger.LogError($"Error: {AutoClaimerStrings.debugAutoClaim}");
			}
		}

		public void Dispose() { 
			GC.SuppressFinalize(this);
		}
	}
}

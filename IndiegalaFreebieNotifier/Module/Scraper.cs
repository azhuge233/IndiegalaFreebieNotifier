using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IndiegalaFreebieNotifier.Module {
	class Scraper : IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string homeUrl = "https://freebies.indiegala.com/";

		private HttpClient Client { get; set; } = new();

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
			
		}

		public async Task<string> GetHomeSource() {
			try {
				_logger.LogDebug("Getting home page source");
				var resp = await Client.GetAsync(homeUrl);

				var source = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug("Done");
				return source;
			} catch (Exception) {
				_logger.LogError("Scraping Error");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task<string> GetPageSource(string url) {
			try {
				_logger.LogDebug($"Getting page source: {url}");
				var resp = await Client.GetAsync(url);

				var source = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug("Done");
				return source;
			} catch (Exception) {
				_logger.LogError("Scraping Error");
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

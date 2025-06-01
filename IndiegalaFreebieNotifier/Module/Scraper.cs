using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Module {
	class Scraper : IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string homeUrl = "https://freebies.indiegala.com/";

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
			// From https://github.com/microsoft/playwright-dotnet/issues/1545#issuecomment-865199736
			Microsoft.Playwright.Program.Main(["install", "firefox"]);
		}

		public async Task<string> GetHomeSource(Config config) {
			try {
				_logger.LogDebug("Getting page source");
				var webGet = new HtmlDocument();

				var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Firefox.LaunchAsync(new() { Headless = config.EnableHeadless });

				var page = await browser.NewPageAsync();
				page.SetDefaultTimeout(config.TimeOutMilliSecond);
				page.SetDefaultNavigationTimeout(config.TimeOutMilliSecond);
				await page.RouteAsync("**/*", async route => {
					var blockList = new List<string> { "stylesheet", "image", "font" };
					if (blockList.Contains(route.Request.ResourceType)) await route.AbortAsync();
					else await route.ContinueAsync();
				});

				await page.GotoAsync(homeUrl);
				await page.WaitForLoadStateAsync();
				var source = await page.InnerHTMLAsync("*");
				await page.CloseAsync();

				_logger.LogDebug("Done");
				return source;
			} catch (Exception) {
				_logger.LogError("Scraping Error");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task<string> GetPageSource(string url, Config config) {
			try {
				_logger.LogDebug("Getting page source");
				var webGet = new HtmlDocument();

				var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Firefox.LaunchAsync(new() { Headless = config.EnableHeadless });

				var page = await browser.NewPageAsync();
				page.SetDefaultTimeout(config.TimeOutMilliSecond);
				page.SetDefaultNavigationTimeout(config.TimeOutMilliSecond);
				await page.RouteAsync("**/*", async route => {
					var blockList = new List<string> { "stylesheet", "image", "font" };
					if (blockList.Contains(route.Request.ResourceType)) await route.AbortAsync();
					else await route.ContinueAsync();
				});

				await page.GotoAsync(url);
				await page.WaitForLoadStateAsync();
				var source = await page.InnerHTMLAsync("*");
				await page.CloseAsync();

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

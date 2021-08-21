﻿using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Module {
	class Scraper : IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string url = "https://freebies.indiegala.com/";

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
			Microsoft.Playwright.Program.Main(new string[] { "install", "webkit" });
		}

		public async Task<string> GetHtmlSource(Config config) {
			try {
				_logger.LogDebug("Getting page source");
				var webGet = new HtmlDocument();

				var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Webkit.LaunchAsync(new() { Headless = true });

				var page = await browser.NewPageAsync();
				page.SetDefaultTimeout(config.TimeOutMilliSecond);
				page.SetDefaultNavigationTimeout(config.TimeOutMilliSecond);

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
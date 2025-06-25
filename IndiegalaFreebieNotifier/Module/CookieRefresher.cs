using IndiegalaFreebieNotifier.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndiegalaFreebieNotifier.Module {
	internal class CookieRefresher : IDisposable {
		private readonly ILogger<CookieRefresher> _logger;
		private readonly IServiceProvider services = DI.BuildDiCaptchaSolverOnly();

		private readonly Random rd = new();

		private static readonly string BaseUrl = "https://www.indiegala.com";
		private static readonly string LoginUrl = $"{BaseUrl}/login";

		#region debug strings
		private readonly string debugGetCookie = "Getting new cookie";
		#endregion

		public CookieRefresher(ILogger<CookieRefresher> logger) {
			_logger = logger;

			Microsoft.Playwright.Program.Main(["install", "webkit"]);
		}

		public async Task<string> GetCookie(Config config) {
			try {
				_logger.LogDebug(debugGetCookie);

				using var playwright = await Playwright.CreateAsync();
				var browser = await playwright.Webkit.LaunchAsync(new() {
					Headless = true,
					// Args = new[] { "--disable-web-security", "--disable-features=IsolateOrigins,site-per-process" }
				});

				var page = await browser.NewPageAsync();

				await page.RouteAsync("**/*", async route => {
					// add google.com and gstatic.com to whitelist
					// so browser can properly load recaptcha
					HashSet<string> whitelist = ["indiegala.com", "google.com", "gstatic.com"];
					if (whitelist.Any(domain => route.Request.Url.Contains(domain))) await route.ContinueAsync();
					else await route.AbortAsync();
				});

				await page.GotoAsync(LoginUrl, new() { WaitUntil = WaitUntilState.NetworkIdle });

				await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(config.Username);
				await Task.Delay(GetRandomDelay());
				await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync(config.Password);
				await Task.Delay(GetRandomDelay());

				var solveResult = await services.GetRequiredService<CaptchaSolver>().SolveReCaptchaAsync(config.TwoCaptchaApiKey);
				await page.EvaluateAsync($"solveResult => document.querySelector('#g-recaptcha-response').innerHTML = solveResult", solveResult);

				_logger.LogDebug("Text injected");

				await page.GetByRole(AriaRole.Button, new() { Name = "LOGIN" }).ClickAsync();
				await page.WaitForURLAsync(BaseUrl);

				var cookie = await page.Context.CookiesAsync(BaseUrl);

				await page.CloseAsync();

				string newCookie = $"sessionid={cookie.FirstOrDefault(c => c.Name == "sessionid").Value};";

				_logger.LogInformation($"New cookie: {newCookie}");

				_logger.LogDebug($"Done: {debugGetCookie}");
				return newCookie;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetCookie}");
				throw;
			}
		}

		private int GetRandomDelay() => rd.Next(1, 3) * 500;

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}

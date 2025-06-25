using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwoCaptcha.Captcha;

namespace IndiegalaFreebieNotifier.Module {
	internal class CaptchaSolver: IDisposable {
		private readonly ILogger<Parser> _logger;

		private readonly TwoCaptcha.TwoCaptcha solver;

		#region debug strings
		private readonly string debugCaptchaSolved = "Captcha solved: {0}";
		private readonly string debugSolvingCaptcha = "Solving captcha";
		#endregion

		public CaptchaSolver(ILogger<Parser> logger, string apiKey) {
			_logger = logger;
			solver = new(apiKey);
		}

		public async Task<string> SolveReCaptchaAsync(string siteKey, string pageUrl) {
			try {
				_logger.LogDebug(debugSolvingCaptcha);

				var captcha = new ReCaptcha();

				captcha.SetSiteKey(siteKey);
				captcha.SetUrl(pageUrl);

				await solver.Solve(captcha);

				_logger.LogDebug(debugCaptchaSolved, captcha.Code);

				_logger.LogDebug($"Done: {debugSolvingCaptcha}");
				return captcha.Code;
			} catch (AggregateException ex) {
				_logger.LogError($"Error: {debugSolvingCaptcha}");
				_logger.LogError($"Error: {ex.InnerExceptions.First().Message}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}

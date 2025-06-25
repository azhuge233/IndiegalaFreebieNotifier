using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwoCaptcha.Captcha;

namespace IndiegalaFreebieNotifier.Module {
	internal class CaptchaSolver: IDisposable {
		private readonly ILogger<Parser> _logger;

		private readonly string recaptchaSiteKey = "6Le8buspAAAAAL_KiyJlKizmrag_ZHR7SPi8nvX6";
		private readonly string indiegalaBaseUrl = "https://www.indiegala.com";

		#region debug strings
		private readonly string debugCaptchaSolved = "Captcha solved: {0}";
		private readonly string debugSolvingCaptcha = "Solving captcha";
		#endregion

		public CaptchaSolver(ILogger<Parser> logger) {
			_logger = logger;
		}

		public async Task<string> SolveReCaptchaAsync(string apiKey) {
			try {
				_logger.LogDebug(debugSolvingCaptcha);

				var solver = new TwoCaptcha.TwoCaptcha(apiKey);

				var captcha = new ReCaptcha();

				captcha.SetSiteKey(recaptchaSiteKey);
				captcha.SetUrl(indiegalaBaseUrl);

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

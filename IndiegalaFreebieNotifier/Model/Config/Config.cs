namespace IndiegalaFreebieNotifier.Model {
	public class Config : NotifyConfig {
		public bool EnableAutoClaim { get; set; }
		public string Cookie { get; set; }
		public bool EnableCookieAutoRefresh { get; set; }
		public string TwoCaptchaApiKey { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
namespace IndiegalaFreebieNotifier.Model {
	public class Config : NotifyConfig {
		public bool EnableHeadless { get; set; }
		public int TimeOutMilliSecond { get; set; }
		public bool EnableAutoClaim { get; set; }
		public string Cookies { get; set; }
	}
}
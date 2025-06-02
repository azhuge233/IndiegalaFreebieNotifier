namespace IndiegalaFreebieNotifier.Model {
	public static class ParseStrings {
		#region XPath strings
		public static readonly string divXpath = "//div[@class=\"products-col-inner box-shadow-1 relative\"]";
		public static readonly string titleXpath = ".//div[@class=\"left product-title\"]";
		public static readonly string linkXpath = ".//a";
		public static readonly string imgXpath = ".//figure/img";
		#endregion

		public static readonly string IDRegex = "(?<=products\\/)[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}";
	}
}
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace IndiegalaFreebieNotifier {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly string divXpath = "//div[@class=\"products-col-inner box-shadow-1 relative\"]";
		private readonly string titleXpath = ".//div[@class=\"left product-title\"]";
		private readonly string linkXpath = ".//a";
		private readonly string pushFormat = "<b>Indiegala 信息</b>\n\n<i>{0}</i>\n领取链接: {1}";

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<string>, List<Dictionary<string, string>>> Parse(string source, List<Dictionary<string, string>> records) {
			try {
				_logger.LogDebug("Start parsing");
				var pushList = new List<string>();
				var recordList = new List<Dictionary<string, string>>();

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var freebies = htmlDoc.DocumentNode.SelectNodes(divXpath).ToList();

				foreach (var each in freebies) {
					// get article titles and links
					var title = each.SelectSingleNode(titleXpath).InnerText;
					var link = each.SelectSingleNode(linkXpath).Attributes["href"].Value;

					_logger.LogInformation("Found new info:\n Title: {0}\n Link: {1}", title, link);

					// save titles and links to List
					var tmp = new Dictionary<string, string> {
						{ "title", title },
						{ "url", link}
					};
					recordList.Add(tmp);

					// push list
					if (!records.Where(x => x["title"] == title && x["url"] == link).Any()) {
						_logger.LogInformation("Add {0} to push list", title);

						StringBuilder sb = new();
						sb.AppendFormat(pushFormat, title, link);

						pushList.Add(sb.ToString());
					} else _logger.LogInformation("{0} is found in previous records, stop adding it to push list", title);
				}

				_logger.LogDebug("Done");
				return new Tuple<List<string>, List<Dictionary<string, string>>>(pushList, recordList);
			} catch (Exception) {
				_logger.LogError("Parsing Error");
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

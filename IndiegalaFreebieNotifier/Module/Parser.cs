using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;
using System.Text.RegularExpressions;

namespace IndiegalaFreebieNotifier.Module {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public ParseResult Parse(string source, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug("Start parsing");
				var parseResult = new ParseResult();

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				if (htmlDoc.DocumentNode.SelectNodes(ParseStrings.divXpath) != null) {
					var freebies = htmlDoc.DocumentNode.SelectNodes(ParseStrings.divXpath).ToList();

					foreach (var each in freebies) {
						// get article titles and links
						var title = each.SelectSingleNode(ParseStrings.titleXpath);
						var link = each.SelectSingleNode(ParseStrings.linkXpath);
						var img = each.SelectSingleNode(ParseStrings.imgXpath);

						_logger.LogInformation($"Found new info: {title}");

						var newFreeGame = new FreeGameRecord { 
							Title = title.InnerText,
							Url = link.Attributes["href"].Value,
							ID = Regex.Match(img.Attributes["src"].Value, ParseStrings.IDRegex).Value
						};

						_logger.LogDebug($"New freebie: {newFreeGame.Title} | {newFreeGame.Url} | {newFreeGame.ID}");

						parseResult.Records.Add(newFreeGame);

						// push list
						if (!records.Exists(x => x.Title == newFreeGame.Title && x.Url == newFreeGame.Url)) {
							_logger.LogInformation($"Add {title} to push list");
							parseResult.PushList.Add(newFreeGame);
						} else _logger.LogInformation($"{title} is found in previous records, stop adding it to push list");
					}
				} else _logger.LogInformation("No freebies currently.");

				_logger.LogDebug("Done");
				return parseResult;
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

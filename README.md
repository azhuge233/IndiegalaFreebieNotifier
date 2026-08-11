# IndiegalaFreebieNotifier

A CLI tool that
- Fetchs freebies info from [https://freebies.indiegala.com/](https://freebies.indiegala.com/).
- Sends notifications through Telegram, Bark, Email, QQ, PushPlus, DingTalk, Discord and MeoW.
- Auto claim new freebies use given cookie.
- Auto refresh cookie with [playwright](https://playwright.dev/) and [2Captcha](https://2captcha.com/).

Demo Telegram Channel [@azhuge233_FreeGames](https://telegram.me/azhuge233_FreeGames)

## Build

Install dotnet 10.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

### Publish

```
dotnet publish -c Release -p:PublishDir=/your/path/here -r [win10-x64/osx-x64/linux-x64] --sc
```

## Usage

Set your Telegram Bot token and chat ID in config.json

```json
{
	"TelegramToken": "xxxxxx:xxxxxx",
	"TelegramChatID": "xxxxxxxx"
}
```

Check [wiki](https://github.com/azhuge233/IndiegalaFreebieNotifier/wiki) for more explanations.

### Repeatedly running

The program will not add while/for loop, it's a scraper. To schedule the program, use cron.d in Linux(macOS) or Task Scheduler in Windows.

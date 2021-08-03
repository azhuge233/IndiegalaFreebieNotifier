# IndiegalaFreebieNotifier
Same as [SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet) and [EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)

## Build

### Publish

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64]
```

## Usage

Fill your Telegram Bot token and chat ID in config.json

```json
{
	"TOKEN": "xxxxxx:xxxxxx",
	"CHAT_ID": "xxxxxxxx"
}
```

## To-do
- (Maybe)A python version.

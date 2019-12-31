# novetus.scott.center SDK

Read more about novetus.scott.center API [here](https://novetus.scott.center/API/)


## Example 1
Listing all games with credentials
``` C#
Client NSCClient = Client.FromLogin(Username, Password);
foreach(NSCGame in NSCClient.GetGames())
{
  Console.WriteLine(NSCGame.Map);
}
```

## Example 2
Launching a game with a token and printing the ID
``` C#
Client NSCClient = new Client(Token);
Game NewGame = NSCClient.LaunchGame("2011E", "Universal - Welcome to ROBLOX");
if(NewGame.ID != 0)
{
  Console.WriteLine(NewGame.ID);
}
```

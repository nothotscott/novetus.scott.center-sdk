# novetus.scott.center SDK

Read more about novetus.scott.center API [here]("https://novetus.scott.center/API/")


## Example 1
Listing all games with credentials
``` C#
Client NSCClient = Client.FromLogin(Username, Password);
foreach(NSCGame in NSCClient.GetGames())
{
  Console.WriteLine(NSCGame.Map);
}
```

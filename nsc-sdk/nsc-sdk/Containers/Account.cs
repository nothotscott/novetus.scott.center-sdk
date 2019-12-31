using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nsc_sdk.Containers
{
    public struct Account
    {
        public int ID { get; set; }
        public int MaxGames { get; set; }
        public string DiscordID { get; set; }
        public string DiscordUsername { get; set; }
        public string DiscordDiscriminator { get; set; }
        public string Username { get; set; }

        public Account(int _ID, int _MaxGames, string _DiscordID, string _DiscordUsername, string _DiscordDiscriminator, string _Username)
        {
            ID = _ID;
            MaxGames = _MaxGames;
            DiscordID = _DiscordID;
            DiscordUsername = _DiscordUsername;
            DiscordDiscriminator = _DiscordDiscriminator;
            Username = _Username;
        }

        public static Account FromJObject(JToken JAccount)
        {
            return JsonConvert.DeserializeObject<Account>(JAccount.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nsc_sdk.Containers
{
    public struct Game
    {
        public int ID { get; set; }
        public int VirtualMachineID { get; set; }
        public int AccountID { get; set; }
        public int Port { get; set; }
        public string Client { get; set; }
        public string Map { get; set; }
        public int MaxPlayers { get; set; }
        public string URI { get; set; }
        public string Status { get; set; }
        public int Provisional { get; set; }

        public Game(int _ID, int _VirtualMachineID, int _AccountID, int _Port, string _Client, string _Map, int _MaxPlayers, string _URI, string _Status, int _Provisional)
        {
            ID = _ID;
            VirtualMachineID = _VirtualMachineID;
            AccountID = _AccountID;
            Port = _Port;
            Client = _Client;
            Map = _Map;
            MaxPlayers = _MaxPlayers;
            URI = _URI;
            Status = _Status;
            Provisional = _Provisional;
        }

        public static Game FromJObject(JToken JGame)
        {
            return JsonConvert.DeserializeObject<Game>(JGame.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using nsc_sdk.Containers;
using Newtonsoft.Json.Linq;

namespace nsc_sdk
{
    /// <summary>
    /// Provides a client to interface with novetus.scott.center
    /// </summary>
    public class Client
    {
        internal WebClient NovetusClient;

        private string Token { get; set; }

        public Client(string _Token)
        {
            NovetusClient = new WebClient("https://novetus.scott.center");
            Token = _Token;
        }

        /// <summary>
        /// Creates a client with a token based off provided credentials
        /// </summary>
        /// <param name="Username">Username for scott.center</param>
        /// <param name="Password">Password for scott.center</param>
        /// <returns></returns>
        public static Client FromLogin(string Username, string Password)
        {
            Client Return = null;
            WebClient NovetusClient = new WebClient("https://novetus.scott.center");
            NovetusClient.RequestSync("Login.php", null, new List<RequestParameter> { new RequestParameter("Username", Username), new RequestParameter("Password", Password), new RequestParameter("Rest", 1) }, (JObject Result) =>
            {
                Return = new Client(Result["Result"].ToString());
            }, null, RestSharp.Method.POST);
            return Return;
        }


        public List<Game> GetGames()
        {
            List<Game> Return = new List<Game>();
            NovetusClient.RequestSync("API/GetGame.php", Token, null, (JObject Result) =>
            {
                foreach(JObject JGame in Result["Result"].Children())
                {
                    Return.Add(Game.FromJObject(JGame));
                }
            }, null, RestSharp.Method.GET);
            return Return;
        }

        public Game GetGame(int ID)
        {
            Game Return = new Game();
            NovetusClient.RequestSync("API/GetGame.php", Token, new List<RequestParameter> { new RequestParameter("ID", ID) }, (JObject Result) =>
            {
                Return = Game.FromJObject(Result["Result"]);
            }, null, RestSharp.Method.GET);
            return Return;
        }

        public Game LaunchGame(string Client, string Map)
        {
            Game Return = new Game();
            NovetusClient.RequestSync("API/LaunchGame.php", Token, new List<RequestParameter> { new RequestParameter("Client", Client), new RequestParameter("Map", Map) }, (JObject Result) =>
            {
                Return = Game.FromJObject(Result["Result"]);
            }, null, RestSharp.Method.POST);
            return Return;
        }

        public bool ShutdownGame(int ID)
        {
            bool Return = false;
            NovetusClient.RequestSync("API/ShutdownGame.php", Token, new List<RequestParameter> { new RequestParameter("ID", ID) }, (JObject Result) =>
            {
                Return = true;
            }, null, RestSharp.Method.GET);
            return Return;
        }

        public Account GetAccount(int ID)
        {
            Account Return = new Account();
            NovetusClient.RequestSync("API/GetAccount.php", Token, new List<RequestParameter> { new RequestParameter("ID", ID) }, (JObject Result) =>
            {
                Return = Account.FromJObject(Result["Result"]);
            }, null, RestSharp.Method.GET);
            return Return;
        }
    }
}

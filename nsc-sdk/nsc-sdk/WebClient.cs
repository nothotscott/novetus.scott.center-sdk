using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nsc_sdk
{
    internal enum RequestParameterType
    {
        Default,
        File
    }
    internal struct RequestParameter
    {
        public RequestParameterType Type;
        public string Name;
        public object Data;

        public RequestParameter(string _Name, object _Data)
        {
            Type = RequestParameterType.Default;
            Name = _Name;
            Data = _Data;
        }
        public RequestParameter(RequestParameterType _Type, string _Name, object _Data)
        {
            Type = _Type;
            Name = _Name;
            Data = _Data;
        }
    }

    internal class WebClient
    {
        private static RestRequest CreateRestRequest(string URLExtension, string Token, List<RequestParameter> Parameters, Method _Method)
        {
            RestRequest Request = new RestRequest(URLExtension, _Method);
            if (_Method != Method.GET)
            {
                Request.RequestFormat = DataFormat.Json;
            }
            if (String.IsNullOrEmpty(Token) == false)
            {
                Request.AddParameter("Token", Token);
            }
            if (Parameters != null)
            {
                foreach (RequestParameter Parameter in Parameters)
                {
                    switch (Parameter.Type)
                    {
                        case RequestParameterType.Default:
                            Request.AddParameter(Parameter.Name, Parameter.Data);
                            break;
                        case RequestParameterType.File:
                            Request.AddFile(Parameter.Name, (string)Parameter.Data);
                            break;
                    }
                }
            }

            return Request;
        }

        private static void HandleRequest(IRestResponse RawResponse, Action<JObject> Completion, Action<string> Error)
        {
            try
            {
                JObject Response = JObject.Parse(RawResponse.Content);
                if (Response["Error"] == null || Response["Error"].ToString() == "None")
                {
                    Completion?.Invoke(Response);
                }
                else
                {
                    if (Error == null)
                    {
                        Console.WriteLine("ERROR: " + RawResponse.Request.Resource + " resulted in an error (" + Response["Error"].ToString() + ")");
                        Error.Invoke(RawResponse.Content);
                    }
                    else
                    {
                        Error.Invoke(Response["Error"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + "Cannot decode JSON from " + RawResponse.Request.Resource + " | Exception: " + e.ToString() + " | Response: " + RawResponse.Content);
                if (Error != null)
                {
                    Console.WriteLine("INFORMATION: Reverting to error handler");
                    Error("Unknown");
                }
                else
                {
                    Console.WriteLine("INFORMATION: No error handler to handle exception");
                }
            }
        }

        private readonly RestClient Client;

        public WebClient(string DomainURL)
        {
            Client = new RestClient(DomainURL);
            Client.FollowRedirects = false;
        }

        public RestRequest RequestAsync(string URLExtension, string Token = null, List<RequestParameter> Parameters = null, Action<JObject> Completion = null, Action<string> Error = null, Method _Method = Method.GET)
        {
            RestRequest Request = CreateRestRequest(URLExtension, Token, Parameters, _Method);
            Client.ExecuteAsync(Request, (RawResponse) =>
            {
                HandleRequest(RawResponse, Completion, Error);
            });

            return Request;
        }

        public IRestResponse RequestSync(string URLExtension, string Token = null, List<RequestParameter> Parameters = null, Action<JObject> Completion = null, Action<string> Error = null, Method _Method = Method.GET)
        {
            RestRequest Request = CreateRestRequest(URLExtension, Token, Parameters, _Method);

            IRestResponse RawResponse = Client.Execute(Request);
            HandleRequest(RawResponse, Completion, Error);

            return RawResponse;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI
{
    public class Client
    {
        public string clientId{get;set;}
        public string secretKey{get;set;}
        public string accessToken{get;set;}
        public bool enableDebugMode {get;set;} = false;
        //public Locale locale{get;set;}

        /*
        public Client(string clientId, string secretKey, string localeCode){
            this.Init(clientId, secretKey, localeCode);
        }

        public void Init(string clientId, string secretKey, string localeCode){
            this.clientId = clientId;
            this.secretKey = secretKey;
            this.locale = Locale.GetByCode(localeCode);
        }
        */

        public Client(){
            
        }

        public Client(string clientId, string secretKey){
            this.Init(clientId, secretKey);
        }

        public void Init(string clientId, string secretKey){
            this.clientId = clientId;
            this.secretKey = secretKey;
        }

        public void Authenticate(){
            this.accessToken = this.GetAccessToken();
        }

        public string GetAccessToken(){
            Dictionary<string, string> values = new Dictionary<string, string>{
                { "grant_type", "client_credentials" }
            };
            FormUrlEncodedContent data = new FormUrlEncodedContent(values);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.secretKey}")) );
            HttpResponseMessage response = client.PostAsync("https://us.battle.net/oauth/token", data).Result;
            string accessToken = JObject.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();
            client.Dispose();
            return accessToken;

        }

        public HttpResponseMessage HttpGetWithAuth(string url){
            if(this.enableDebugMode){
                System.Console.WriteLine($"GET: {url}");
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync(url).Result;
            client.Dispose();
            return response;
        }
        
        

    }
}

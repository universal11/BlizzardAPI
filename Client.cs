using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BlizzardAPI.WoW;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI
{
    public class Client
    {
        public Locale locale{get;set;}
        public string clientId{get;set;}
        public string secretKey{get;set;}
        public string accessToken{get;set;}
        public bool enableDebugMode {get;set;} = false;
        private int requestDelayInSeconds {get;set;} = 1;
        private int requestLimit {get;set;} = 30;
        private int requestCounter {get;set;} = 0;
        private HttpClient httpClient = new HttpClient();

    
        public Client(string clientId, string secretKey, string localeCode){
            this.Init(clientId, secretKey, localeCode);
        }

        public void Init(string clientId, string secretKey, string localeCode){
            this.clientId = clientId;
            this.secretKey = secretKey;
            this.locale = Locale.GetByCode(localeCode);
        }
        

        public void Init(string clientId, string secretKey){
            this.clientId = clientId;
            this.secretKey = secretKey;
        }

        public void Authenticate(){
            this.accessToken = this.GetAccessToken();
        }

        public string GetAccessToken(){
            string url = $"{this.locale.authHost}oauth/token";
            Dictionary<string, string> values = new Dictionary<string, string>{
                { "grant_type", "client_credentials" }
            };
            FormUrlEncodedContent data = new FormUrlEncodedContent(values);
            //HttpClient client = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.secretKey}")) );
            if(this.enableDebugMode){
                System.Console.WriteLine($"POST: {url}");
            }
            HttpResponseMessage response = this.httpClient.PostAsync($"{url}", data).Result;
            string accessToken = JObject.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();
            //client.Dispose();
            return accessToken;

        }

        public HttpResponseMessage HttpGetWithAuth(string url){
            if(this.requestCounter == this.requestLimit){
                System.Threading.Thread.Sleep(this.requestDelayInSeconds * 1000);
                this.requestCounter = 0;
            }
            if(this.enableDebugMode){
                System.Console.WriteLine($"GET: {url}");
            }
            //HttpClient client = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = this.httpClient.GetAsync(url).Result;
            if(!response.IsSuccessStatusCode){
                System.Console.WriteLine($"Error Code: {response.StatusCode} | {response.Content.ReadAsStringAsync().Result}");
                return null;
            }
            this.requestCounter++;
            /*
            if(this.enableDebugMode){
                System.Console.WriteLine($"Response: {response.Content.ReadAsStringAsync().Result}");
            }
            */
            //client.Dispose();
            return response;
        }
        
        
        

    }
}

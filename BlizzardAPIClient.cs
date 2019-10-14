using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI
{
    public class BlizzardAPIClient
    {
        public string clientId{get;set;}
        public string secretKey{get;set;}
        public string accessToken{get;set;}
        public Locale locale{get;set;}

        public BlizzardAPIClient(string clientId, string secretKey, string localeCode){
            this.clientId = clientId;
            this.secretKey = secretKey;
            this.locale = Locale.GetByCode(localeCode);
        }


        public string GetAccessToken(){
            Dictionary<string, string> values = new Dictionary<string, string>{
                { "grant_type", "client_credentials" }
            };
            FormUrlEncodedContent data = new FormUrlEncodedContent(values);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.secretKey}")) );
            HttpResponseMessage response = client.PostAsync("https://us.battle.net/oauth/token", data).Result;
            return JObject.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();

        }
        
        public List<Realm> GetRealms(){
            List<Realm> realms = new List<Realm>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}wow/realm/status?locale={this.locale.code}&access_token={this.accessToken}").Result;
            foreach(var result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["realms"].Children()){
                Realm realm = result.ToObject<Realm>();
                realms.Add(realm);
            }
            return realms;
        }

    }
}

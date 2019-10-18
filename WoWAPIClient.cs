using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI{
    public class WoWAPIClient : BlizzardAPIClient{
        public WoWAPIClient(string clientId, string secretKey, string localeCode) : base(clientId, secretKey, localeCode ){
            
        }

        public List<Realm> GetRealms(){
            List<Realm> realms = new List<Realm>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/connected-realm/index?namespace=dynamic-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["connected_realms"].Children()){
                realms.AddRange(this.GetConnectedRealms(result.GetValue("href").ToString())); //result.ToObject<Realm>();
                //realms.Add(realm);
            }
            return realms;
        }
        
        private List<Realm> GetConnectedRealms(string realmURL){
            List<Realm> realms = new List<Realm>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{realmURL}&locale={this.locale.code}&access_token={this.accessToken}").Result;
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            foreach(JObject result in data["realms"].Children()){

                Realm realm = new Realm();
                realm.name = result["name"].ToString();
                realm.hasQueue = (bool)data["has_queue"];
                realm.isTournament = (bool)result["is_tournament"];
                realm.type = result["type"]["name"].ToString();
                realm.slug = result["slug"].ToString();
                realms.Add(realm);

                
            }
            
            return realms;
        }

        public List<Race> GetRaces(){
            List<Race> races = new List<Race>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}wow/data/character/races?locale={this.locale.code}&access_token={this.accessToken}").Result;
            //races = JsonConvert.DeserializeObject<List<Race>>(response.Content.ReadAsStringAsync().Result);
            
            foreach(var result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["races"].Children()){
                Race race = result.ToObject<Race>();
                races.Add(race);
            }
            
            return races;
        }
    }
}
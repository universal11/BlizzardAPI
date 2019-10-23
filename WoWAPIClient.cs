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
                realms.AddRange(this.GetConnectedRealms(result.GetValue("href").ToString()));
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
                realm.id = Convert.ToInt32(result["id"]);
                realm.name = result["name"].ToString();
                realm.hasQueue = (bool)data["has_queue"];
                realm.isTournament = (bool)result["is_tournament"];
                realm.type = result["type"]["name"].ToString();
                realm.population = data["population"]["name"].ToString();
                realm.status = data["status"]["name"].ToString();
                realm.slug = result["slug"].ToString();
                realm.timezone = result["timezone"].ToString();
                realms.Add(realm);

                
            }
            
            return realms;
        }

        private Race GetRace(string raceURL){
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{raceURL}&locale={this.locale.code}&access_token={this.accessToken}").Result;
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Race race = new Race();
            race.id = Convert.ToInt32(data["id"]);
            race.name = data["name"].ToString();
            race.faction = data["faction"]["name"].ToString();
            race.isSelectable = (bool)data["is_selectable"];
            race.isAlliedRace = (bool)data["is_allied_race"];
            return race;
        }

        public List<Race> GetRaces(){
            List<Race> races = new List<Race>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/playable-race/index?namespace=static-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["races"].Children()){
                races.Add(this.GetRace(result["key"]["href"].ToString()));
            }
            return races;
        }

        /*
        public List<Race> GetRaces(){
            List<Race> races = new List<Race>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/playable-race/index?namespace=static-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            //races = JsonConvert.DeserializeObject<List<Race>>(response.Content.ReadAsStringAsync().Result);
            
            foreach(var result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["races"].Children()){
                Race race = result.ToObject<Race>();
                races.Add(race);
            }
            
            return races;
        }
        */
    }
}
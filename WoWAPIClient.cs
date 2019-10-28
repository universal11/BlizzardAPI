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

        private DateTime UnixTimeStampToDateTime( double timeStamp ){
            DateTime dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds( timeStamp ).ToLocalTime();
            return dateTime;
        }

        public List<Dungeon> GetDungeons(){
            List<Dungeon> dungeons = new List<Dungeon>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/mythic-keystone/dungeon/index?namespace=dynamic-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["dungeons"].Children()){
                dungeons.Add(this.GetDungeon(Convert.ToInt32(result["id"])));
            }
            return dungeons;
        }
        public Dungeon GetDungeon(int dungeonId){
            Dungeon dungeon = null;
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/mythic-keystone/dungeon/{dungeonId}?namespace=dynamic-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if(result != null){
                dungeon = new Dungeon();
                dungeon.id = Convert.ToInt32(result["id"]);
                dungeon.name = result["name"].ToString();
            }
            return dungeon;
        }
        public List<MythicSeason> GetMythicSeasons(){
            List<MythicSeason> seasons = new List<MythicSeason>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/mythic-keystone/season/index?namespace=dynamic-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["seasons"].Children()){
                string period = result["id"].ToString();
                System.Console.WriteLine($"Period ID: {period}");
                int mythicSeasonId = Convert.ToInt32(result["id"]);
                if(mythicSeasonId > 0){
                    seasons.Add(this.GetMythicSeason(mythicSeasonId));
                }
                
            }
            return seasons;
        }

        public MythicSeason GetMythicSeason(int mythicSeasonId){
            MythicSeason season = null;
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}data/wow/mythic-keystone/season/{mythicSeasonId}?namespace=dynamic-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if(result != null){
                season = new MythicSeason();
                season.id = Convert.ToInt32(result["id"]);
                season.startDate = this.UnixTimeStampToDateTime(Convert.ToDouble(result["start_timestamp"]));
                season.endDate = ( (result["end_timestamp"] != null) ? this.UnixTimeStampToDateTime(Convert.ToDouble(result["end_timestamp"])) : (DateTime?)null );
            }
            return season;
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

        public Character GetCharacterProfile(string realmSlug, string name){
            name = name.ToLower();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            HttpResponseMessage response = client.GetAsync($"{this.locale.host}profile/wow/character/{realmSlug}/{name}?namespace=profile-us&locale={this.locale.code}&access_token={this.accessToken}").Result;
            if(response.StatusCode != System.Net.HttpStatusCode.OK){
                return null;
            }
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Character character = new Character();
            character.id = Convert.ToInt32(data["id"]);
            character.name = data["name"].ToString();
            character.realmId = Convert.ToInt32(data["realm"]["id"]);
            return character;
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
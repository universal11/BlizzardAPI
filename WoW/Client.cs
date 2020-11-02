using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using BlizzardAPI.WoW;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI.WoW{
    public class Client : BlizzardAPI.Client{
        public Locale locale{get;set;}

        public Client(){

        }
        public Client(string clientId, string secretKey, string localeCode){
            this.Init(clientId, secretKey, localeCode);
        }

        public void Init(string clientId, string secretKey, string localeCode){
            this.clientId = clientId;
            this.secretKey = secretKey;
            this.locale = Locale.GetByCode(localeCode);
        }

        private DateTime UnixTimeStampToDateTime( double timeStamp ){
            DateTime dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds( timeStamp ).ToLocalTime();
            return dateTime;
        }

        public List<Dungeon> GetDungeons(){
            List<Dungeon> dungeons = new List<Dungeon>();
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/mythic-keystone/dungeon/index?namespace=dynamic-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["dungeons"].Children()){
                dungeons.Add(this.GetDungeon(Convert.ToInt32(result["id"])));
            }
            return dungeons;
        }
        public Dungeon GetDungeon(int dungeonId){
            Dungeon dungeon = null;
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/mythic-keystone/dungeon/{dungeonId}?namespace=dynamic-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
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
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/mythic-keystone/season/index?namespace=dynamic-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
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
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/mythic-keystone/season/{mythicSeasonId}?namespace=dynamic-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
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
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/connected-realm/index?namespace=dynamic-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["connected_realms"].Children()){
                realms.AddRange(this.GetConnectedRealms(result.GetValue("href").ToString()));
            }
            return realms;
        }
        
        private List<Realm> GetConnectedRealms(string realmURL){
            List<Realm> realms = new List<Realm>();
            HttpResponseMessage response = this.HttpGetWithAuth($"{realmURL}&locale={this.locale.code}&access_token={this.accessToken}");
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
            HttpResponseMessage response = this.HttpGetWithAuth($"{raceURL}&locale={this.locale.code}&access_token={this.accessToken}");
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
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/playable-race/index?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["races"].Children()){
                races.Add(this.GetRace(result["key"]["href"].ToString()));
            }
            return races;
        }
        

        public Character GetCharacterProfile(string realmSlug, string name){
            name = name.ToLower();
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}profile/wow/character/{realmSlug}/{name}?namespace=profile-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
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


        public List<Pet> GetPets(){
            List<Pet> pets = new List<Pet>();
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/pet/index?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["pets"].Children()){
                pets.Add( this.GetPetById(Convert.ToInt32(result["id"])) );
            }
            return pets;
        }

        public Pet GetPetById(int id){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/pet/{id}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            Pet pet = new Pet();
            pet.id = Convert.ToInt32(data["id"]);
            pet.name = data["name"].ToString();
            pet.description = data["description"].ToString();

            PetType type = new PetType();
            type.id = Convert.ToInt32(data["battle_pet_type"]["id"].ToString());
            type.type = data["battle_pet_type"]["type"].ToString();
            type.name = data["battle_pet_type"]["name"].ToString();
            pet.type = type;

            pet.isCapturable = (bool)data["is_capturable"];
            pet.isTradable = (bool)data["is_tradable"];
            pet.isBattlePet = (bool)data["is_battlepet"];
            pet.isAllianceOnly = (bool)data["is_alliance_only"];
            pet.isHordeOnly = (bool)data["is_horde_only"];

            if( data["abilities"] != null){
                foreach(JObject _ability in data["abilities"].Children() ){
                    PetAbility ability = this.GetPetAbilityById(Convert.ToInt32(_ability["ability"]["id"].ToString()));
                    ability.slot = Convert.ToInt32(_ability["slot"].ToString());
                    ability.requiredLevel = Convert.ToInt32(_ability["required_level"].ToString());
                    ability.assets = this.GetPetAbilityMediaAssetsByPetAbilityId(ability.id);
                    pet.abilities.Add(ability);
                }
            }
            


            return pet;
        }

        public PetAbility GetPetAbilityById(int id){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/pet-ability/{id}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            PetAbility ability = new PetAbility();
            ability.id = Convert.ToInt32(data["id"].ToString());
            ability.name = data["name"].ToString();

            PetType type = new PetType();
            type.id = Convert.ToInt32(data["battle_pet_type"]["id"].ToString());
            type.type = data["battle_pet_type"]["type"].ToString();
            type.name = data["battle_pet_type"]["name"].ToString();
            ability.type = type;
            ability.rounds = Convert.ToInt32(data["rounds"].ToString());

            return ability;
        }

        public List<PetAbilityMediaAsset> GetPetAbilityMediaAssetsByPetAbilityId(int petAbilityId){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/media/pet-ability/{petAbilityId}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            List<PetAbilityMediaAsset> assets = new List<PetAbilityMediaAsset>();

            foreach(JObject _asset in data["assets"].Children()){
                PetAbilityMediaAsset asset = new PetAbilityMediaAsset();
                asset.petAbilityId = Convert.ToInt32( data["id"].ToString() );
                asset.key = _asset["key"].ToString();
                asset.value = _asset["value"].ToString();
                asset.fileDataId = Convert.ToInt32( _asset["file_data_id"].ToString() );
            }

            return assets;
        }
    }
}
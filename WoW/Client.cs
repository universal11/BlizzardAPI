using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using BlizzardAPI.WoW;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlizzardAPI.WoW{
    public class Client : BlizzardAPI.Client{
        
        public Client(string clientId, string secretKey, string localeCode) : base(clientId, secretKey, localeCode){
            this.Init(clientId, secretKey, localeCode);
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
            //int i = 0;
            List<Pet> pets = new List<Pet>();
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/pet/index?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            foreach(JObject result in JObject.Parse(response.Content.ReadAsStringAsync().Result)["pets"].Children()){
                /*
                if(i == 10){
                    continue;
                }
                */
                pets.Add( this.GetPetById(Convert.ToInt32(result["id"])) );
                //i++;
                
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
            type.id = Convert.ToInt32(data["battle_pet_type"]["id"]);
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
                    PetAbility ability = this.GetPetAbilityById(Convert.ToInt32(_ability["ability"]["id"]));
                    ability.slot = Convert.ToInt32(_ability["slot"]);
                    ability.requiredLevel = Convert.ToInt32(_ability["required_level"]);
                    pet.abilities.Add(ability);
                }
            }
            
            if( data["source"] != null){
                PetSource source = new PetSource();
                source.name = data["source"]["name"].ToString();
                source.type = data["source"]["type"].ToString();
                pet.source = source;
            }
            
            pet.icon = data["icon"].ToString();

            Creature creature = this.GetCreatureById( Convert.ToInt32(data["creature"]["id"]) );
            pet.creature = creature;

            pet.isRandomCreatureDisplay = (bool)data["is_random_creature_display"];

            pet.media = new PetMedia();
            pet.media.petId = pet.id;
            pet.media.assets = this.GetPetMediaAssetsByPetId(pet.id);

            return pet;
        }

        public List<PetMediaAsset> GetPetMediaAssetsByPetId(int petId){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/media/pet/{petId}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            List<PetMediaAsset> assets = new List<PetMediaAsset>();
            if(data["assets"] != null){
                foreach(JObject _asset in data["assets"].Children()){
                    PetMediaAsset asset = new PetMediaAsset();
                    asset.petId = Convert.ToInt32( data["id"] );
                    asset.key = _asset["key"].ToString();
                    asset.value = _asset["value"].ToString();
                    asset.fileDataId = Convert.ToInt32( _asset["file_data_id"] );
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public Creature GetCreatureById(int id){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/creature/{id}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Creature creature = new Creature();
            creature.id = Convert.ToInt32(data["id"]);
            creature.name = data["name"].ToString();
            creature.type = this.GetCreatureTypeById( Convert.ToInt32(data["type"]["id"]) );
            creature.isTameable = (bool)data["is_tameable"];
            foreach(JObject record in data["creature_displays"].Children()){
                CreatureDisplay display = new CreatureDisplay();
                display.id = Convert.ToInt32( record["id"] );
                display.assets = this.GetCreatureDisplayMediaAssetsByCreatureDisplayId(display.id);
                creature.displays.Add(display);
            }
            return creature;
        }

        public CreatureType GetCreatureTypeById(int id){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/creature-type/{id}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            CreatureType creatureType = new CreatureType();
            creatureType.id = Convert.ToInt32(data["id"]);
            creatureType.name = data["name"].ToString();
            return creatureType;
        }

        public List<CreatureDisplayMediaAsset> GetCreatureDisplayMediaAssetsByCreatureDisplayId(int creatureDisplayId){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/media/creature-display/{creatureDisplayId}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            List<CreatureDisplayMediaAsset> assets = new List<CreatureDisplayMediaAsset>();
            foreach(JObject _asset in data["assets"].Children()){
                CreatureDisplayMediaAsset asset = new CreatureDisplayMediaAsset();
                asset.creatureDisplayId = Convert.ToInt32( data["id"] );
                asset.key = _asset["key"].ToString();
                asset.value = _asset["value"].ToString();
                assets.Add(asset);
            }

            return assets;
        }

        public PetAbility GetPetAbilityById(int id){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/pet-ability/{id}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            PetAbility ability = new PetAbility();
            ability.id = Convert.ToInt32(data["id"]);
            ability.name = data["name"].ToString();

            PetType type = new PetType();
            type.id = Convert.ToInt32(data["battle_pet_type"]["id"]);
            type.type = data["battle_pet_type"]["type"].ToString();
            type.name = data["battle_pet_type"]["name"].ToString();
            ability.type = type;

            ability.rounds = Convert.ToInt32(data["rounds"]);

            ability.media = new PetAbilityMedia();
            ability.media.id = Convert.ToInt32(data["media"]["id"]);
            ability.media.assets = this.GetPetAbilityMediaAssetsByPetAbilityId(ability.id);

            return ability;
        }

        public List<PetAbilityMediaAsset> GetPetAbilityMediaAssetsByPetAbilityId(int petAbilityId){
            HttpResponseMessage response = this.HttpGetWithAuth($"{this.locale.host}data/wow/media/pet-ability/{petAbilityId}?namespace=static-{this.locale.region}&locale={this.locale.code}&access_token={this.accessToken}");
            JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            List<PetAbilityMediaAsset> assets = new List<PetAbilityMediaAsset>();
            foreach(JObject _asset in data["assets"].Children()){
                PetAbilityMediaAsset asset = new PetAbilityMediaAsset();
                asset.petAbilityId = Convert.ToInt32( data["id"] );
                asset.key = _asset["key"].ToString();
                asset.value = _asset["value"].ToString();
                asset.fileDataId = Convert.ToInt32( _asset["file_data_id"] );
                assets.Add(asset);
            }

            return assets;
        }
    }
}
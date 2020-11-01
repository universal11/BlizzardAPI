using System;

namespace BlizzardAPI.WoW{
    public class Realm{
        public int id{get;set;}
        public string type{get;set;}
        public string population{get;set;}
        public bool hasQueue{get;set;}
        public string name{get;set;}
        public string slug{get;set;}
        public string battlegroup{get;set;}
        public string timezone{get;set;}
        public bool isTournament{get;set;}
        public string status{get;set;}

        public Realm(){

        }

        public void Init(int id, string type, string population, bool hasQueue, string name, string slug, string battlegroup, string timezone, string status){
            this.id = id;
            this.type = type;
            this.population = population;
            this.hasQueue = hasQueue;
            this.name = name;
            this.slug = slug;
            this.status = status;
            this.battlegroup = battlegroup;
            this.timezone = timezone;
        }
        
    }
}
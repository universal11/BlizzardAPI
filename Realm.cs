using System;

namespace BlizzardAPI{
    public class Realm{
        public string type{get;set;}
        public string population{get;set;}
        public bool hasQueue{get;set;}
        public string name{get;set;}
        public string slug{get;set;}
        public string battlegroup{get;set;}
        public string timezone{get;set;}
        public bool isTournament{get;set;}

        public Realm(){

        }

        public void Init(string type, string population, bool hasQueue, string name, string slug, string battlegroup, string timezone){
            this.type = type;
            this.population = population;
            this.hasQueue = hasQueue;
            this.name = name;
            this.slug = slug;
            this.battlegroup = battlegroup;
            this.timezone = timezone;
        }
    }
}
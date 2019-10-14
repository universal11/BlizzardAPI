using System;

namespace BlizzardAPI{
    public class Realm{
        public string type{get;set;}
        public string population{get;set;}
        public bool queue{get;set;}
        public string name{get;set;}
        public string slug{get;set;}
        public string battlegroup{get;set;}
        public string timezone{get;set;}

        public Realm(){

        }

        public void Init(string type, string population, bool queue, string name, string slug, string battlegroup, string timezone){
            this.type = type;
            this.population = population;
            this.queue = queue;
            this.name = name;
            this.slug = slug;
            this.battlegroup = battlegroup;
            this.timezone = timezone;
        }
    }
}
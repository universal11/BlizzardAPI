using System;

namespace BlizzardAPI{
    public class Race{
        public int id{get;set;}
        public string name{get;set;}
        public string faction{get;set;}
        public bool isSelectable{get;set;}
        public bool isAlliedRace{get;set;}
        public Race(){

        }

        public void Init(int id, string name, string faction, bool isSelectable, bool isAlliedRace){
            this.id = id;
            this.name = name;
            this.faction = faction;
            this.isSelectable = isSelectable;
            this.isAlliedRace = isAlliedRace;
        }

        
    }
}
using System;

namespace BlizzardAPI{
    public class Race{
        public int id{get;set;}
        public int mask{get;set;}
        public string side{get;set;}
        public string name{get;set;}
        public Race(){

        }

        public void Init(int id, int mask, string side, string name){
            this.id = id;
            this.mask = mask;
            this.side = side;
            this.name = name;
        }

        
    }
}
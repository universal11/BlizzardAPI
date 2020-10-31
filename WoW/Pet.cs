using System;

namespace BlizzardAPI.WoW{
    public class Pet{
        public int id{get;set;}
        public string name{get;set;}
        public Pet(){

        }

        public void Init(int id, string name){
            this.id = id;
            this.name = name;
        }

        
    }
}
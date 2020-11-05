using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class Creature{
        public int id{get;set;}
        public string name{get;set;}
        public CreatureType type{get;set;}
        public bool isTameable{get;set;}
        public List<CreatureDisplay> displays{get;set;} = new List<CreatureDisplay>();
        
        public Creature(){

        }
    }
}
using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class Pet{
        public int id{get;set;}
        public string name{get;set;}
        public string description{get;set;}
        public PetType type{get;set;}
        public bool isCapturable{get;set;}
        public bool isTradable{get;set;}
        public bool isBattlePet{get;set;}
        public bool isAllianceOnly{get;set;}
        public bool isHordeOnly{get;set;}
        public List<PetAbility> abilities{get;set;} = new List<PetAbility>();
        public PetSource source{get;set;}
        public string icon{get;set;}
        public Creature creature{get;set;}
        public bool isRandomCreatureDisplay{get;set;}
        public PetMedia media{get;set;}


        public Pet(){

        }

        
    }
}
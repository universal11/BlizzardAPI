using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class PetAbility{
        public int id{get;set;}
        public string name{get;set;}
        public int? slot{get;set;} = null;
        public int? requiredLevel{get;set;} = null;
        public int rounds{get;set;}
        public PetType type{get;set;}
        public PetAbilityMedia media{get;set;}

        public PetAbility(){

        }

    }
}
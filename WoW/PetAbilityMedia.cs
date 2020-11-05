using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class PetAbilityMedia{
        public int id{get;set;}
        public List<PetAbilityMediaAsset> assets{get;set;} = new List<PetAbilityMediaAsset>();
        public PetAbilityMedia(){

        }
    }
}
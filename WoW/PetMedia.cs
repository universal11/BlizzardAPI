using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class PetMedia{
        public int petId{get;set;}
        public List<PetMediaAsset> assets {get;set;} = new List<PetMediaAsset>();
        public PetMedia(){
            
        }
    }
}
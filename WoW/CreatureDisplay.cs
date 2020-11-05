using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class CreatureDisplay{
        public int id{get;set;}
        public List<CreatureDisplayMediaAsset> assets{get;set;} = new List<CreatureDisplayMediaAsset>();
        public CreatureDisplay(){

        }
    }
}
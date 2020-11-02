using System;

namespace BlizzardAPI.WoW{
    public class Realm{
        public int id{get;set;}
        public string type{get;set;}
        public string population{get;set;}
        public bool hasQueue{get;set;}
        public string name{get;set;}
        public string slug{get;set;}
        public string battlegroup{get;set;}
        public string timezone{get;set;}
        public bool isTournament{get;set;}
        public string status{get;set;}

        public Realm(){

        }
        
    }
}
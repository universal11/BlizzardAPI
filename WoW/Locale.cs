using System;
using System.Collections.Generic;

namespace BlizzardAPI.WoW{
    public class Locale{
        public string code{get;set;}
        public string region{get;set;}
        public string name{get;set;}
        public string host{get;set;}
        public Locale(){

        }

        public void Init(string code, string region, string name, string host){
            this.code = code;
            this.region = region;
            this.name = name;
            this.host = host;
        }

        public static Locale GetByCode(string code){
            foreach(Locale locale in Locale.GetAll()){
                if(locale.code == code){
                    return locale;
                }
            }
            return null;
        }

        public static List<Locale> GetAll(){
            List<Locale> locales = new List<Locale>();
            Locale locale = new Locale();
            locale.code = "en_US";
            locale.region = "US";
            locale.name = "English (US)";
            locale.host = "https://us.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "es_MX";
            locale.region = "US";
            locale.name = "Espanol (AL)";
            locale.host = "https://us.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "pt_BR";
            locale.region = "US";
            locale.name = "Portugues (AL)";
            locale.host = "https://us.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "en_GB";
            locale.region = "EU";
            locale.name = "English (EU)";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "es_ES";
            locale.region = "EU";
            locale.name = "Espanol (EU)";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "fr_FR";
            locale.region = "EU";
            locale.name = "French";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "ru_RU";
            locale.region = "EU";
            locale.name = "Russian";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "de_DE";
            locale.region = "EU";
            locale.name = "Deutsch";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "pt_PT";
            locale.region = "EU";
            locale.name = "PT?";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "it_IT";
            locale.region = "EU";
            locale.name = "Italiano";
            locale.host = "https://eu.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "ko_KR";
            locale.region = "KR";
            locale.name = "Korean";
            locale.host = "https://kr.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "zh_TW";
            locale.region = "TW";
            locale.name = "Taiwanese";
            locale.host = "https://tw.api.blizzard.com/";
            locales.Add(locale);

            locale = new Locale();
            locale.code = "zh_CN";
            locale.region = "CN";
            locale.name = "Chinese";
            locale.host = "https://gateway.battlenet.com.cn/";
            locales.Add(locale);

            return locales;
        }
    }

}
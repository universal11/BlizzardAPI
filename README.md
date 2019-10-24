# BlizzardAPI

### Authentication ###

```
string clientID = "08b3f3aa83259e9d0db3ca3911e4f180";
string secretKey = "41cc3fe37c66d3370c653b330837636d";
string locale = "en_US";
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.Authenticate();
```

### Get Races ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.Authenticate();
List<Race> races = client.GetRaces();
```


### Get Realms ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.Authenticate();
List<Realm> realms = client.GetRealms();
```

### Get Dungeons ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.Authenticate();
List<Dungeon> dungeons = client.GetDungeons();
```
### Get Mythic Seasons ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.Authenticate();
List<MythicSeason> seasons = client.GetMythicSeasons();
```



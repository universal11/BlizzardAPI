# BlizzardAPI

### Authentication ###

```
string clientID = "08b3f3aa83259e9d0db3ca3911e4f180";
string secretKey = "41cc3fe37c66d3370c653b330837636d";
string locale = "en_US";
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.accessToken = client.GetAccessToken();
```

### Get Races ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.accessToken = client.GetAccessToken();
List<Race> races = client.GetRaces()
```


### Get Realms ###
```
WoWAPIClient client = new WoWAPIClient(clientID, secretKey, locale);
client.accessToken = client.GetAccessToken();
List<Realm> realms = client.GetRealms()
```

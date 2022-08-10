using Yandex;


Console.WriteLine("Hello, World! input yandex login and pass...");
string login = "login@yandex.ru";
string pass = "password";
//string oauthToken = "AQAAAAAFqAjTAAG8XotGIt5YD09quyr6xCmTbnE";

login = Console.ReadLine();
pass = Console.ReadLine();


YandexMusicClient client = new YandexMusicClient();
var res = client.Authorize(login, pass);
//var res = client.Authorize(oauthToken);
var isAuth = client.IsAuthorized;
var token = client.Token;
var plPeronal = client.GetPersonalPlaylists();


var plFavs = client.GetFavorites();


//download
//var track3 = plWithTracks.Tracks[3];
//var trackMore = client.GetTrack(track3.Track.Id);
//var ff = client.GetDownInfo(trackMore.Id);
//var fdown = ff[0];
//var downP = client.TrackDownload(fdown);
//var urla = client.BuildUrl(track3.Track);

//names start
Console.WriteLine("---Favs---");
string names = "";
foreach (var playlist in plFavs)
{
    names = names + "<" + playlist.Title + "> " + playlist.TrackCount + Environment.NewLine;
}
Console.WriteLine(names);
names = "";
Console.WriteLine("---Personal---");
foreach (var playlist in plPeronal)
{
    names = names + "<" + playlist.Title + "> " + playlist.TrackCount + Environment.NewLine;
}
Console.WriteLine(names);
Console.WriteLine("---");
//names end

Console.WriteLine("input playlist name");
string plName = Console.ReadLine();
//этот плейлист без треков!
var myPl = plFavs.Where(pp => pp.Title == plName).First();
//а вот так вот он уже с треками!!! ура!!!
var plWithTracks = client.GetPlaylist(login, myPl.Kind);
var testCount = plWithTracks.Tracks.Count;

//tracks shuffle
List<Yandex.Music.Api.Models.Track.YTrack> tracks1 = new List<Yandex.Music.Api.Models.Track.YTrack>();
foreach (var track in plWithTracks.Tracks)
{
    tracks1.Add(track.Track);
}
var rnd = new Random();
var randomizedTracks = tracks1;
//var randomizedTracks = tracks1.OrderBy(item => rnd.Next()).ToList();
//Fisher-Yates shuffle algorithm 
int n = randomizedTracks.Count;
while (n > 1)
{
    n--;
    int k = rnd.Next(n + 1);
    var value = randomizedTracks[k];
    randomizedTracks[k] = randomizedTracks[n];
    randomizedTracks[n] = value;
}

//change in pl
//client.ChangePlaylistTracks(myPl, tracks1.ToArray() , randomizedTracks.ToArray() );
string name = plName+"_random_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
var res3 = client.CreateWithTracks(name, randomizedTracks.ToArray());

Console.WriteLine("SHUFFLED OK tracks="+res3.Tracks.Count);

Console.ReadLine();
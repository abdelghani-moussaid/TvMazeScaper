using TvMazeScaper.Services;

using HttpClient client = new();
var tvMazeService = new TvMazeService(client);

var shows = await tvMazeService.ProcessShowAsync();
if(shows != null) 
{
    foreach (var show in shows)
    {
        Console.WriteLine($"Show: {show.Name} (ID: {show.Id})");
        var castList = await tvMazeService.ProcessCastAsync(show.Id);
        if(castList != null) 
        {
            foreach (var cast in castList ?? Enumerable.Empty<Cast>())
            {
                Console.WriteLine($"  ID: {cast.Person.Id}");
                Console.WriteLine($"  Name: {cast.Person.Name}");
                Console.WriteLine($"  Birthday: {cast.Person.Birthday?.ToShortDateString()}\n");
            }
        }
    }
}


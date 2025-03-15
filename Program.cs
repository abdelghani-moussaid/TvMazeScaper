using System.Text.Json;
using HttpClient client = new();

var shows = await ProcessShowAsync(client);
if(shows != null) 
{
    foreach (var show in shows)
    {
        Console.WriteLine($"Show: {show.Name} (ID: {show.Id})");
        var castList = await ProcessCastAsync(client, show);
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

static async Task<List<Show>> ProcessShowAsync(HttpClient client)
{
    await using Stream showStream =
        await client.GetStreamAsync("https://api.tvmaze.com/shows");
    var shows =
        await JsonSerializer.DeserializeAsync<List<Show>>(showStream);
    return shows ?? new();
}



static async Task<List<Cast>> ProcessCastAsync(HttpClient client, Show show)
{
    await using Stream castStream =
            await client.GetStreamAsync($"https://api.tvmaze.com/shows/{show.Id}/cast");
        var castList =
            await JsonSerializer.DeserializeAsync<List<Cast>>(castStream);
        return castList ?? new();
}
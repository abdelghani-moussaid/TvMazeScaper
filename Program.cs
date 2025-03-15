using TvMazeScaper.Data;
using TvMazeScaper.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<TvMazeService>();
builder.Services.AddSqlite<TvMazeContext>("Data Source=TvMaze.db");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<TvMazeService>();
    
    try
    {
        // Fetch and save the shows, then get the list of all shows
        var shows = await service.PersistShowsAsync();
        
        var testShows = shows.Take(10);
        // Iterate through the shows and save the cast for each one
        foreach (var show in testShows)
        {
            Console.WriteLine($"Fetching cast for show with ID: {show.Id}");
            await service.PersistCastAsync(show.Id);

            // Add a small delay between each request to be safe
            await Task.Delay(500); // 500ms delay between requests
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

app.Run();


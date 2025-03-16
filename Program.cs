using Microsoft.EntityFrameworkCore;
using TvMazeScaper.Data;
using TvMazeScaper.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<TvMazeService>();
builder.Services.AddSqlite<TvMazeContext>("Data Source=TvMaze.db");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TvMazeScaper API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

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

app.MapGet("/shows", async (TvMazeContext db) =>
{
    var shows = await db.Show
        .Include(s => s.Cast)
        .ThenInclude(c => c.Person)
        .Select(s => new
        {
            s.Id,
            s.Name,
            Cast = s.Cast
                .OrderByDescending(c => c.Person.Birthday)
                .Select(c => new
                {
                    c.Person.Id,
                    c.Person.Name,
                    Birthday = c.Person.Birthday.HasValue
                        ? c.Person.Birthday.Value.ToString("yyyy-MM-dd") // Format date
                        : null // Handle null birthdays
                }).ToList()
        })
        .ToListAsync();

    return Results.Ok(shows);
});

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Message = "Welcome to the TV Maze Scraper API!",
        Instructions = "Use /shows to fetch all shows with their cast information."
    });
});

app.Run();


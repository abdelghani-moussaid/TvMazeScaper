using System.Text.Json;
using TvMazeScaper.Data;

namespace TvMazeScaper.Services
{
    public class TvMazeService
    {
        private readonly HttpClient _httpClient;
        private readonly TvMazeContext _context;

        public TvMazeService(HttpClient httpClient, TvMazeContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<List<Show>> ProcessShowAsync()
        {
            await using Stream showStream =
                await _httpClient.GetStreamAsync("https://api.tvmaze.com/shows");
            var shows =
                await JsonSerializer.DeserializeAsync<List<Show>>(showStream);
            return shows ?? new();
        }

        public async Task<List<Cast>> ProcessCastAsync(int showId)
        {
            await using Stream castStream =
                    await _httpClient.GetStreamAsync($"https://api.tvmaze.com/shows/{showId}/cast");
                var castList =
                    await JsonSerializer.DeserializeAsync<List<Cast>>(castStream);
                return castList ?? new();
        }

        public async Task<List<Show>> PersistShowsAsync()
        {
            var shows = await ProcessShowAsync();

            // Avoid Duplicate Entries
            foreach (var show in shows)
            {
                if (!_context.Show.Any(s => s.Id == show.Id)) 
                {
                    _context.Show.Add(show);
                }
            }
            await _context.SaveChangesAsync();

            return shows;
        }

        public async Task PersistCastAsync(int showId)
        {
            var castList = await ProcessCastAsync(showId);

            foreach (var cast in castList)
            {
                // Check if the Person already exists in the context or database
                var existingPerson = _context.Person.Local.FirstOrDefault(p => p.Id == cast.Person.Id)
                                    ?? _context.Person.SingleOrDefault(p => p.Id == cast.Person.Id);

                if (existingPerson != null)
                {
                    // Use the tracked or database Person instance
                    Console.WriteLine($"Person exists, using existing Person: {existingPerson.Id}, {existingPerson.Name}");
                    cast.Person = existingPerson;
                }
                else
                {
                    // Add new Person to the context
                    Console.WriteLine($"Adding new Person: {cast.Person.Id}, {cast.Person.Name}");
                    _context.Person.Add(cast.Person);
                }

                // Check if the Cast already exists
                if (!_context.Cast.Any(c => c.Id == cast.Id))
                {
                    Console.WriteLine($"Adding new Cast: {cast.Id}");
                    _context.Cast.Add(cast);
                }
            }

            await _context.SaveChangesAsync();
        }


    }

}
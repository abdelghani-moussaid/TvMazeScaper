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

            // Avoid duplicate entries by checking if the show already exists
            foreach (var show in shows)
            {
                if (!_context.Show.Any(s => s.Id == show.Id)) 
                {
                    _context.Show.Add(show);
                    await _context.SaveChangesAsync();
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
                // Check if the Person already exists
                var existingPerson = _context.Person.Local.FirstOrDefault(p => p.Id == cast.Person.Id)
                                    ?? _context.Person.SingleOrDefault(p => p.Id == cast.Person.Id);

                if (existingPerson != null)
                {
                    cast.Person = existingPerson; // Use existing Person instance
                }
                else
                {
                    _context.Person.Add(cast.Person); // Add new Person
                }

                // Check if the Cast already exists for the same ShowId and PersonId
                if (!_context.Cast.Any(c => c.ShowId == showId && c.PersonId == cast.Person.Id))
                {
                    cast.ShowId = showId; // Ensure ShowId is set
                    _context.Cast.Add(cast); // Add new Cast
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
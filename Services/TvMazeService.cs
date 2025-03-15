using System.Text.Json;

namespace TvMazeScaper.Services
{
    public class TvMazeService
    {
        private readonly HttpClient _httpClient;

        public TvMazeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }

}
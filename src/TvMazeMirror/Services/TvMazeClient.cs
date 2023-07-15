using System.Text.Json;

namespace TvMazeMirror.Services;

public class TvMazeClient : ITvMazeClient {
    private readonly HttpClient httpClient;

    public TvMazeClient(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<ShowDto>?> GetShows(int page) {
        var response = await httpClient.GetAsync($"shows?page={page}");

        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) {
            return null;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
            return Enumerable.Empty<ShowDto>();
        }

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<List<ShowDto>>(await response.Content.ReadAsStringAsync());
    }
}

namespace TvMazeMirror.Services;

public interface ITvMazeClient {
    public Task<IEnumerable<ShowDto>?> GetShows(int page);
}

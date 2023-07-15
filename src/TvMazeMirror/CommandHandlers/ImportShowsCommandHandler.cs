using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Services;

namespace TvMazeMirror.CommandHandlers;

public class ImportShowsCommandHandler : IImportShowsCommandHandler {
    private const int showIdsPerPage = 250;

    private readonly ITvMazeClient client;
    private readonly ITvMazeContext context;
    private readonly IUnitOfWork unitOfWork;

    public ImportShowsCommandHandler(ITvMazeClient client, ITvMazeContext context, IUnitOfWork unitOfWork) {
        this.client = client;
        this.context = context;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ValueCommandResult> Execute() {
        var result = new ValueCommandResult();
        var page = context.GetHighestTvMazeId() / showIdsPerPage;
        var shows = await client.GetShows(page);

        if (shows != null) {
            foreach (var dto in shows) {
                var show = new Show(dto.Name ?? "<unknown>") {
                    TvMazeId = dto.Id,
                    Language = dto.Language,
                    Premiered = dto.Premiered,
                    Summary = dto.Summary,
                    Genres = dto.Genres.Select(genre => new ShowGenre(genre)).ToList()
                };

                context.Shows.AddRange(show);
            }
            await unitOfWork.SaveChangesAsync();

            result.Value = shows.Count();
        }

        return result;
    }
}

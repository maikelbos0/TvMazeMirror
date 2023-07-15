using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Services;

namespace TvMazeMirror.CommandHandlers;

public class ImportShowsCommandHandler : IImportShowsCommandHandler {
    private const int showIdsPerPage = 250;
    private readonly static DateTime premieredFrom = new(2014, 1, 1);

    private readonly ITvMazeClient client;
    private readonly ITvMazeContext context;
    private readonly IUnitOfWork unitOfWork;

    public ImportShowsCommandHandler(ITvMazeClient client, ITvMazeContext context, IUnitOfWork unitOfWork) {
        this.client = client;
        this.context = context;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ImportShowsCommandResult> Execute(int page) {
        var result = new ImportShowsCommandResult();
        var highestTvMazeId = context.GetHighestTvMazeId();

        page = Math.Max(page, highestTvMazeId / showIdsPerPage);
        result.NextPage = page + 1;

        var showDtos = await client.GetShows(page);

        if (showDtos == null) {
            result.IsRateLimited = true;
        }
        if (showDtos != null) {
            var newShowDtos = showDtos.Where(dto => dto.Id > highestTvMazeId && dto.Premiered > premieredFrom);

            foreach (var newShowDto in newShowDtos) {
                var show = new Show(newShowDto.Name ?? "<unknown>") {
                    TvMazeId = newShowDto.Id,
                    Language = newShowDto.Language,
                    Premiered = newShowDto.Premiered,
                    Summary = newShowDto.Summary,
                    Genres = newShowDto.Genres.Select(genre => new ShowGenre(genre)).ToList()
                };

                context.Shows.Add(show);
            }

            await unitOfWork.SaveChangesAsync();

            result.Imported = newShowDtos.Count();
            result.Downloaded = showDtos.Count();
        }

        return result;
    }
}

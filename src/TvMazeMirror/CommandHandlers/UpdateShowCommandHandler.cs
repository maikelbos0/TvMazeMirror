using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public class UpdateShowCommandHandler : IUpdateShowCommandHandler {
    private readonly ITvMazeContext context;
    private readonly IUnitOfWork unitOfWork;

    public UpdateShowCommandHandler(ITvMazeContext context, IUnitOfWork unitOfWork) {
        this.context = context;
        this.unitOfWork = unitOfWork;
    }

    public async Task<LookupCommandResult> Execute(ShowModel model) {
        var commandResult = new LookupCommandResult();

        if (string.IsNullOrWhiteSpace(model.Name)) {
            commandResult.AddError(nameof(ShowModel.Name), "Name is required");
        }

        if (commandResult.IsValid) {
            var show = context.FindShow(model.Id);

            if (show != null) {
                commandResult.IsFound = true;

                show.Name = model.Name!;
                show.Language = model.Language;
                show.Premiered = model.Premiered;
                show.Summary = model.Summary;

                foreach (var genre in show.Genres.Where(genre => !model.Genres.Contains(genre.Name)).ToList()) {
                    show.Genres.Remove(genre);
                    context.ShowGenres.Remove(genre);
                }

                foreach (var name in model.Genres.Where(name => !show.Genres.Any(genre => genre.Name == name)).ToList()) {
                    show.Genres.Add(new ShowGenre(name));
                }

                context.Shows.Update(show);
                await unitOfWork.SaveChangesAsync();
            };
        }

        return commandResult;
    }
}
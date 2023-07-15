using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public class AddShowCommandHandler : IAddShowCommandHandler {
    private readonly ITvMazeContext context;
    private readonly IUnitOfWork unitOfWork;

    public AddShowCommandHandler(ITvMazeContext context, IUnitOfWork unitOfWork) {
        this.context = context;
        this.unitOfWork = unitOfWork;
    }

    public async Task<AddShowCommandResult> Execute(ShowModel model) {
        var commandResult = new AddShowCommandResult();

        if (string.IsNullOrWhiteSpace(model.Name)) {
            commandResult.AddError(nameof(ShowModel.Name), "Name is required");
        }

        if (commandResult.IsValid) {
            var show = new Show(model.Name!) {
                Language = model.Language,
                Premiered = model.Premiered,
                Summary = model.Summary,
                Genres = model.Genres.Select(genre => new ShowGenre(genre)).ToList()
            };

            context.Shows.Add(show);
            await unitOfWork.SaveChangesAsync();

            commandResult.Id = show.Id;
        }

        return commandResult;
    }
}

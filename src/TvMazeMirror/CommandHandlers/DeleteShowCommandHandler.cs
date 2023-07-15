using TvMazeMirror.Database;

namespace TvMazeMirror.CommandHandlers;

public class DeleteShowCommandHandler : IDeleteShowCommandHandler {
    private readonly ITvMazeContext context;
    private readonly IUnitOfWork unitOfWork;

    public DeleteShowCommandHandler(ITvMazeContext context, IUnitOfWork unitOfWork) {
        this.context = context;
        this.unitOfWork = unitOfWork;
    }

    public async Task<LookupCommandResult> Execute(int id) {
        var commandResult = new LookupCommandResult();

        var show = context.FindShow(id);

        if (show != null) {
            commandResult.IsFound = true;

            context.Shows.Remove(show);
            await unitOfWork.SaveChangesAsync();
        }

        return commandResult;
    }
}
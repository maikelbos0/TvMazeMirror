using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public interface IAddShowCommandHandler {
    Task<CommandResult> Execute(ShowModel model);
}
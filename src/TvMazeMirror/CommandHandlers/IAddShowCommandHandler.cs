using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public interface IAddShowCommandHandler {
    Task<AddShowCommandResult> Execute(ShowModel model);
}
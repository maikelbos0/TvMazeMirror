using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public interface IAddShowCommandHandler {
    Task<ValueCommandResult> Execute(ShowModel model);
}
using TvMazeMirror.Models;

namespace TvMazeMirror.CommandHandlers;

public interface IUpdateShowCommandHandler {
    Task<LookupCommandResult> Execute(ShowModel model);
}
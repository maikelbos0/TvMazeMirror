namespace TvMazeMirror.CommandHandlers;

public interface IDeleteShowCommandHandler {
    Task<LookupCommandResult> Execute(int id);
}
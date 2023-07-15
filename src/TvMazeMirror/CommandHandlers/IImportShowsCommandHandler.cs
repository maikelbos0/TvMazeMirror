namespace TvMazeMirror.CommandHandlers; 

public interface IImportShowsCommandHandler {
    Task<ValueCommandResult> Execute();
}
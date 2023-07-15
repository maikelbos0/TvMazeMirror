namespace TvMazeMirror.CommandHandlers;

public interface IImportShowsCommandHandler {
    Task<ImportShowsCommandResult> Execute(int page);
}

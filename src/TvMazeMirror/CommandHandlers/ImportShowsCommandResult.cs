namespace TvMazeMirror.CommandHandlers;

public class ImportShowsCommandResult : CommandResult {
    public int Downloaded { get; set; }
    public int Imported { get; set; }
    public int NextPage { get; set; }
    public bool IsRateLimited { get; set; }
}
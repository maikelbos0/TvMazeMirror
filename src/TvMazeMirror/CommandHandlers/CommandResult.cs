namespace TvMazeMirror.CommandHandlers;

public class CommandResult {
    public int? Value { get; set; }
    public List<CommandError> Errors { get; set; } = new();
    public bool IsSuccesful => !Errors.Any();

    public void AddError(string property, string message)
        => Errors.Add(new(property, message));
}

namespace TvMazeMirror.CommandHandlers;

public class CommandResult {
    public List<CommandError> Errors { get; set; } = new();
    public bool IsValid => !Errors.Any();

    public void AddError(string property, string message)
        => Errors.Add(new(property, message));
}

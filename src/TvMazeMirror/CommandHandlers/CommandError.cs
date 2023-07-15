namespace TvMazeMirror.CommandHandlers;

public class CommandError {
    public string Property { get; }
    public string Message { get; }

    public CommandError(string property, string message) {
        Property = property;
        Message = message;
    }
}

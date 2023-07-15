namespace TvMazeMirror.Services;

public class ShowDto {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Language { get; set; }
    public DateTime? Premiered { get; set; }
    public List<string> Genres { get; set; } = new List<string>();
    public string? Summary { get; set; }
}
namespace TvMazeMirror.Entities;

// TODO language and genre could be normalized
public class Show {
    public int Id { get; set; }
    public int? TvMazeId { get; set; }
    public string Name { get; set; }        
    public string? Language { get; set; }
    public DateTime? Premiered { get; set; }        
    public ICollection<ShowGenre> Genres { get; set; } = new List<ShowGenre>();
    public string? Summary { get; set; }

    public Show(string name) {
        Name = name;
    }
}

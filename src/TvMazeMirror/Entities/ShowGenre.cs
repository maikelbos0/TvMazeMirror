namespace TvMazeMirror.Entities;

public class ShowGenre {
    public int Id { get; set; }
    public string Name { get; set; }

    public ShowGenre(string name) {
        Name = name;
    }
}

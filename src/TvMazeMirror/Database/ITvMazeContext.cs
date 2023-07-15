using Microsoft.EntityFrameworkCore;
using TvMazeMirror.Entities;

namespace TvMazeMirror.Database;

public interface ITvMazeContext {
    DbSet<Show> Shows { get; }
    DbSet<ShowGenre> ShowGenres { get; }

    Show? FindShow(int id);
}

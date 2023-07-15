using Microsoft.EntityFrameworkCore;
using TvMazeMirror.Entities;

namespace TvMazeMirror.Database;

public interface ITvMazeContext {
    DbSet<Show> Shows { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
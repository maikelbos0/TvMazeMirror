using Microsoft.EntityFrameworkCore;
using TvMazeMirror.Entities;

namespace TvMazeMirror.Database;

public class TvMazeContext : DbContext, ITvMazeContext, IUnitOfWork {
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<ShowGenre> ShowGenres => Set<ShowGenre>();

    public TvMazeContext(AppSettings appSettings)
        : base(new DbContextOptionsBuilder<TvMazeContext>().UseSqlServer(appSettings.DatabaseConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Show>(builder => {
            builder.HasMany(show => show.Genres).WithOne().IsRequired();

            builder.Property(show => show.Name).IsRequired();
        });

        modelBuilder.Entity<ShowGenre>(builder => {
            builder.Property(genre => genre.Name).IsRequired();
        });
    }

    public Show? FindShow(int id) => Shows.Include(show => show.Genres).SingleOrDefault(show => show.Id == id);

    public int GetHighestTvMazeId() => Shows.OrderByDescending(show => show.TvMazeId).FirstOrDefault()?.TvMazeId ?? 0;
}

using Microsoft.EntityFrameworkCore;
using TvMazeMirror.Entities;

namespace TvMazeMirror.Database;

public class TvMazeContext : DbContext, ITvMazeContext, IUnitOfWork {
    public DbSet<Show> Shows => Set<Show>();

    public TvMazeContext(AppSettings appSettings)
        : base(new DbContextOptionsBuilder<TvMazeContext>().UseSqlServer(appSettings.DatabaseConnectionString).Options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Show>(builder => {
            builder.HasMany(show => show.Genres).WithOne();

            builder.Property(show => show.Name).IsRequired();

            builder.ToTable(nameof(Shows));
        });
    }
}

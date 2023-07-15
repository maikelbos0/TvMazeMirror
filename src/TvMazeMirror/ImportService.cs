using TvMazeMirror.CommandHandlers;

namespace TvMazeMirror;

public class ImportService : BackgroundService {
    private const int rateLimitDelayInSeconds = 10;

    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<ImportService> logger;

    public ImportService(IServiceScopeFactory serviceScopeFactory, ILogger<ImportService> logger) {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken) {
        Task.Run(() => RunScheduledTasks(cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }

    private async Task RunScheduledTasks(CancellationToken cancellationToken) {
        int? importedShows = null;

        while (!(cancellationToken.IsCancellationRequested || importedShows == 0)) {
            try {
                using var scope = serviceScopeFactory.CreateScope();
                var importShowsCommandHandler = scope.ServiceProvider.GetRequiredService<IImportShowsCommandHandler>();

                importedShows = (await importShowsCommandHandler.Execute()).Value;

                if (importedShows == null) {
                    logger.LogInformation("Rate limit reached; waiting for {RateLimitDelayInSeconds} seconds", rateLimitDelayInSeconds);
                    await Task.Delay(rateLimitDelayInSeconds * 1000, cancellationToken);
                }
                else {
                    logger.LogInformation("Imported {ShowCount} shows", importedShows);
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to import shows");
            }
        }

        logger.LogInformation("Finished importing shows");
    }
}
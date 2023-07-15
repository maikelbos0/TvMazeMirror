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
        int page = 0;

        while (!cancellationToken.IsCancellationRequested) {
            try {
                using var scope = serviceScopeFactory.CreateScope();

                var importShowsCommandHandler = scope.ServiceProvider.GetRequiredService<IImportShowsCommandHandler>();
                var result = await importShowsCommandHandler.Execute(page);

                page = result.NextPage;

                if (result.IsRateLimited) {
                    logger.LogInformation("Rate limit reached; waiting for {RateLimitDelayInSeconds} seconds", rateLimitDelayInSeconds);
                    await Task.Delay(rateLimitDelayInSeconds * 1000, cancellationToken);
                }
                else {
                    logger.LogInformation("Imported {ShowCount} shows", result.Imported);
                }

                if (result.Downloaded == 0) {
                    logger.LogInformation("Finished importing shows");
                    return;
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to import shows");
            }
        }
    }
}
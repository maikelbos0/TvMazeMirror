using TvMazeMirror;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;
using TvMazeMirror.Services;

var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? throw new InvalidOperationException($"Missing object '{nameof(AppSettings)}' in configuration");

if (builder.Environment.IsDevelopment()) {
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ITvMazeClient, TvMazeClient>(client => client.BaseAddress = new Uri(appSettings.ApiBaseUri));
builder.Services.AddDbContext<TvMazeContext>(ServiceLifetime.Scoped);
builder.Services.AddScoped<ITvMazeContext, TvMazeContext>(provider => provider.GetRequiredService<TvMazeContext>());
builder.Services.AddScoped<IUnitOfWork, TvMazeContext>(provider => provider.GetRequiredService<TvMazeContext>());
builder.Services.AddSingleton(appSettings);
builder.Services.AddScoped<IAddShowCommandHandler, AddShowCommandHandler>();
builder.Services.AddScoped<IUpdateShowCommandHandler, UpdateShowCommandHandler>();
builder.Services.AddScoped<IImportShowsCommandHandler, ImportShowsCommandHandler>();
builder.Services.AddScoped<IDeleteShowCommandHandler, DeleteShowCommandHandler>();
builder.Services.AddHostedService<ImportService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

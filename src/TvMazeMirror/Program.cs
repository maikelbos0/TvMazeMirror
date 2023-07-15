

using TvMazeMirror;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TvMazeContext>(ServiceLifetime.Scoped);
builder.Services.AddScoped<ITvMazeContext, TvMazeContext>(provider => provider.GetRequiredService<TvMazeContext>());
builder.Services.AddScoped<IUnitOfWork, TvMazeContext>(provider => provider.GetRequiredService<TvMazeContext>());
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? throw new InvalidOperationException($"Missing object '{nameof(AppSettings)}' in configuration"));
builder.Services.AddScoped<IAddShowCommandHandler, AddShowCommandHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();



using TvMazeMirror;
using TvMazeMirror.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ITvMazeContext, TvMazeContext>();
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? throw new InvalidOperationException($"Missing object '{nameof(AppSettings)}' in configuration"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

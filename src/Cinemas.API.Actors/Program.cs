using Cinemas.API.Actors.Apis;
using Cinemas.API.Actors.Extensions;
using Cinemas.API.Actors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddCors();

builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseCors(b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.MapDefaultEndpoints();
app.MapActorApi();

await Task.Delay(TimeSpan.FromSeconds(15));
if (app.Environment.IsDevelopment())
    app.Services.CreateScope().ServiceProvider.GetService<ActorContext>()!.Database.EnsureCreated();

app.Run();

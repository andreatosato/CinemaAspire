using Cinemas.API.Films.Apis;
using Cinemas.API.Films.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddProblemDetails();
var app = builder.Build();

app.MapDefaultEndpoints();
app.MapFilmApi();

app.Run();

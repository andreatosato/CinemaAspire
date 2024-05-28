using Cinemas.API.Films.Apis;
using Cinemas.API.Films.Entities;
using Cinemas.API.Films.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddCors();

builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseCors(b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.MapDefaultEndpoints();
app.MapFilmApi();

if(app.Environment.IsDevelopment())
    app.Services.CreateScope().ServiceProvider.GetService<FilmContext>()!.Database.EnsureCreated();

app.Run();

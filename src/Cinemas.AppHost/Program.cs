using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Storage
var storage = builder.AddAzureStorage("cinemasaspire")
                     .AddBlobs("films");
// DB
var cache = builder.AddRedis("cache");
var sqlFilm = builder.AddSqlServer("cinemaaspire").AddDatabase("films");
var cosmosActors = builder.AddAzureCosmosDB("cinemaaspire").AddDatabase("actors");

// Bus
var rabbitMq = builder.AddRabbitMQ("eventbus-cinema")
    .WithImage("rabbitmq")
    .WithImageTag("3.13.2-management");

// Services
var apiFilms = builder.AddProject<Projects.Cinemas_API_Films>("films")
    .WithReference(sqlFilm)
    .WithReference(cache)
    .WithReference(storage)
    .WithReference(rabbitMq);

var apiActors = builder.AddProject<Projects.Cinemas_API_Actors>("actors")
    .WithReference(cosmosActors)
    .WithReference(cache)
    .WithReference(rabbitMq);

// Frontend
var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

builder.AddProject<Projects.Cinemas_Web>("webfrontend")
    .WithExternalHttpEndpoints()    
    .WithReference(apiFilms)
    .WithReference(apiActors)
    .WithEnvironment("Film__Url", apiFilms.GetEndpoint(launchProfileName))
    .WithEnvironment("Actor__Url", apiActors.GetEndpoint(launchProfileName));

builder.Build().Run();


// For test use only.
// Looks for an environment variable that forces the use of HTTP for all the endpoints. We
// are doing this for ease of running the Playwright tests in CI.
static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}

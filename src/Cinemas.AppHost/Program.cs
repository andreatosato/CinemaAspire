using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var secrets = builder.AddConnectionString("secrets");

// Storage
var storage = builder.AddAzureStorage("cinemas-aspire-storage")
    .RunAsEmulator()
    .AddBlobs("films-blob");

// DB
var cache = builder.AddRedis("cinemas-aspire-cache");
var sqlFilm = builder.AddSqlServer("cinema-aspire-db").AddDatabase("filmsdb");
var cosmosActors = builder.AddAzureCosmosDB("cinemas-aspire-cosmos")
    .RunAsEmulator()
    .AddDatabase("actorsdb");

// Bus
var rabbitMq = builder.AddRabbitMQ("cinemas-aspire-bus");
    //.WithImage("rabbitmq", "3.13.2-management");

//var rabbitMq = builder.AddRabbitMQ("cinemas-aspire-bus");

// Services
var apiFilms = builder.AddProject<Projects.Cinemas_API_Films>("filmsApi")
    .WithReference(sqlFilm)
    .WithReference(cache)
    .WithReference(storage)
    .WithReference(rabbitMq);
//.WithReference(secrets);

var apiActors = builder.AddProject<Projects.Cinemas_API_Actors>("actorsApi")
    .WithReference(cosmosActors)
    .WithReference(cache)
    .WithReference(rabbitMq);
    //.WithReference(secrets);

// Frontend
var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

builder.AddProject<Projects.Cinemas_Web>("webfrontend")
    .WithExternalHttpEndpoints()    
    .WithReference(apiFilms)
    .WithReference(apiActors)
    .WithReference(cache)
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

//cinemas
//Cinema-Password!
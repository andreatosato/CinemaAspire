using Cinemas.API.Actors.Infrastructure;
using Cinemas.API.Actors.IntegrationEvents.EventHandling;
using Cinemas.EventBus.Events;
using System.Text.Json.Serialization;

namespace Cinemas.API.Actors.Extensions;
public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddCosmosDbContext<ActorContext>(null, "actorsdb");

        builder.AddRabbitMqEventBus("cinemas-aspire-bus")
               .AddSubscription<FilmCreatedEvent, FilmCreatedEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));

        //builder.Services.AddScoped<IOmdbClient>(sp => new OmdbClient(builder.Configuration.GetConnectionString("Omdb")));
        //builder.AddAzureBlobClient("films-blob");
    }
}

[JsonSerializable(typeof(FilmCreatedEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{
}
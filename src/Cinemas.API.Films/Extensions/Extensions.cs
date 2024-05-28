using Cinemas.API.Films.Entities;
using Cinemas.API.Films.IntegrationEvents.EventHandling;
using Cinemas.API.Films.IntegrationEvents.Events;
using OMDbApiNet;
using System.Text.Json.Serialization;

namespace Cinemas.API.Films.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<FilmContext>("filmsdb");

        builder.AddRabbitMqEventBus("eventbus-cinema")
               .AddSubscription<FilmCreatedEvent, FilmCreatedEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
       
        builder.Services.AddScoped<IOmdbClient>(sp => new OmdbClient(builder.Configuration.GetConnectionString("Omdb")));
        builder.AddAzureBlobClient("films-blob");

    }
}

[JsonSerializable(typeof(FilmCreatedEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{
}
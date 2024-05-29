using Cinemas.API.Actors.Infrastructure;
using Cinemas.API.Actors.IntegrationEvents.EventHandling;
using Cinemas.EventBus.Events;
using GoogleApi.Extensions;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Cinemas.API.Actors.Extensions;
public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddCosmosDbContext<ActorContext>(builder.Configuration.GetConnectionString("actorsdb")!, "actorsdb");

        builder.AddRabbitMqEventBus("cinemas-aspire-bus")
               .AddSubscription<ActorLoadedEvent, ActorLoadedEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));

        builder.Services.AddGoogleApiClients();
    }
}

[JsonSerializable(typeof(FilmCreatedEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{
}
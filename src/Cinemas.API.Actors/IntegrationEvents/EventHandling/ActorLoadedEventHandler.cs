using Cinemas.EventBus.Abstractions;
using Cinemas.EventBus.Events;
using Google.Protobuf.WellKnownTypes;
using GoogleApi;
using GoogleApi.Entities.Search.Image.Request;
using GoogleApi.Entities.Search.Common;
using GoogleApi.Entities.Search.Common.Enums;
using GoogleApi.Interfaces.Places;
using GoogleApi.Interfaces.Search;

namespace Cinemas.API.Actors.IntegrationEvents.EventHandling;

public class ActorLoadedEventHandler : IIntegrationEventHandler<ActorLoadedEvent>
{
    private readonly IImageSearchApi googleApi;

    public ActorLoadedEventHandler(IImageSearchApi googleApi)
    {
        this.googleApi = googleApi;
    }
    public async Task Handle(ActorLoadedEvent @event)
    {
        foreach (var actor in @event.Actors)
        {
            var response = await googleApi.QueryAsync(new ImageSearchRequest
            {
                ImageOptions = new SearchImageOptions
                {
                    ImageType = ImageType.Photo
                },
                Query = actor
            });

        }        
        Console.WriteLine(@event.Id);
    }
}

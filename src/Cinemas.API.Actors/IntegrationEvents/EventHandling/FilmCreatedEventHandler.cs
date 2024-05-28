using Cinemas.EventBus.Abstractions;
using Cinemas.EventBus.Events;

namespace Cinemas.API.Actors.IntegrationEvents.EventHandling;

public class FilmCreatedEventHandler : IIntegrationEventHandler<FilmCreatedEvent>
{
    public async Task Handle(FilmCreatedEvent @event)
    {
        Console.WriteLine(@event.Id);
    }
}

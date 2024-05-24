using Cinemas.EventBus.Events;

namespace Cinemas.API.Films.IntegrationEvents.Events;

public record FilmCreatedEvent(Guid FilmId) : IntegrationEvent;

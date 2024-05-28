using System;

namespace Cinemas.EventBus.Events;
public record FilmCreatedEvent(Guid FilmId) : IntegrationEvent;

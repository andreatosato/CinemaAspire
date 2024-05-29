using System;
using System.Collections.Generic;

namespace Cinemas.EventBus.Events;

public record ActorLoadedEvent(Guid FilmId, List<string> Actors) : IntegrationEvent;
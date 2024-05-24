using System.Threading.Tasks;

namespace Cinemas.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}

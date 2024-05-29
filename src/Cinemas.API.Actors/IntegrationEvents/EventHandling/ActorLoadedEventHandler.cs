using Cinemas.EventBus.Abstractions;
using Cinemas.EventBus.Events;
using GoogleApi;
using GoogleApi.Entities.Search.Image.Request;
using Cinemas.API.Actors.Infrastructure;

namespace Cinemas.API.Actors.IntegrationEvents.EventHandling;

public class ActorLoadedEventHandler : IIntegrationEventHandler<ActorLoadedEvent>
{
    private readonly ActorContext db;

    public ActorLoadedEventHandler(ActorContext db)
    {
        this.db = db;
    }

    public async Task Handle(ActorLoadedEvent @event)
    {
        foreach (var actor in @event.Actors)
        {
            var actorEntity = await db.Actors.FindAsync(actor);
            if (actorEntity == null)
            {
                actorEntity = new Models.ActorEntity(actor)
                {
                    FilmIds = new() { new Models.FilmEntity() { FilmId = @event.FilmId } }
                };
                db.Actors.Add(actorEntity);
            }
            try
            {
                var response = await GoogleSearch.ImageSearch.QueryAsync(new ImageSearchRequest
                {
                    Key = "AIzaSyBZ24-mhL7GA_kfKGIcIxBscpzh5hsaOBY",
                    SearchEngineId = "760a83375a7614d8b",
                    Query = actor
                });

                var actorImageResponse = response.Items.Where(t => t.MimeType == "image/jpeg").FirstOrDefault();
                if (actorImageResponse != null)
                {
                    actorEntity.Picture = actorImageResponse.FormattedUrl;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            await db.SaveChangesAsync();
        }        
        Console.WriteLine(@event.Id);
    }
}

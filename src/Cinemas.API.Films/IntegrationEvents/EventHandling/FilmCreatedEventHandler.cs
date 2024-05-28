using Cinemas.API.Films.Entities;
using Cinemas.API.Films.IntegrationEvents.Events;
using Cinemas.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using OMDbApiNet;

namespace Cinemas.API.Films.IntegrationEvents.EventHandling;

public class FilmCreatedEventHandler : IIntegrationEventHandler<FilmCreatedEvent>
{
    private readonly FilmContext db;
    private readonly IOmdbClient omdb;

    public FilmCreatedEventHandler(FilmContext db, IOmdbClient omdb)
    {
        this.db = db;
        this.omdb = omdb;
    }

    public async Task Handle(FilmCreatedEvent @event)
    {
        var film = await db.Films.FindAsync(@event.FilmId);
        var omdbItem = omdb.GetItemByTitle(film!.Name);

        // TODO: Copy image to Azure Storage
        // TODO: Save Azure Storage Link
        film.PictureUri = omdbItem.Poster;
        await db.SaveChangesAsync();
    }
}

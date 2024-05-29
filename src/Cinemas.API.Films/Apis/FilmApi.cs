using Cinemas.API.Films.Entities;
using Cinemas.API.Films.Models;
using Cinemas.EventBus.Abstractions;
using Cinemas.EventBus.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinemas.API.Films.Apis;


public static class CatalogApi
{
    public static IEndpointRouteBuilder MapFilmApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/film");

        // Routes for querying catalog items.
        api.MapGet("/items", GetAllItems);
        api.MapPost("/items", CreateItem);
        return app;
    }

    public static async Task<Results<Ok<List<FilmEntity>>, BadRequest<string>>> GetAllItems(
        [FromServices] FilmContext db)
    {
        var itemsOnPage = await db.Films
            .OrderBy(c => c.Name)
            .ToListAsync();

        return TypedResults.Ok(itemsOnPage);
    }

    public static async Task<Results<Ok<FilmEntity>, NotFound, BadRequest<string>>> GetItemById(
        [FromServices] FilmContext db,
        Guid id)
    {
        if (Guid.Empty == id)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await db.Films.SingleOrDefaultAsync(ci => ci.Id == id);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }


    public static async Task<Created> CreateItem(
        [FromServices] FilmContext db,
        [FromServices] IEventBus bus,
        [FromBody] FilmEntity filmEntity)
    {
        if (db.Films.Where(t => t.Name == filmEntity.Name).Any())
        {
            filmEntity = await db.Films.FirstAsync(t => t.Name == filmEntity.Name);
        }
        else
        {
            db.Films.Add(filmEntity);
            await db.SaveChangesAsync();
        }
        await bus.PublishAsync(new FilmCreatedEvent(filmEntity.Id));
        return TypedResults.Created($"/api/catalog/items/{filmEntity.Id}");
    }

}

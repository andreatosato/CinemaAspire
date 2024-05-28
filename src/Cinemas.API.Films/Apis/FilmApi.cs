using Cinemas.API.Films.Entities;
using Cinemas.API.Films.IntegrationEvents.Events;
using Cinemas.API.Films.Models;
using Cinemas.EventBus.Abstractions;
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
        //api.MapGet("/items", GetAllItems);
        //api.MapGet("/items/{id:int}", GetItemById);
        
        // Routes for modifying catalog items.
        api.MapPut("/items", UpdateItem);
        api.MapPost("/items", CreateItem);
        api.MapDelete("/items/{id:int}", DeleteItemById);

        return app;
    }

    public static async Task<Results<Ok<PaginatedItems<FilmEntity>>, BadRequest<string>>> GetAllItems(
        [FromQuery] PaginationRequest paginationRequest,
        [FromServices] FilmContext db)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await db.Films
            .LongCountAsync();

        var itemsOnPage = await db.Films
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<FilmEntity>(pageIndex, pageSize, totalItems, itemsOnPage));
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

    public static async Task<Results<Created, NotFound<string>>> UpdateItem(
        [FromServices] FilmContext db,
        FilmEntity filmToUpdate)
    {
        var filmItem = await db.Films.SingleOrDefaultAsync(i => i.Id == filmToUpdate.Id);

        if (filmItem == null)
        {
            return TypedResults.NotFound($"Item with id {filmToUpdate.Id} not found.");
        }

        // Update current product
        var filmEntry = db.Entry(filmToUpdate);
        filmEntry.CurrentValues.SetValues(filmToUpdate);

        return TypedResults.Created($"/api/catalog/items/{filmToUpdate.Id}");
    }

    public static async Task<Created> CreateItem(
        [FromServices] FilmContext db,
        [FromServices] IEventBus bus,
        [FromBody] FilmEntity filmEntity)
    {
        db.Films.Add(filmEntity);
        await db.SaveChangesAsync();
        await bus.PublishAsync(new FilmCreatedEvent(filmEntity.Id));
        return TypedResults.Created($"/api/catalog/items/{filmEntity.Id}");
    }

    public static async Task<Results<NoContent, NotFound>> DeleteItemById(
        [FromServices] FilmContext db,
        Guid id)
    {
        var item = db.Films.SingleOrDefault(x => x.Id == id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        db.Films.Remove(item);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}

using Cinemas.API.Actors.Infrastructure;
using Cinemas.API.Actors.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinemas.API.Actors.Apis;

public static class ActorApi
{
    public static IEndpointRouteBuilder MapActorApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/actor");
        api.MapGet("/items/{id:guid}", GetItemById);

        return app;
    }

    public static async Task<Results<Ok<ActorEntity[]>, NotFound, BadRequest<string>>> GetItemById(
        [FromServices] ActorContext db,
        Guid id)
    {
        if (Guid.Empty == id)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await db.Actors.ToListAsync();
        var actors = item.Where(t => t.FilmIds.Any(c => c.FilmId == id));

        if (actors == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(actors.ToArray());
    }

}

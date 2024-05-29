namespace Cinemas.Web;

public class ActorApiClient(HttpClient httpClient)
{
    public async Task<ActorEntity[]> GetActorByFilm(Guid filmId, CancellationToken cancellationToken = default)
    {
        var actors = await httpClient.GetFromJsonAsync<List<ActorEntity>>($"/api/actor/items/{filmId}", cancellationToken);
        return actors?.ToArray() ?? [];
    }
}

public record ActorEntity(string Name, string Picture, List<FilmEntity> FilmIds);
public record FilmEntity(Guid Id)
{
    public string Name { get; set; }
    public string? PictureUri { get; set; }
    public ActorEntity[] Actors { get; set; } = Array.Empty<ActorEntity>();
}
using System.Net.Http;
using System.Text.Json;

namespace Cinemas.Web;

public class FilmApiClient(HttpClient httpClient)
{
    public async Task PostFilm(string filmTitle, CancellationToken cancellationToken = default)
    {
        var entity = new FilmRequestEntity() { Name = filmTitle };
        await httpClient.PostAsJsonAsync<FilmRequestEntity>("/api/film/items", entity, JsonSerializerOptions.Default);
    }

    public async Task<List<FilmEntity>> GetFilms(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<FilmEntity>>("/api/film/items", cancellationToken);
    }

    public class FilmRequestEntity
    {
        public required string Name { get; set; }
    }

}

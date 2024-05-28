using System.Net.Http;
using System.Text.Json;

namespace Cinemas.Web;

public class FilmApiClient(HttpClient httpClient)
{
    public async Task PostFilm(string filmTitle, CancellationToken cancellationToken = default)
    {
        var entity = new FilmEntity() { Name = filmTitle };
        await httpClient.PostAsJsonAsync<FilmEntity>("/api/film/items", entity, JsonSerializerOptions.Default);
    }

    public class FilmEntity
    {
        public required string Name { get; set; }
    }

}

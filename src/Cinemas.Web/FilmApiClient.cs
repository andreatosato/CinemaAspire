using System.Net.Http;

namespace Cinemas.Web;

public class FilmApiClient(HttpClient httpClient)
{
    public async Task PostFilm(string filmTitle, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync<FilmEntity>("/api/film/items", new FilmEntity() { Name = filmTitle });
    }

    public class FilmEntity
    {
        public required string Name { get; set; }
    }

}

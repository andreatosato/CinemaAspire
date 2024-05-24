namespace Cinemas.API.Films.Models;

public class FilmEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset PublishDate { get; set; }
    public string? PictureUri { get; set; }
}

namespace Cinemas.API.Actors.Models;

public class ActorEntity
{
    public ActorEntity(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public string Picture { get; set; }
    public List<Guid> FilmIds { get; set; } = new List<Guid>();
}

using Microsoft.EntityFrameworkCore;

namespace Cinemas.API.Actors.Infrastructure;

public class ActorContext : DbContext
{
    public ActorContext()
    {
    }

    public ActorContext(DbContextOptions<ActorContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

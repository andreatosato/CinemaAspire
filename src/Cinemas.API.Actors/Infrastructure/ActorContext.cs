using Cinemas.API.Actors.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Cinemas.API.Actors.Infrastructure;

public class ActorContext : DbContext
{
    public ActorContext()
    {
    }

    public ActorContext(DbContextOptions<ActorContext> options) : base(options) { }

    public DbSet<ActorEntity> Actors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

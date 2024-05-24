using Cinemas.API.Films.Infrastructure.EntityConfigurations;
using Cinemas.API.Films.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Cinemas.API.Films.Entities;

public class FilmContext : DbContext
{
    public DbSet<FilmEntity> Films { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

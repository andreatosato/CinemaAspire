using Cinemas.API.Films.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemas.API.Films.Infrastructure.EntityConfigurations;

public class FilmEntityConfiguration : IEntityTypeConfiguration<FilmEntity>
{
    public void Configure(EntityTypeBuilder<FilmEntity> builder)
    {
        builder.ToTable("Films");
        builder.HasKey(x => x.Id);
    }
}

using Cinemas.API.Actors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemas.API.Actors.Infrastructure.EntityConfigurations
{
    public class ActorEntityConfiguration : IEntityTypeConfiguration<ActorEntity>
    {
        public void Configure(EntityTypeBuilder<ActorEntity> builder)
        {
            builder.HasKey(t => t.Name);

        }
    }
}

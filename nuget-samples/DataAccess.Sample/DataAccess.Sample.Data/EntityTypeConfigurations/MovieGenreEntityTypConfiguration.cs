using DataAccess.Sample.Domain.Entities;
using DataAccess.Sample.Domain.Statics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Sample.Data.EntityTypeConfigurations
{
    public class MovieGenreEntityTypConfiguration : IEntityTypeConfiguration<MovieGenre>
    {
        public void Configure(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.HasKey(pk => new { pk.MovieId, pk.Genre });

            builder.Property(p => p.Genre).HasColumnName("GenreId");

            builder.ToTable(nameof(MovieGenre), SchemaNames.Public);    
        }
    }
}

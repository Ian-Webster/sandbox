using DataAccess.Sample.Domain.Entities;
using DataAccess.Sample.Domain.Statics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Sample.Data.EntityTypeConfigurations;

public class MovieEntityTypeConfiguration: IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(pk => pk.MovieId);

        builder.ToTable(nameof(Movie), SchemaNames.Public);
    }
}
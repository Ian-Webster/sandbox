using DataAccess.Sample.Domain.Enums;

namespace DataAccess.Sample.Domain.Entities;

public class Movie
{
    public Guid MovieId { get; set; }

    public string Name { get; set; }

    public List<MovieGenre>? MovieGenres { get; set; }
}
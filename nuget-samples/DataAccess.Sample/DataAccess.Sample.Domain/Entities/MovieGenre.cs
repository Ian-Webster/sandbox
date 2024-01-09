using DataAccess.Sample.Domain.Enums;

namespace DataAccess.Sample.Domain.Entities
{
    public class MovieGenre
    {
        public Guid MovieId { get; set; }

        public Genres Genre { get; set; }

        public required Movie Movie { get; set; }

    }
}

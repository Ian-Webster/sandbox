using DataAccess.Sample.Domain.Entities;

namespace DataAccess.Sample.Data.Repositories;

public interface IMovieRepository
{
    Task<Movie?> GetMovieById(Guid movieId, CancellationToken token);

    Task<IEnumerable<Movie>?> GetAllMovies(CancellationToken token);

    Task<bool> AddMovie(Movie movie, CancellationToken token);

    Task<bool> UpdateMovie(Movie movie, CancellationToken token);

    Task<bool> RemoveMovie(Guid movieId, CancellationToken token);
}
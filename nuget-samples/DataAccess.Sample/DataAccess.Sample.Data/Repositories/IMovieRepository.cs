using DataAccess.Sample.Domain.Entities;
using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;

namespace DataAccess.Sample.Data.Repositories;

public interface IMovieRepository
{
    Task<Movie?> GetMovieById(Guid movieId, CancellationToken token);

    Task<Movie?> GetMovieForGraphQuery(IResolverContext context, CancellationToken token);

    Task<IEnumerable<Movie>?> GetAllMovies(CancellationToken token);

    Task<IEnumerable<Movie>?> GetMoviesForGraphQuery(IResolverContext context, CancellationToken token);

    Task<Connection<Movie>> GetMoviesPaginatedForGraphQuery(IResolverContext context, CancellationToken token);

    Task<bool> AddMovie(Movie movie, CancellationToken token);

    Task<bool> UpdateMovie(Movie movie, CancellationToken token);

    Task<bool> RemoveMovie(Guid movieId, CancellationToken token);
}
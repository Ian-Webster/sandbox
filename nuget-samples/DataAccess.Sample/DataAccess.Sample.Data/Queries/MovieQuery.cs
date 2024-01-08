using DataAccess.Sample.Data.Repositories;
using DataAccess.Sample.Domain.Entities;
using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;

namespace DataAccess.Sample.Data.Queries;

[ExtendObjectType("Query")]
public class MovieQuery //Query 
{
    [UseProjection]
    [UseFiltering]
    public async Task<Movie?> GetMovie([Service] IMovieRepository repository, IResolverContext context,
        CancellationToken token)
    {
        return await repository.GetMovieForGraphQuery(context, token);
    }
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Movie>?> GetMovies([Service] IMovieRepository repository, IResolverContext context,
        CancellationToken token)
    {
        return await repository.GetMoviesForGraphQuery(context, token);
    }
    /*
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<Connection<Movie>> GetPaginatedMovies([Service] IMovieRepository repository, IResolverContext context,
        CancellationToken token)
    {
        return await repository.GetMoviesPaginatedForGraphQuery(context, token);
    }*/
}
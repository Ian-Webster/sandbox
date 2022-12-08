using DataAccess.Repository;
using DataAccess.Sample.Data.DatabaseContexts;
using DataAccess.Sample.Domain.Entities;

namespace DataAccess.Sample.Data.Repositories;

public class MovieRepository: IMovieRepository
{
    private readonly IRepository<Movie> _movieRepository;

    public MovieRepository(RepositoryFactory<MovieContext> repositoryFactory)
    {
        _movieRepository = repositoryFactory.GetRepositoryByType<Movie>();
    }

    public async Task<Movie?> GetMovieById(Guid movieId, CancellationToken token)
    {
        return await _movieRepository.FirstOrDefault(m => m.MovieId == movieId, token);
    }

    public async Task<IEnumerable<Movie>?> GetAllMovies(CancellationToken token)
    {
        return await _movieRepository.List(m => true, token);
    }

    public async Task<bool> AddMovie(Movie movie, CancellationToken token)
    {
        if (await _movieRepository.Exists(m => m.MovieId == movie.MovieId, token)) return false;

        return await _movieRepository.Add(movie, token);
    }

    public async Task<bool> UpdateMovie(Movie movie, CancellationToken token)
    {
        var movieToUpdate = await _movieRepository.FirstOrDefault(m => m.MovieId == movie.MovieId, token);

        if (movieToUpdate == null) return false;

        movieToUpdate.Name = movie.Name;

        return await _movieRepository.Update(movieToUpdate, token);
    }

    public async Task<bool> RemoveMovie(Guid movieId, CancellationToken token)
    {
        var movieToRemove = await _movieRepository.FirstOrDefault(m => m.MovieId == movieId, token);

        if (movieToRemove == null) return false;

        return await _movieRepository.Remove(movieToRemove, token);
    }
}
using DataAccess.Sample.Data.Repositories;
using DataAccess.Sample.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAccess.Sample.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll(CancellationToken token)
        {
            var result = await _movieRepository.GetAllMovies(token);

            return result != null ? Ok(result) : NotFound("No movies found");
        }

        [HttpGet("{movieId:guid}")]
        public async Task<ActionResult<Movie>> Get(Guid movieId, CancellationToken token)
        {
            var result = await _movieRepository.GetMovieById(movieId, token);

            return result != null ? Ok(result) : NotFound($"Movie with id {movieId}, not found");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie movie, CancellationToken token)
        {
            var result = await _movieRepository.AddMovie(movie, token);

            return result ? Ok(result) : UnprocessableEntity($"Failed to add new movie with id {movie.MovieId}");
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Movie movie, CancellationToken token)
        {
            var result = await _movieRepository.UpdateMovie(movie, token);

            return result ? Ok(result) : UnprocessableEntity($"Failed to add new movie with id {movie.MovieId}");
        }

        [HttpDelete("{movieId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid movieId, CancellationToken token)
        {
            var result = await _movieRepository.RemoveMovie(movieId, token);

            return result ? Ok(result) : UnprocessableEntity($"Failed to remove movie with id {movieId}");
        }
    }
}

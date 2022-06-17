using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DataContext _Context;

        public MovieController(DataContext dataContext)
        {
            _Context = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieLibrary>>> Get()
        {
            return Ok(await _Context.Movies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieLibrary>> Get(int id)
        {
            var movie = await _Context.Movies.FindAsync(id);
            if (movie == null)
                return BadRequest("Movie not found");
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<List<MovieLibrary>>> AddMovie(MovieLibrary movie)
        {
            _Context.Movies.Add(movie);
            await _Context.SaveChangesAsync();
            return Ok(await _Context.Movies.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<MovieLibrary>>> UpdateMovie(MovieLibrary request)
        {
            var dbmovie = await _Context.Movies.FindAsync(request.Id);
            if (dbmovie == null)
                return BadRequest("Movie not found");

            dbmovie.Title = request.Title;
            dbmovie.release_year = request.release_year;

            await _Context.SaveChangesAsync();

            return Ok(await _Context.Movies.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<MovieLibrary>>> Delete(int id)
        {
            var dbmovie = await _Context.Movies.FindAsync(id);
            if (dbmovie == null)
                return BadRequest("Movie not found");
            _Context.Movies.Remove(dbmovie);
            await _Context.SaveChangesAsync();

            return Ok(await _Context.Movies.ToListAsync());
        }
    }
}

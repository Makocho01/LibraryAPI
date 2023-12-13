using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/Genres")]
    [ApiController]
    public class GenresController : Controller
    {
        public readonly AppDbContext _dbContext;
        
        public GenresController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult GetGenres() 
        {
            return Ok(_dbContext.Genres.ToList());
        }

        // get id author
        [HttpGet("{id}")]
        public IActionResult GetGenre([FromRoute] Guid id)
        {
            var genre = _dbContext.Genres.FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }


        [HttpPost]
        public ActionResult CreateGenre(GenreDto genre) 
        {
            var newGenre = new Genre()
            {
                Name = genre.Name,
                Description = genre.Description
            };

            _dbContext.Genres.Add(newGenre);
            _dbContext.SaveChanges();

            return Ok(_dbContext.Genres.FirstOrDefault(g => g.Name == genre.Name && g.Description == genre.Description));
        }


        [HttpPut("{id}")] //put = edit
        public IActionResult EditMember([FromRoute] Guid id, GenreDto genre)
        {
            var originalGenre = _dbContext.Genres.FirstOrDefault(r => r.Id == id);
            if (originalGenre == null)
            {
                return NotFound();
            }


            originalGenre.Name = genre.Name;
            originalGenre.Description = genre.Description;
            

            _dbContext.SaveChanges();
            return Ok(_dbContext.Genres.FirstOrDefault(r => r.Id == id));
        }


        // delete id genere
        [HttpDelete("{id}")]
        public IActionResult DeleteGenre([FromRoute] Guid id)
        {
            var genre = _dbContext.Genres.FirstOrDefault(e => e.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            _dbContext.Genres.Remove(genre);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}

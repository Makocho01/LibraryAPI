using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/Authors")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AuthorsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAuthors() 
        {
            return Ok(_dbContext.Authors.ToList());
        }

        // get id author
        [HttpGet("{id}")]
        public IActionResult GetAuthor([FromRoute] Guid id)
        {
            var author = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor(AuthorDto author) 
        {
            var newAuthor = new Author()
            {
                Name = author.Name,
                Surname = author.Surname
            };

            _dbContext.Authors.Add(newAuthor);
            _dbContext.SaveChanges();

            return Ok(_dbContext.Authors.FirstOrDefault(a => a.Name == author.Name && a.Surname == author.Surname));
        }


        [HttpPut("{id}")] 
        public IActionResult EditBook([FromRoute] Guid id, AuthorDto author)
        {
            var originalAuthor = _dbContext.Authors.FirstOrDefault(r => r.Id == id);
            if (originalAuthor == null)
            {
                return NotFound();
            }


            originalAuthor.Name = author.Name;
            originalAuthor.Surname = author.Surname;
            
            _dbContext.SaveChanges();
            return Ok(_dbContext.Authors.FirstOrDefault(r => r.Id == id));
        }


        
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor([FromRoute] Guid id)
        {
            var author = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}

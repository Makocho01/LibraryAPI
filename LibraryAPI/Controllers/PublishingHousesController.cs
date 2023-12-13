using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/PublishingHouses")]
    [ApiController]
    public class PublishingHousesController : Controller
    {
        private readonly AppDbContext _dbContext;

        public PublishingHousesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetPublishingHouses()
        {
            return Ok(_dbContext.PublishingHouses.ToList());
        }

        //get id house
        [HttpGet("{id}")]
        public IActionResult GetPublishingHouse([FromRoute] Guid id)
        {
            var publishinghouse = _dbContext.PublishingHouses.FirstOrDefault(p => p.Id == id);
            if (publishinghouse == null)
            {
                return NotFound();
            }
            return Ok(publishinghouse);
        }

        //put tu zrob i delete wszedzie (get po id w reszcie)

        [HttpPost]
        public IActionResult CreatePublishingHouse(PublishingHouseDto publishingHouse)
        {
            var newPublishingHouse = new PublishingHouse()
            {
                Name = publishingHouse.Name,
                FoundationYear = publishingHouse.FoundationYear,
                Address = publishingHouse.Address,
                Website = publishingHouse.Website
            };

            _dbContext.PublishingHouses.Add(newPublishingHouse);
            _dbContext.SaveChanges();

            return Ok(_dbContext.PublishingHouses.FirstOrDefault(p => p.Name == publishingHouse.Name));
        }




        [HttpPut("{id}")] //put = edit
        public IActionResult EditPublishingHouse([FromRoute] Guid id, PublishingHouseDto publishinghouse)
        {
            var originalPublishingHouse = _dbContext.PublishingHouses.FirstOrDefault(r => r.Id == id);
            if (originalPublishingHouse == null)
            {
                return NotFound();
            }








         

            originalPublishingHouse.Name = publishinghouse.Name;
            originalPublishingHouse.FoundationYear = publishinghouse.FoundationYear;
            originalPublishingHouse.Address = publishinghouse.Address;
            originalPublishingHouse.Website = publishinghouse.Website;

            _dbContext.SaveChanges();
            return Ok(_dbContext.PublishingHouses.FirstOrDefault(r => r.Id == id));
        }




        // delete id publish house
        [HttpDelete("{id}")]
        public IActionResult DeletePublishingHouse([FromRoute] Guid id)
        {
            var publishinghouse = _dbContext.PublishingHouses.FirstOrDefault(e => e.Id == id);
            if (publishinghouse == null)
            {
                return NotFound();
            }

            _dbContext.PublishingHouses.Remove(publishinghouse);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}

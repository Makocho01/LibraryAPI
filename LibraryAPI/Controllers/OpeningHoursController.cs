using Contentful.Core;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/OpeningHours")]
    [ApiController]
    public class OpeningHoursController : Controller
    {
        private readonly IContentfulClient _contentfulClient;
        
        public OpeningHoursController(IContentfulClient contentfulClient) 
        {
            _contentfulClient = contentfulClient;
        }

        [HttpGet]
        public async Task <IActionResult> GetOpeningHours()
        {
            var hours = await _contentfulClient.GetEntries<LibrarySystem>();
            return Ok(hours);
        }
    }
}

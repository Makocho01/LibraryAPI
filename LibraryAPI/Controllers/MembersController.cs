using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{

    [Route("api/Members")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly AppDbContext _dbContext;
        

        public MembersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetMembers() 
        {
            var members = _dbContext.Members
                .Include(x => x.Rents)
                .ToList();

            return Ok(members);
        }

        [HttpGet("{id}")]
        public IActionResult GetMember([FromRoute] Guid id)
        {
            var member = _dbContext.Members
                .Include(x => x.Rents)
                .FirstOrDefault(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpPost]
        public IActionResult CreateMember(MemberDto member)
        {
            var rents = _dbContext.Rents.Where(r => member.Rents.Contains(r.Id)).ToList();

            var newMember = new Member()
            {
                Name = member.Name,
                Surname = member.Surname,
                Birthdate = member.Birthdate,
                Address = member.Address,
                PhoneNumber = member.PhoneNumber,
                Email = member.Email,
                Rents = rents
            };

            _dbContext.Members.Add(newMember);
            _dbContext.SaveChanges();

            return Ok(_dbContext.Members.FirstOrDefault(m => m.Surname == member.Surname && m.Email == member.Email));
        }

        [HttpPut("{id}")] //put = edit
        public IActionResult EditMember([FromRoute] Guid id, MemberDto member)
        {
            var originalMember = _dbContext.Members.FirstOrDefault(r => r.Id == id);
            if (originalMember == null)
            {
                return NotFound();
            }

            var rents = _dbContext.Rents.Where(r => member.Rents.Contains(r.Id)).ToList();

            originalMember.Name = member.Name;
            originalMember.Surname = member.Surname;
            originalMember.Birthdate = member.Birthdate;
            originalMember.Address = member.Address;
            originalMember.PhoneNumber = member.PhoneNumber;
            originalMember.Email = member.Email;
            originalMember.Rents = rents;

            _dbContext.SaveChanges();
            return Ok(_dbContext.Members.FirstOrDefault(r => r.Id == id));
        }


        // delete id member
        [HttpDelete("{id}")]
        public IActionResult DeleteMember([FromRoute] Guid id)
        {
            var member = _dbContext.Members.FirstOrDefault(e => e.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            _dbContext.Members.Remove(member);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}


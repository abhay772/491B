using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMTOGO.WebAPP.Data;
using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UsersDbContext usersDbContext;

        public UsersController(UsersDbContext usersDbContext)
        {
            this.usersDbContext = usersDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await usersDbContext.Users.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetUserById")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            await usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            var user = await usersDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("{email}")]
        [ActionName("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            await usersDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            var user = await usersDbContext.Users.FindAsync(email);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(Users user)
        {
            user.Id = Guid.NewGuid();
            await usersDbContext.Users.AddAsync(user);
            await usersDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] Users updateUser)
        {
            var user = await usersDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = updateUser.Password;
            user.Email = updateUser.Email;

            await usersDbContext.SaveChangesAsync();

            return Ok(user);
        }
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await usersDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            usersDbContext.Users.Remove(user);
            await usersDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

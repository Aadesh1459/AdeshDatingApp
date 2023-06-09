using AdeshDatingApp.Data;
using AdeshDatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdeshDatingApp.Controllers
{
    [ApiController]
    [Route("adeshdatingapp/[Controller]")]  // GET  adeshdatingapp/users
    [Authorize]

    public class UsersController : BaseAPIController
    {
        private readonly DataContext c;

        public UsersController(DataContext c) => this.c = c;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUSers()
        {
            var users = await c.Users.ToListAsync();
            
            return users;
        }

        [HttpGet("{id}")]
        public async  Task<ActionResult<AppUser>> GetUSer(int id)
        {
            return await c.Users.FindAsync(id);
        }

    }
}
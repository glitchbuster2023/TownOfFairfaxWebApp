using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Town_of_Fairfax.Data.Context;

namespace Town_of_Fairfax.Data
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("api/auth/getusers")]
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet]
        [Route("api/auth/getuserbyusername")]
        public async Task<User> GetUser(string username)
        {
            var user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();

            if(user == null)
            {
                return null!;
            }else
            {
                return user;
            }
        }

        [HttpPost]
        [Route("api/auth/register")]

        public async Task<ActionResult<User>> AddUser(User user)
        {
            var userObject = await _context.Users.Where(u => u.Username == user.Username).FirstOrDefaultAsync();

            if(userObject is not null)
            {
                return BadRequest("Username is already taken");
            }else
            {
                await _context.Users.AddAsync(userObject!);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        } 

    }
}

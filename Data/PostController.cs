using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Town_of_Fairfax.Data.Context;

namespace Town_of_Fairfax.Data
{

    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly ApplicationContext _context;

        public PostController(ApplicationContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("api/posts/getposts")]
        public async Task<List<Post>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }


        [HttpGet]
        [Route("api/posts/getpostsbydepartment")]

        public async Task<List<Post>> GetPostsByDepartment(string department)
        {
            var posts = await _context.Posts.ToListAsync();
            List<Post> byDept = posts.Where(p => p.Department == department).ToList();
            return byDept;
        }

        [HttpPost]
        [Route("api/posts/createpost")]
        public async Task<HttpResponseMessage> NewPost([FromBody] Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
            {
                Content = new StringContent(post.Id + "," + post.Title + ", " + post.Content)
            };

            return response;
        }

        [HttpDelete]
        [Route("api/posts/removepost")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                _context.Posts.Remove(_context.Posts.First(p => p.Id == id));
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status202Accepted, "Post deleted succesfully");
            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting post");
            }
        }

    }
}
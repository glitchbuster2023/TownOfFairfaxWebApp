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

    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Town_of_Fairfax.Data;

namespace Town_of_Fairfax.Security
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private static readonly AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = false
        };

        [HttpGet]
        [Route("api/auth/test")]
        public string Test()
        {
            return "test";
        }

        

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult> SignIn(Credential cred)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, cred.Username),
                                new Claim(ClaimTypes.Role, cred.Role),
                                new Claim("userId", cred.Id)
                            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = COOKIE_EXPIRES;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return this.Ok();


        }

        [HttpGet]
        [Route("api/auth/refreshaccess")]
        public async Task<ActionResult> RefreshAsync() {
            var cookieAuth = await HttpContext.AuthenticateAsync("Cookies");
            await HttpContext.SignInAsync("Cookies", cookieAuth.Principal, cookieAuth.Properties);
            return Ok();
        }

        [HttpPost]
        [Route("api/auth/signout")]
        public async Task<ActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }

        
    }

    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Id { get; set; }

        public string Role { get; set; }
    }



}

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
            bool inDev = false;
            User userToCheck = null!;

            var _httpClient = _clientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");


            if (inDev is false) { 
                userToCheck = await _httpClient.GetFromJsonAsync<User>("https://fairfaxok.com/api/auth/getuserbyusername?username=" + cred.Username);
            }
            else if(inDev is true)
            {
                userToCheck = await _httpClient.GetFromJsonAsync<User>("https://localhost:7095/api/auth/getuserbyusername?username=" + cred.Username);
            }

            if(userToCheck is null) {
                return BadRequest();
            }else {
                if (userToCheck!.Username.Equals(cred.Username)){
                    if (userToCheck.Password.Equals(cred.Password)){
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, cred.Username),
                                new Claim(ClaimTypes.Role, userToCheck.Role),
                                new Claim("userId", userToCheck.Id.ToString())
                            };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = COOKIE_EXPIRES;

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        _httpClient.Dispose();

                        return this.Ok();
                    }
                    else
                    {
                        _httpClient.Dispose();
                        return BadRequest();
                    }
                }
                else
                {
                    _httpClient.Dispose();
                    return BadRequest();
                }
            }

           
        }

        [HttpPost]
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
    }



}

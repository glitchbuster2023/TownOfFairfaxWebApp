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
        public async Task<bool> SignIn(Credential cred)
        {
            bool inDev = false;
            User userToCheck = null!;

            var _httpClient = _clientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");


            if (inDev is false) { 
                userToCheck = await _httpClient.GetFromJsonAsync<User>("https://fairfaxoklahoma.azurewebsites.net/api/auth/getuserbyusername?username=" + cred.Username);
            }
            else if(inDev is true)
            {
                userToCheck = await _httpClient.GetFromJsonAsync<User>("https://localhost:7095/api/auth/getuserbyusername?username=" + cred.Username);
            }


            if (userToCheck!.Username.Equals(cred.Username))
            {
                //Valid Login Attempt
                if (userToCheck.Password.Equals(cred.Password))
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, cred.Username),
                            new Claim(ClaimTypes.Role, userToCheck.Role),
                            new Claim("userId", userToCheck.Id.ToString())
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = COOKIE_EXPIRES;

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }



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

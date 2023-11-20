using SportsNetCoreUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel;

// using static SportsNetCoreUI.Areas.Identity.Data.SportsNetCoreUIIdentityDbContext;
// using static SportsNetCoreUI.Areas.Identity.Pages._ViewStartPage;
// using static SportsNetCoreUI.Areas.Identity.Pages.Account.LoginModel;

namespace SportsNetCoreUI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        // private readonly UserManager<SportsNetCoreUIUser> userManager;

        // public TokenController(UserManager<SportsNetCoreUIUser> userManager)
        // {
        //     this.userManager = userManager;
        // }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InputModel model)
        {
            // var user = await userManager.FindByNameAsync(model.Email);
            // var passCheck = !await userManager.CheckPasswordAsync(user, model.Password);


            // if (user == null || passCheck)
            // {
            //     return Unauthorized();
            // }

            // var authClaims = new List<Claim>
            // {
            //     // new(JwtRegisteredClaimNames.Sub, user.UserName),
            //     // new(JwtRegisteredClaimNames.Email, user.Email),
            //     new(JwtRegisteredClaimNames.Sub as string, user.UserName as string),
            //     new(JwtRegisteredClaimNames.Email as string, user.Email as string),
            //     new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // };

            var authClaims = "";

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("V€r¥ $ecret (not!)"));
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expires = token.ValidTo
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("products")]
        public async Task<IActionResult> GetProducts()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://localhost:7113/products");
            var data = await response.Content.ReadAsStringAsync();

            return Ok(data);
        }
    }
}

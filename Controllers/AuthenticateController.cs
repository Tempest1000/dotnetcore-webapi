using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleWebApi.Domain.DTO;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public object CustomClaimsType { get; private set; }

        // POST api/authenticate
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] LoginDto dto)
        {
            if (dto.Username == "admin" && dto.Password == "admin")
            {
                // https://jonhilton.net/complex-aspnet-core-custom-security-policies
                // https://github.com/jonhilt/NetCoreAuth/tree/master/NetCoreJWTAuth.App

                string series = string.Empty;
                // series (session Id) info from Java, add as a JWT claim
                //byte[] newSeries = new byte[16];
                //random.nextBytes(newSeries);
                //series = Base64.encode(newSeries);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, dto.Username),
                    new Claim("CompletedBasicTraining", ""),
                    new Claim("series", "")
                };

                //,
                //new Claim("TokenAlive", true.ToString(), ClaimValueTypes.Boolean)

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "genco.com",
                    audience: "genco.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}

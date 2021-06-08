using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using service.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class App1Controller : ControllerBase
    {



        private readonly IConfiguration _configuration;

        public ClaimsIdentity Subject { get; private set; }

        public Logmodel log = new Logmodel();

        public App1Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
        
        [HttpGet]
        public string GetToken(string email)
        {
           

            if (email != "surawat@it-element.com" )
            {
                return null;
            }
            else {

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.NameId, "13"),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, "Surawat lowtuy"),
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

                var token = new JwtSecurityToken(
                   issuer: _configuration["JwtIssuer"],
                   audience: _configuration["JwtAudience"],
                   claims: claims,
                   expires: expires,
                   signingCredentials: creds
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }


           
        }


        // GET: api/App1
        [Authorize]
        [HttpGet]   
        public IEnumerable<string> Getdata()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/App1/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        [HttpPost]
        public string GenerateKey(string email)
        {
            //string email = "surawat@it-element.com";

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
               issuer: _configuration["JwtIssuer"],
               audience: _configuration["JwtAudience"],
               claims: claims,
               expires: expires,
               signingCredentials: creds
            );
            string tokenkey =  new JwtSecurityTokenHandler().WriteToken(token);

            return tokenkey;
        }
        // POST: api/App1
        //[HttpPost]
        // public void Post([FromBody] string value)
        // {

        // }

        [Authorize]
        [HttpPost]
        public IActionResult Acction([FromBody] JObject data)
        {
           
            var Token = Request.Headers["Authorization"];

            char[] delimiterChars = { ' ' };

            string text = Token.ToString();
          
            string[] keyslogin = text.Split(delimiterChars);



            log.info("Token is : "+ keyslogin[1].ToString());


            try
            {
                if (data == null)
                {

                    return BadRequest("object is null");
                }
                else
                {

                    return StatusCode(200, data);
                }

                
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Internal server error"+ex.ToString());
            }
        }


        // PUT: api/App1/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

  
}

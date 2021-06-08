using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using service.Models;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MobileController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> wealth_default_condition;

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, "SWAN Mobile Service");
           
        }
        [Authorize]
        [HttpGet("{item1}/{item2}", Name ="")]
        public IActionResult Get(string item1,string item2)
        {

            switch (item1)
            {
                case "forms_list":
                    return StatusCode(200, service.Models.Mobile.foms_list());
                    break;

                case "forms":
                    return StatusCode(200, service.Models.Mobile.foms_swan(item2));
                    break;
                default:
                    return StatusCode(400, "SWAN Mobile You Request Not Avaliable!");
                    break;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post()
        {
            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            log.info("log Post Mobile : " + body.ToString());


             var request_data = JObject.Parse(body);


            return StatusCode(200, wealth_default_condition);
        }
        // PUT: api/App1/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
             return StatusCode(200, "method Not allow");
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(200, "method Not allow");
        }
    }

  
}

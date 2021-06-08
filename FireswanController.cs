using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FireswanController : Controller
    {
       
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();
        Fireswanmodel Fireswan = new Fireswanmodel();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return   new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "";
        }

        // POST api/values
        [HttpPost]
        public IEnumerable<string> Post(string value)
        {
            return new string[] { "value1", "value2" };
        }

        // PUT api/values/5
        [HttpPut]
        public IEnumerable<string> Put(string value)
        {
            return new string[] { "value1", "value2" };
        }

        // DELETE api/values/5
        [HttpDelete]
        public IEnumerable<string> Delete(int id)
        {
            return new string[] { "value1", "value2" };
        }
    }
}

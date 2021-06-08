using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using service.Models;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class Fireswan_serviceController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> fireswan_service_condition;

        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }


        // GET: api/App1
        //[Authorize]
        //[HttpGet]   
        //public IEnumerable<string> Getdata()
        //{
        // return new string[] { "value1", "value2" };
        // }

        // GET: api/App1/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>  Post()
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

            log.info("log Post Wealth : " + body.ToString());

            object sealnet_response = "";
            var fireswan_service_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Wealth : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
                case "fireswan":
                    fireswan_service_condition = new Dictionary<object, object>();
                    fireswan_service_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    fireswan_service_condition.Add("apiname", request_data["apiname"].ToString());
                    fireswan_service_condition.Add("version", request_data["version"].ToString());
                    fireswan_service_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    fireswan_service_condition.Add("response", await Task.Run(() => Fireswanmodel.Fire_mssql(request_data["version"].ToString(), request_data["data"].ToString())));
                    fireswan_service_status = "true";
                    break;
                case "Externalfireswan":
                    fireswan_service_condition = new Dictionary<object, object>();
                    fireswan_service_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    fireswan_service_condition.Add("apiname", request_data["apiname"].ToString());
                    fireswan_service_condition.Add("version", request_data["version"].ToString());
                    fireswan_service_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    fireswan_service_condition.Add("response", await Task.Run(() => Externalfireswan.External_Service(request_data["version"].ToString(), request_data["data"].ToString())));
                    fireswan_service_status = "true";
                    break;
                default:
                    fireswan_service_condition = new Dictionary<object, object>();
                    fireswan_service_condition.Add("info", "Web Api Access Not allow");
                    fireswan_service_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    fireswan_service_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    fireswan_service_status = "fail";
                    break;
            }

          

            return StatusCode(200, fireswan_service_condition);
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

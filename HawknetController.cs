using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using service.Models;


namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HawknetController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Hawknet_default_condition;


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

            log.info("log Post Hawknet : " + body.ToString());
            
            var hawknet_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Hawknet : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
                case "HawknetKyc":
                    Hawknet_default_condition = new Dictionary<object, object>();
                    Hawknet_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknet_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknet_default_condition.Add("version", request_data["version"].ToString());
                    Hawknet_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                   // Hawknet_default_condition.Add("response", HawknetModel.Hawknet_KYC(request_data["version"].ToString(), request_data["data"].ToString()));
                    hawknet_status = "true";
                    break;
                case "Hawknet_centralized":
                    Hawknet_default_condition = new Dictionary<object, object>();
                    Hawknet_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknet_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknet_default_condition.Add("version", request_data["version"].ToString());
                    Hawknet_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                   // Hawknet_default_condition.Add("response", Hawknet_Model.Hawknet_centralizedcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    hawknet_status = "true";

                    break;
                default:
                    Hawknet_default_condition = new Dictionary<object, object>();
                    Hawknet_default_condition.Add("info", "Web Api Access Not allow");
                    Hawknet_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Hawknet_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    hawknet_status = "fail";
                    break;
            }

          

            return StatusCode(200, Hawknet_default_condition);
        }

       

        // PUT: api/App1/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
             return StatusCode(200, "method Not allow");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(200, "method Not allow");
        }
    }

  
}

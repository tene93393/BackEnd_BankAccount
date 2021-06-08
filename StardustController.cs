using System;
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
    public class StardustController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Stardust_default_condition;

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

            //log.info("log Post Wealth : " + body.ToString());

          
            var Stardust_status = "";

            var request_data = JObject.Parse(body);

            //log.info("log Parser Wealth : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
                case "todo":
                    Stardust_default_condition = new Dictionary<object, object>();
                    Stardust_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Stardust_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Stardust_default_condition.Add("version", request_data["version"].ToString());
                    Stardust_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Stardust_default_condition.Add("response", Stardust_Model.Stardust_todo(request_data["version"].ToString(), request_data["data"].ToString()));
                    Stardust_status = "true";
                    break;
                case "Task":
                    Stardust_default_condition = new Dictionary<object, object>();
                    Stardust_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Stardust_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Stardust_default_condition.Add("version", request_data["version"].ToString());
                    Stardust_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Stardust_default_condition.Add("response", Stardust_Model.Stardust_task(request_data["version"].ToString(), request_data["data"].ToString()));
                    Stardust_status = "true";
                    break;
                case "meeting":
                    Stardust_default_condition = new Dictionary<object, object>();
                    Stardust_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Stardust_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Stardust_default_condition.Add("version", request_data["version"].ToString());
                    Stardust_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Stardust_default_condition.Add("response", Stardust_Model.Stardust_meeting(request_data["version"].ToString(), request_data["data"].ToString()));
                    Stardust_status = "true";

                    break;
                case "Jobs":
                    Stardust_default_condition = new Dictionary<object, object>();
                    Stardust_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Stardust_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Stardust_default_condition.Add("version", request_data["version"].ToString());
                    Stardust_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Stardust_default_condition.Add("response", Stardust_Model.Stardust_jobs(request_data["version"].ToString(), request_data["data"].ToString()));
                    Stardust_status = "true";

                    break;
                default:
                    Stardust_default_condition = new Dictionary<object, object>();
                    Stardust_default_condition.Add("info", "Web Api Access Not allow");
                    Stardust_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Stardust_status = "fail";
                    break;
            }

            if (Stardust_status == "fail")
            {
                return StatusCode(400, Stardust_default_condition);
            }
            else {
                return StatusCode(200, Stardust_default_condition);
            }

           
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

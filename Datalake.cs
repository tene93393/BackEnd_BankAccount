using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;


namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DatalakeController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Datalake_default_condition;
        private Dictionary<object, object> raw_data;

        [HttpGet]
        public IActionResult Get()
        {
            var temp_status = new Dictionary<object,object>();
            temp_status.Add("data",Date.date_th_string());

            return StatusCode(200, temp_status);
        }

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

            log.info("log Post Datalake : " + body.ToString());

            object Datalake = "";
            var Datalake_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Wealth : " + request_data.ToString());
            try
            {
                switch (request_data["apiname"].ToString())
                {
                    case "infoswan":
                        Datalake_default_condition = new Dictionary<object, object>();

                        raw_data = Swan_logic.infoswan(request_data["version"].ToString(), request_data["data"].ToString());


                        Datalake_default_condition.Add("apiname", request_data["apiname"].ToString());
                        Datalake_default_condition.Add("version", request_data["version"].ToString());
                        Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_default_condition.Add("response", raw_data["result"]);
                        Datalake_status = "true";
                        break;
                    case "lookup":

                        var temp = JObject.Parse(request_data["data"].ToString());

                            Mysqlswan mysqlswan = new Mysqlswan();
                            Datalake_default_condition = new Dictionary<object, object>();
                            var lookup_arr = new Dictionary<object, object>();
                            string Command = "lookuplist";
                            Dictionary<object, object> param = new Dictionary<object, object>();
                            param.Add("@v1", temp["func"].ToString());
                            var result = mysqlswan.data_with_col_procedures(Command, param);

                    
                       

                        JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

                        foreach (var item in temp_data)
                        {
                            lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                        }





                        Datalake_default_condition.Add("apiname", request_data["apiname"].ToString());
                        Datalake_default_condition.Add("version", request_data["version"].ToString());
                        Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_default_condition.Add("response", lookup_arr);
                        Datalake_status = "true";
                        break;

                    case "Form":

                        Datalake_default_condition = new Dictionary<object, object>();

                        raw_data = Swan_logic.form(request_data["version"].ToString(), request_data["data"].ToString());

                        Datalake_default_condition.Add("apiname", request_data["apiname"].ToString());
                        Datalake_default_condition.Add("version", request_data["version"].ToString());
                        Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_default_condition.Add("response", raw_data["result"]);
                        Datalake_status = "true";
                        break;

                    case "Flow":

                        Datalake_default_condition = new Dictionary<object, object>();

                        raw_data = Swan_logic.flow(request_data["version"].ToString(), request_data["data"].ToString());

                        Datalake_default_condition.Add("apiname", request_data["apiname"].ToString());
                        Datalake_default_condition.Add("version", request_data["version"].ToString());
                        Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_default_condition.Add("response", raw_data["result"]);
                        Datalake_status = "true";
                        break;

                    default:
                        Datalake_default_condition = new Dictionary<object, object>();
                        Datalake_default_condition.Add("info", "Web Api Access Not allow");

                        Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_status = "fail";
                        break;
                }
            }
            catch (Exception) {

                Datalake_default_condition = new Dictionary<object, object>();
                Datalake_default_condition.Add("info", "Web Api Access Not allow");
                Datalake_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                Datalake_status = " ";

            }

            var status_api = "";
            if (Datalake_status == "Create")
            {
                status_api = "201";
            }
            else if (Datalake_status == "true")
            {
                status_api = "200";
            }
            else if (Datalake_status == "fail")
            {
                status_api = "400";
            }
            else if (Datalake_status == " ")
            {
                status_api = "500";
            }
            return StatusCode(Int32.Parse(status_api), Datalake_default_condition);

          
        }

       

        // PUT: api/App1/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
             return StatusCode(400, "method Not allow");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(400, "method Not allow");
        }
    }

  
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using service.Models;
using Newtonsoft.Json;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class Tiger_apiController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> tiger_default_condition;


        [HttpGet]
        public IActionResult Get()
        {

            return StatusCode(400, "method Not allow");
        }



        [HttpPost]
        public async Task<IActionResult> Post()
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

            log.info("log Post Tiger : " + body.ToString());

            object tiger_response = "";
            var tiger_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Tiger : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
               
                case "Groupcode":
                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //tiger_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_default_condition.Add("response",await Edocument_model.Edocument_group(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_status = "true";

                    break;

                case "Docgroup":

                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //tiger_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_default_condition.Add("response", await Edocument_model.Edocument_code(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_status = "true";

                    break;

                case "Document_Flow":

                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //tiger_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_default_condition.Add("response", "Document_Flow tiger OK");
                    tiger_status = "true";

                    break;

                case "Document_require_account":

                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //tiger_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_default_condition.Add("response", "Document_require tiger OK");
                    tiger_status = "true";

                    break;

                case "Document_require":  //un upload document

                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //tiger_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    tiger_default_condition.Add("response", "Document_require tiger OK");
                    tiger_status = "true";

                    break;

               

                default:


                    log.info("Start ================== > Tiger logs");

                    Mysqltiger tigerdb = new Mysqltiger();

                    string Command = "Tiger_command";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("@v1", request_data["apiname"].ToString());
                    param.Add("@v2", request_data["data"].ToString());


                    log.info("Tiger logs Command "+ Command);

                    var  result_function = await tigerdb.data_with_col_procedures(Command, param);

                    log.info("Tiger Resutl "+JsonConvert.SerializeObject(result_function));

                    tiger_default_condition = new Dictionary<object, object>();
                    tiger_default_condition.Add("apiname", request_data["apiname"].ToString());
                    tiger_default_condition.Add("version", request_data["version"].ToString());
                    tiger_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    tiger_default_condition.Add("response", result_function);
                    tiger_status = "true";

                    break;
            }

          

            return StatusCode(200, tiger_default_condition);
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

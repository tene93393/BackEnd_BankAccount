using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using service.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using service.Models;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WealthController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> wealth_default_condition;


        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
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

            log.info("log Post Wealth : " + body.ToString());

            object sealnet_response = "";
            var wealth_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Wealth : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
                case "wealth001":

                    log.info("Log  Action  Wealth001 : " + request_data.ToString());

                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealth001(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";
                    break;
                case "wealth002":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealth002(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";
                    break;
                case "wealthreport_ap":

                    log.info("Log  Action  Wealth001 : " + request_data.ToString());

                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthreport_ap(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";
                    break;
                case "asp_wealths":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.asp_wealths(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "customer":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthcustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "bank":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthbank(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "universe":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthuniverse(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "fund":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthfund(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "port":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthport(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "eod":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealtheod(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "report":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthreport(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";

                    break;
                case "trade":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response", Wealthnet_Model.wealthtrade(request_data["version"].ToString(), request_data["data"].ToString()));
                    wealth_status = "true";
                    break;

                case "Openaccount":
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    wealth_default_condition.Add("apiname", request_data["apiname"].ToString());
                    wealth_default_condition.Add("version", request_data["version"].ToString());
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    // wealth_default_condition.Add("response", Wealthnet_Model.wealthtrade(request_data["version"].ToString(), request_data["data"].ToString()));


                    switch (request_data["data"]["func"].ToString()) {
                        case "acc_info":
                            wealth_default_condition.Add("response", await Wealthinterface.acc_info(request_data["data"].ToString()));
                            break;
                        case "contract_info":
                            wealth_default_condition.Add("response", await Wealthinterface.acc_info(request_data["data"].ToString()));
                            break;
                        case "kyc_info":
                            wealth_default_condition.Add("response", await Wealthinterface.kyc_info(request_data["data"].ToString()));
                            break;
                        case "suite_info":
                            wealth_default_condition.Add("response", await Wealthinterface.suite_info(request_data["data"].ToString()));
                            break;
                        case "marketing_info":
                            wealth_default_condition.Add("response", await Wealthinterface.marketing_info(request_data["data"].ToString()));
                            break;
                        case "legal_address_info":
                            wealth_default_condition.Add("response", await Wealthinterface.legal_address_info(request_data["data"].ToString()));
                            break;
                        case "current_address_info":
                            wealth_default_condition.Add("response", await Wealthinterface.current_address_info(request_data["data"].ToString()));
                            break;
                        case "emergency_contact_info":
                            wealth_default_condition.Add("response", await Wealthinterface.emergency_contact_info(request_data["data"].ToString()));
                            break;
                        default:
                        
                            break;
                    } 
                         
                    wealth_status = "true";

                    break;
                default:
                    wealth_default_condition = new Dictionary<object, object>();
                    wealth_default_condition.Add("info", "Web Api Access Not allow");
                    wealth_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    wealth_status = "fail";
                    break;
            }

          

            return StatusCode(200, wealth_default_condition);
        }

        private async Task<IActionResult> post_mysql_listreport()
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

            log.info("log Post report : " + body.ToString());

            object sealnet_response = "";
            var wealth_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser report : " + request_data.ToString());

            var cmd = "SELECT concat('เอกสารเลขที่  ', templatecode ) AS name_report, concat('<button class=\"btn btn-xs btn - primary\" onclick=\"viewReport(','\'', templatecode, '\'', ')\">Click me</button>') AS btn_view FROM reportpdf_template";
            Mysqlswan mysqlswan = new Mysqlswan();
            Dictionary<object, object> param_total = new Dictionary<object, object>();
            param_total.Add("", "");
            var data_total = await mysqlswan.data_with_col(cmd, param_total);

            return StatusCode(200, data_total);

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

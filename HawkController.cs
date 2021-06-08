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
    public class HawkController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Hawknetcore_default_condition;

        public HawkController()
        {
        }

        //[Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }


        [Authorize]
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
            //log.info("log Post Wealth : " + body.ToString());           
            var Hawknetcore_status = "";
            var request_data = JObject.Parse(body);

            //log.info("log Parser Wealth : " + request_data.ToString());
            switch (request_data["apiname"].ToString())
            {
                case "Exitsing":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                  //  Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_Exitscustomer(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";
                    break;
                case "Openaccount":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));

                    switch (request_data["data"]["func"].ToString())
                    {
                        //case "trn_cust":
                        //    Hawknetcore_default_condition.Add("response", await  Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_ext":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_acc":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_acc_ext":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_acc_subaccount":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_group_exposure":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;

                        //case "trn_group_exposure_member":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys_add":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys_bank":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys_person":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_person":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys_questionnaire":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;
                        //case "trn_cust_sys_questionnaire_answer":
                        //    Hawknetcore_default_condition.Add("response", await Hawknet_Model.central_customer(request_data["data"]["func"].ToString(), request_data["data"].ToString()));
                        //    Hawknetcore_status = "true";
                        //    break;

                     
                        //default:
                        //    Hawknetcore_default_condition.Add("response","Fail  No api match");
                        //    Hawknetcore_status = "fail";
                        //    break;
                    }


                    // Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_centralizedcustomer(request_data["version"].ToString(), request_data["data"].ToString()));

                    break;
                case "Kyc":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                   // Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_KYC(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";

                    break;
                case "BatchKyc":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                   // Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_BatchKyc(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";

                    break;
                case "UpdateProfile":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                   // Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_UpdateProfile(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";

                    break;
                case "Importdata":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_Importdata(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";

                    break;
                case "Management_datakyc":
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    Hawknetcore_default_condition.Add("apiname", request_data["apiname"].ToString());
                    Hawknetcore_default_condition.Add("version", request_data["version"].ToString());
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    //Hawknetcore_default_condition.Add("response",await Hawknet_Model.Hawknet_Management_datakyc(request_data["version"].ToString(), request_data["data"].ToString()));
                    Hawknetcore_status = "true";

                    break;
                default:
                    Hawknetcore_default_condition = new Dictionary<object, object>();
                    Hawknetcore_default_condition.Add("info", "Web Api Access Not allow");
                    Hawknetcore_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    Hawknetcore_status = "fail";
                    break;
            }


            if (Hawknetcore_status == "fail")
            {
                return StatusCode(400, Hawknetcore_default_condition);
            }
            else
            {
                return StatusCode(200, Hawknetcore_default_condition);
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

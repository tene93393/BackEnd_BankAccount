using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using service.Models;


namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SealController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> seal_default_condition;


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
            var seal_status = "";

            var request_data = JObject.Parse(body);

            log.info("log Parser Wealth : " + request_data.ToString());

            switch (request_data["apiname"].ToString())
            {
                case "sealnet":

                    seal_default_condition = new Dictionary<object, object>();
                    seal_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                    seal_default_condition.Add("apiname", request_data["apiname"].ToString());
                    seal_default_condition.Add("version", request_data["version"].ToString());
                    seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                 //   seal_default_condition.Add("response", Seal_Model.sealapi(request_data["version"].ToString(), request_data["data"].ToString()));
                    seal_status = "true";

                    break;

                case "sealauthen":
                    try
                    {
                        var client_seal = new RestClient("http://10.9.101.99:5008/Account/Loginmobile");
                        client_seal.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AlwaysMultipartFormData = true;
                        request.AddParameter("Email", "surawat@ite-asp.local");
                        request.AddParameter("Password", "@welcome");
                        IRestResponse response = client_seal.Execute(request);
                        Console.WriteLine(response.Content);

                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                        seal_default_condition.Add("apiname", request_data["apiname"].ToString());
                        seal_default_condition.Add("version", request_data["version"].ToString());
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response", "Seal_authen : "+ response.Content.ToString());
                        seal_status = "true";
                    }
                    catch (Exception e) {


                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("info", "Web Api Access Not allow");
                        seal_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response", "Error : " + e.ToString());
                        seal_status = "fail";
                    }

                    break;
                case "Addcustomer":
         
                    var alldata = new Dictionary<object, object>();
         
                    alldata.Add("apiname","ins_custinfo");
                    alldata.Add("custid","047818");
                    alldata.Add("custtype","1");
                    alldata.Add("cussubtype","1");
                    alldata.Add("thainame","วรเวท");
                    alldata.Add("thaisurname","เอกโท");
                    alldata.Add("engname","Worawat");
                    alldata.Add("engsurname","Aketo");
                    alldata.Add("birthdate","19960805");
                    alldata.Add("gender","M");
                    alldata.Add("marketID"," 000076");




                    try
                    {
                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                        seal_default_condition.Add("apiname", request_data["apiname"].ToString());
                        seal_default_condition.Add("version", request_data["version"].ToString());
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response_endpint", "Seal_endpoint : " + Appconfig.back_office_config("openaccount_seal", "endpoint") + "/Pacific");
                        seal_default_condition.Add("response_data", "Seal_data : " + JsonConvert.SerializeObject(alldata));
                        seal_default_condition.Add("response", "Seal_AddCustomer : " + await Sealinterface.pacific(alldata));
                        seal_status = "true";
                    }
                    catch (Exception e)
                    {


                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("info", "Web Api Access Not allow");
                        seal_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response", "Error : " + e.ToString());
                        seal_status = "fail";
                    }

                    break;

                case "openacc_flow":

                   




                    try
                    {
                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("webapi_access_token", request_data["webapi_access_token"].ToString());
                        seal_default_condition.Add("apiname", request_data["apiname"].ToString());
                        seal_default_condition.Add("version", request_data["version"].ToString());
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response_endpint", "Seal_endpoint : " + Appconfig.back_office_config("openaccount_seal", "endpoint") + "/Pacific");

                        seal_default_condition.Add("response_customer", "Seal_AddCustomer : " + await Sealinterface.Constomer_individual(request_data["data"]["flow"].ToString()));
                        seal_default_condition.Add("response_customerdetail", "Seal_AddCustomerdetail : " + await Sealinterface.Constomer_detail(request_data["data"]["flow"].ToString()));
                        seal_default_condition.Add("response_account", "Seal_AddAccount : " + await Sealinterface.Account_info(request_data["data"]["flow"].ToString()));

                        seal_status = "true";
                    }
                    catch (Exception e)
                    {


                        seal_default_condition = new Dictionary<object, object>();
                        seal_default_condition.Add("info", "Web Api Access Not allow");
                        seal_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        seal_default_condition.Add("response", "Error : " + e.ToString());
                        seal_status = "fail";
                    }

                    break;

                default:
                    seal_default_condition = new Dictionary<object, object>();
                    seal_default_condition.Add("info", "Web Api Access Not allow");
                    seal_default_condition.Add("Respond date ", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    seal_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                    seal_status = "fail";
                    break;
            }

          

            return StatusCode(200, seal_default_condition);
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

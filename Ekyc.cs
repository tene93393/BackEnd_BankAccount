using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;


namespace service
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EkycController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Apiflow_default_condition;
        private Dictionary<object, object> raw_data;
      

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return StatusCode(200, "Ekyc");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            Logmodel log = new Logmodel();
            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

        
            object Datalake = "";
            var Datalake_status = "";
            var request_data = JObject.Parse(body);

            Openaccount open = new Openaccount();
            Check_customer chk = new Check_customer();

            try
            {
                switch (request_data["apiname"].ToString())
                {
                    case "dipchip":

                        Apiflow_default_condition = new Dictionary<object, object>();
                        _ = log.swan_core_log("EKYC"," transter Tiger diffship : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            _ = log.swan_core_log("EKYC", " Data before Save  : " + JsonConvert.SerializeObject(request_data));

                            Mysqlhawk hawk = new Mysqlhawk();

                            var param_ins = new Dictionary<object, object>();
                            param_ins.Add("channel", "Tiger_Windows");
                            param_ins.Add("date_request", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            param_ins.Add("citizenid", request_data["data"]["Id"].ToString());
                            param_ins.Add("ThaiName", request_data["data"]["ThaiName"].ToString());
                            param_ins.Add("EngName", request_data["data"]["EngName"].ToString());
                            param_ins.Add("EnglishNameWithPrefix", request_data["data"]["EnglishNameWithPrefix"].ToString());
                            param_ins.Add("EnglishSurname", request_data["data"]["EnglishSurname"].ToString());
                            param_ins.Add("DateOfBirth", request_data["data"]["DateOfBirth"].ToString());
                            param_ins.Add("ThaiDOB", request_data["data"]["ThaiDOB"].ToString());
                            param_ins.Add("Gender", request_data["data"]["Gender"].ToString());
                            param_ins.Add("BP1", request_data["data"]["BP1"].ToString());
                            param_ins.Add("Issuer", request_data["data"]["Issuer"].ToString());
                            param_ins.Add("IssuerCode", request_data["data"]["IssuerCode"].ToString());
                            param_ins.Add("DateOfIssue", request_data["data"]["DateOfIssue"].ToString());
                            param_ins.Add("ThaiDOI", request_data["data"]["ThaiDOI"].ToString());
                            param_ins.Add("DateOfExpiry", request_data["data"]["DateOfExpiry"].ToString());
                            param_ins.Add("ThaiDOE", request_data["data"]["ThaiDOE"].ToString());
                            param_ins.Add("TypeCode", request_data["data"]["TypeCode"].ToString());
                            param_ins.Add("Address", request_data["data"]["Address"].ToString());
                            param_ins.Add("PhotoId", request_data["data"]["PhotoId"].ToString());
                            param_ins.Add("photo_raw", request_data["data"]["images"].ToString());
                            param_ins.Add("chipid", request_data["data"]["ChipId"].ToString());



                            param_ins.Add("status", "Y");
                            param_ins.Add("consent_dopa", "N");
                            param_ins.Add("consent_date", "");
                            param_ins.Add("consent_status", "");
                            param_ins.Add("user_request", "");

                            var ins_data = hawk.data_ins("trn_dopa", JsonConvert.SerializeObject(param_ins));
                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship : " + JsonConvert.SerializeObject(ins_data));

                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", "transfer data Complete");
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }





                        break;
                    case "dipchip_android":


                        Apiflow_default_condition = new Dictionary<object, object>();

                        _ = log.swan_core_log("EKYC", " transter Tiger diffship : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            Mysqlhawk hawk = new Mysqlhawk();

                            var param_ins = new Dictionary<object, object>();
                            param_ins.Add("channel", "android");
                            param_ins.Add("date_request", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            param_ins.Add("citizenid", request_data["data"]["Id"].ToString());
                            param_ins.Add("ThaiName", request_data["data"]["ThaiName"].ToString());
                            param_ins.Add("EngName", request_data["data"]["EngName"].ToString());
                            param_ins.Add("EnglishNameWithPrefix", request_data["data"]["EnglishNameWithPrefix"].ToString());
                            param_ins.Add("EnglishSurname", request_data["data"]["EnglishSurname"].ToString());
                            param_ins.Add("DateOfBirth", request_data["data"]["DateOfBirth"].ToString());
                            param_ins.Add("ThaiDOB", request_data["data"]["ThaiDOB"].ToString());
                            param_ins.Add("Gender", request_data["data"]["Gender"].ToString());
                            param_ins.Add("BP1", request_data["data"]["BP1"].ToString());
                            param_ins.Add("Issuer", request_data["data"]["Issuer"].ToString());
                            param_ins.Add("IssuerCode", request_data["data"]["IssuerCode"].ToString());
                            param_ins.Add("DateOfIssue", request_data["data"]["DateOfIssue"].ToString());
                            param_ins.Add("ThaiDOI", request_data["data"]["ThaiDOI"].ToString());
                            param_ins.Add("DateOfExpiry", request_data["data"]["DateOfExpiry"].ToString());
                            param_ins.Add("ThaiDOE", request_data["data"]["ThaiDOE"].ToString());
                            param_ins.Add("TypeCode", request_data["data"]["TypeCode"].ToString());
                            param_ins.Add("Address", request_data["data"]["Address"].ToString());
                            param_ins.Add("PhotoId", request_data["data"]["PhotoId"].ToString());
                            param_ins.Add("photo_raw", request_data["data"]["images"].ToString());
                            param_ins.Add("chipid", request_data["data"]["ChipId"].ToString());

                            param_ins.Add("status", "Y");
                            param_ins.Add("consent_dopa", "N");
                            param_ins.Add("consent_date", "");
                            param_ins.Add("consent_status", "");
                            param_ins.Add("user_request", "");

                            var ins_data = hawk.data_ins("trn_dopa", JsonConvert.SerializeObject(param_ins));
                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship : " + JsonConvert.SerializeObject(ins_data));

                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", "transfer data Complete");
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }


                        break;
                    case "dipchip_ios":


                        Apiflow_default_condition = new Dictionary<object, object>();

                        _ = log.swan_core_log("EKYC", " transter Tiger diffship : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            Mysqlhawk hawk = new Mysqlhawk();

                            var param_ins = new Dictionary<object, object>();
                            param_ins.Add("channel", "ios");
                            param_ins.Add("date_request", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            param_ins.Add("citizenid", request_data["data"]["Id"].ToString());
                            param_ins.Add("ThaiName", request_data["data"]["ThaiName"].ToString());
                            param_ins.Add("EngName", request_data["data"]["EngName"].ToString());
                            param_ins.Add("EnglishNameWithPrefix", request_data["data"]["EnglishNameWithPrefix"].ToString());
                            param_ins.Add("EnglishSurname", request_data["data"]["EnglishSurname"].ToString());
                            param_ins.Add("DateOfBirth", request_data["data"]["DateOfBirth"].ToString());
                            param_ins.Add("ThaiDOB", request_data["data"]["ThaiDOB"].ToString());
                            param_ins.Add("Gender", request_data["data"]["Gender"].ToString());
                            param_ins.Add("BP1", request_data["data"]["BP1"].ToString());
                            param_ins.Add("Issuer", request_data["data"]["Issuer"].ToString());
                            param_ins.Add("IssuerCode", request_data["data"]["IssuerCode"].ToString());
                            param_ins.Add("DateOfIssue", request_data["data"]["DateOfIssue"].ToString());
                            param_ins.Add("ThaiDOI", request_data["data"]["ThaiDOI"].ToString());
                            param_ins.Add("DateOfExpiry", request_data["data"]["DateOfExpiry"].ToString());
                            param_ins.Add("ThaiDOE", request_data["data"]["ThaiDOE"].ToString());
                            param_ins.Add("TypeCode", request_data["data"]["TypeCode"].ToString());
                            param_ins.Add("Address", request_data["data"]["Address"].ToString());
                            param_ins.Add("PhotoId", request_data["data"]["PhotoId"].ToString());
                            param_ins.Add("photo_raw", request_data["data"]["images"].ToString());
                            param_ins.Add("chipid", request_data["data"]["ChipId"].ToString());


                            param_ins.Add("status", "Y");
                            param_ins.Add("consent_dopa", "N");
                            param_ins.Add("consent_date", "");
                            param_ins.Add("consent_status", "");
                            param_ins.Add("user_request", "");

                            var ins_data = hawk.data_ins("trn_dopa", JsonConvert.SerializeObject(param_ins));
                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship : " + JsonConvert.SerializeObject(ins_data));

                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", "transfer data Complete");
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }


                        break;

                    case "nfc":


                        Apiflow_default_condition = new Dictionary<object, object>();

                        _ = log.swan_core_log("EKYC", " transter Tiger diffship : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            Mysqlhawk hawk = new Mysqlhawk();

                            var param_ins = new Dictionary<object, object>();
                            param_ins.Add("channel", "nfc");
                            param_ins.Add("date_request", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            param_ins.Add("citizenid", request_data["data"]["Id"].ToString());
                            param_ins.Add("ThaiName", request_data["data"]["ThaiName"].ToString());
                            param_ins.Add("EngName", request_data["data"]["EngName"].ToString());
                            param_ins.Add("EnglishNameWithPrefix", request_data["data"]["EnglishNameWithPrefix"].ToString());
                            param_ins.Add("EnglishSurname", request_data["data"]["EnglishSurname"].ToString());
                            param_ins.Add("DateOfBirth", request_data["data"]["DateOfBirth"].ToString());
                            param_ins.Add("ThaiDOB", request_data["data"]["ThaiDOB"].ToString());
                            param_ins.Add("Gender", request_data["data"]["Gender"].ToString());
                            param_ins.Add("BP1", request_data["data"]["BP1"].ToString());
                            param_ins.Add("Issuer", request_data["data"]["Issuer"].ToString());
                            param_ins.Add("IssuerCode", request_data["data"]["IssuerCode"].ToString());
                            param_ins.Add("DateOfIssue", request_data["data"]["DateOfIssue"].ToString());
                            param_ins.Add("ThaiDOI", request_data["data"]["ThaiDOI"].ToString());
                            param_ins.Add("DateOfExpiry", request_data["data"]["DateOfExpiry"].ToString());
                            param_ins.Add("ThaiDOE", request_data["data"]["ThaiDOE"].ToString());
                            param_ins.Add("TypeCode", request_data["data"]["TypeCode"].ToString());
                            param_ins.Add("Address", request_data["data"]["Address"].ToString());
                            param_ins.Add("PhotoId", request_data["data"]["PhotoId"].ToString());
                            param_ins.Add("photo_raw", request_data["data"]["images"].ToString());
                            param_ins.Add("chipid", request_data["data"]["ChipId"].ToString());

                            param_ins.Add("status", "Y");
                            param_ins.Add("consent_dopa", "N");
                            param_ins.Add("consent_date", "");
                            param_ins.Add("consent_status", "");
                            param_ins.Add("user_request", "");

                            var ins_data = hawk.data_ins("trn_dopa", JsonConvert.SerializeObject(param_ins));
                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship : " + JsonConvert.SerializeObject(ins_data));

                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", "transfer data Complete");
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }


                        break;
                    case "consent_dopa":


                        Apiflow_default_condition = new Dictionary<object, object>();

                        _ = log.swan_core_log("dopa", " Request consent dopa : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            //Mysqlhawk hawk = new Mysqlhawk();
                            var laser_data = new Dictionary<object, object>();
                            laser_data.Add("apiname", "laser");
                            laser_data.Add("PIN", request_data["PIN"].ToString());
                            laser_data.Add("FirstName", request_data["FirstName"].ToString());
                            laser_data.Add("LastName", request_data["LastName"].ToString());
                            laser_data.Add("BirthDay", request_data["BirthDay"].ToString());
                            laser_data.Add("Laser", request_data["Laser"].ToString());


                            _ = log.swan_core_log("dopa", " Cleansin EKYC : " + JsonConvert.SerializeObject(laser_data));



                            var dopadata = await Dopa_model.call_service_dopa("lasercode", JsonConvert.SerializeObject(laser_data));


                            _ = log.swan_core_log("dopa", " Request consent dopa  data : " + JsonConvert.SerializeObject(dopadata));

                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", dopadata["status"].ToString());
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("dopa", " Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }


                        break;
                    case "consent_ndid":


                        Apiflow_default_condition = new Dictionary<object, object>();

                        _ = log.swan_core_log("EKYC", " transter Tiger diffship : " + JsonConvert.SerializeObject(body));
                        try
                        {

                            //   Mysqlhawk hawk = new Mysqlhawk();

                            var ndid_raw_data = new Dictionary<object, object>();

                            ndid_raw_data.Add("cardid", request_data["cardid"].ToString());
                            ndid_raw_data.Add("idps", request_data["idps"].ToString());
                   



                            var ndid_data =  await NDID_model.call_service_ndid("verifly_request", JsonConvert.SerializeObject(ndid_raw_data));

                            //  นำ ข้อมูลมา Prse ต่อ  
 




                            Apiflow_default_condition.Add("apiname", request_data["apiname"].ToString());
                            Apiflow_default_condition.Add("version", request_data["version"].ToString());
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", ndid_data);
                            Datalake_status = "true";


                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("EKYC", " Inse Tiger diffship Error : " + e.ToString());

                            Apiflow_default_condition = new Dictionary<object, object>();
                            Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", e.ToString());
                            Datalake_status = "fail";

                        }


                        break;

                    default:
                        Apiflow_default_condition = new Dictionary<object, object>();
                        Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                        Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_status = "fail";
                        break;
                }
            }
            catch (Exception e) {

                Apiflow_default_condition = new Dictionary<object, object>();
                Apiflow_default_condition.Add("Error", e.ToString());
                Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
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
            return StatusCode(Int32.Parse(status_api), Apiflow_default_condition);

          
        }


    }

  
}

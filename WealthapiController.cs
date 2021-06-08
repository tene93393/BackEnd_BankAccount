using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;

namespace service.Controllers
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WealthapiController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> wealth_default_condition;
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "wealth", "ite_smartBiz" };
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            Mysqlswan swan = new Mysqlswan();
            Mysqlwealth wealth = new Mysqlwealth();
            var param_ins = new Dictionary<object, object>();
            var api_status = "";
            var Apiflow_default_condition = new Dictionary<object, object>();
            try
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
                _ = log.swan_core_log("wealthapi", "log Post Wealth : " + body.ToString());
                var request_data = JObject.Parse(body);
                _ = log.swan_core_log("wealthapi", "log Parser Wealth : " + request_data.ToString());

                var mappingdata = await apimap_wealth(request_data["apiname"].ToString().Replace(request_data["apiname"].ToString().Substring(0, 4), ""));

                var sub_prefix_interface = request_data["apiname"].ToString().Substring(0, 4);

                _ = log.swan_core_log("wealthapi", "Status sub_prefix apiname   : " + sub_prefix_interface);

                var ps_mappingdata = JObject.Parse(mappingdata);


                switch (sub_prefix_interface)
                {
                    case "ins_":

                        foreach (var item in ps_mappingdata)
                        {
                            param_ins.Add(item.Value.ToString(), request_data[item.Key.ToString()].ToString());
                        }

                        var ins_data = wealth.data_ins(request_data["apiname"].ToString().Replace("ins_", ""), JsonConvert.SerializeObject(param_ins));
                        _ = log.swan_core_log("wealthapi", "Status apiaction   : " + JsonConvert.SerializeObject(ins_data));

                        break;
                    case "upd_":


                        foreach (var item in ps_mappingdata)
                        {
                            param_ins.Add(item.Value.ToString(), request_data[item.Key.ToString()].ToString());
                        }
                      
                        var param_condition = new Dictionary<object, object>();
                        param_condition.Add("hawkid", request_data["hawkid"].ToString());
                        var upd_data = wealth.data_update(request_data["apiname"].ToString().Replace("upd_", ""), JsonConvert.SerializeObject(param_ins), JsonConvert.SerializeObject(param_condition));
                        _ = log.swan_core_log("wealthapi", "Status Update status   : " + JsonConvert.SerializeObject(upd_data));
                        break;
                    case "str_":

                    
                        var list_field = new List<object>();

                       
                        var list_param = new List<object>();
                        foreach (var item in ps_mappingdata)
                        {
                            list_param.Add("@" + item.Key.ToString());
                        }
                        string Command = "CALL " + request_data["apiname"].ToString().Replace("str_", "") + "(" + String.Join(',', list_param) + ")";

                        _ = log.swan_core_log("wealthapi", "Command db" + Command);


                        Dictionary<object, object> param = new Dictionary<object, object>();
                        foreach (var item in ps_mappingdata)
                        {

                            if (item.Key.ToString() == "apiname")
                            {

                            }
                            else {
                                param.Add(item.Key.ToString(), request_data[item.Key.ToString()].ToString());
                            }

                           
                        }

                        _ = log.swan_core_log("wealthapi", "Param call stroc proc db" +JsonConvert.SerializeObject(param));

                        var proc_result = wealth.data_utility(Command, param);
                        _ = log.swan_core_log("wealthapi", "Status proc_data   : " + JsonConvert.SerializeObject(proc_result));
                        break;
                    default:

                    break;
                
                }


             

             
             


                Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                Apiflow_default_condition.Add("response", param_ins);
                api_status = "true";

             

            }
            catch (Exception e) {
                _ = log.swan_core_log("wealthapi", "Error : " + e.ToString());

                Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                Apiflow_default_condition.Add("response", "Invalid api");
                api_status = "fail";
            }


            var status_api = "";
            if (api_status == "Create")
            {
                status_api = "201";
            }
            else if (api_status == "true")
            {
                status_api = "200";
            }
            else if (api_status == "fail")
            {
                status_api = "400";
            }
            else if (api_status == " ")
            {
                status_api = "500";
            }

            return StatusCode(Int32.Parse(status_api), Apiflow_default_condition);

            // return StatusCode(200, wealth_default_condition);
            //return StatusCode(200, JsonConvert.SerializeObject(param_ins));
        }

        private async Task<string> apimap_wealth(string apicode)
        {
            Mysqlswan swan = new Mysqlswan();
            Mysqlhawk hawk = new Mysqlhawk();
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            var func_result = new Dictionary<object, object>();

            try
            {
                var command = "SELECT interface_key AS apikey , interface_map AS apivalue FROM mapzila_wealth WHERE apicode = @1 AND `status` = 'Y' ORDER BY sorting ASC";
                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("1", apicode);

                var data_8585 = await swan.data_with_col(command, param);
                var data_8586 = JsonConvert.SerializeObject(data_8585);

                var data_8587 = JArray.Parse(data_8586);
                for (int i = 0; i < data_8587.Count; i++)
                {

                    func_result.Add(data_8587[i]["apikey"].ToString(), data_8587[i]["apivalue"].ToString());
                }
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("apimap_wealth", "Error : " + e.ToString());
            }


            return JsonConvert.SerializeObject(func_result);
        }


        // PUT: api/App1/5
       // [HttpPut("{id}")]
        public async Task<IActionResult> Put()
        {


            try
            {
                Mysqlswan swan = new Mysqlswan();

                var body = "";
                using (var mem = new MemoryStream())
                using (var reader = new StreamReader(mem))
                {
                    Request.Body.CopyTo(mem);
                    body = reader.ReadToEnd();
                    mem.Seek(0, SeekOrigin.Begin);
                    body = reader.ReadToEnd();
                }
                _ = log.swan_core_log("wealthapi", "log PUT Wealth : " + body.ToString());
                var request_data = JObject.Parse(body);
                _ = log.swan_core_log("wealthapi", "log Parser Wealth : " + request_data.ToString());

                var mappingdata = await apimap_wealth(request_data["apiname"].ToString());
                var param_ins = new Dictionary<object, object>();
                var ps_mappingdata = JObject.Parse(mappingdata);
                foreach (var item in ps_mappingdata)
                {
                    param_ins.Add(item.Value.ToString(), request_data[item.Key.ToString()].ToString());
                }
                var param_condition = new Dictionary<object, object>();
                param_condition.Add("hawkid", request_data["hawkid"].ToString());

                var upd_data = swan.data_update(request_data["apiname"].ToString().Replace("upd_", ""), JsonConvert.SerializeObject(param_ins), JsonConvert.SerializeObject(param_condition));


            }
            catch (Exception e)
            {
                _ = log.swan_core_log("wealthapi", "Error : " + e.ToString());
            }

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

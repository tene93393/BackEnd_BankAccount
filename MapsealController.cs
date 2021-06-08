using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace service.Controllers
{
   
    public class MapsealController : Controller
    {


        // GET: /<controller>/
        public async Task<IActionResult> Index(string mode, string param1, string param2, string param3, string param4, string param5, string param6)
        {
            Logmodel log = new Logmodel();
            Mysqlwealth wealth = new Mysqlwealth();
            var mapdata_return = new Dictionary<object, object>();
            var rs_20200417 = new List<object>();
            var patten_map = "";

            var mapdata = new Dictionary<object, object>();
            mapdata.Add("param1", param1);
            mapdata.Add("param2", param2);
            mapdata.Add("param3", param3);
            mapdata.Add("param4", param4);
            mapdata.Add("param5", param5);
            mapdata.Add("param6", param6);

            try
            {
                patten_map = module_cfg(mode, "patten_map");

                _ = log.swan_core_log("Wealth_sync", " Seal_data Mapdata Mapzila ===============================================================> ");
                _ = log.swan_core_log("Wealth_sync", " map petten: " + patten_map.ToString());
                _ = log.swan_core_log("Wealth_sync", " Param : " + JsonConvert.SerializeObject(mapdata));

                _ = log.swan_core_log("Wealth_sync", " Param New map : " + JsonConvert.SerializeObject(await mapping_interface(mode, mapdata)));

                var wealth_custinfo = await wealthmodel(patten_map);
                var wealth_sealdata = await Sync_seal_customer(await mapping_interface(mode, mapdata));
              
               // _ = log.swan_core_log("Wealth_sync", " Seal_data Mapdata Mapzila===> : " + wealth_custinfo.ToString());
                //_ = log.swan_core_log("Wealth_sync", " Seal_data Mapdata api interface ===> : " + wealth_sealdata.ToString());

                var w_md20200422 = JObject.Parse(wealth_custinfo);
                var w_mp20200434 = JArray.Parse(wealth_sealdata);
                
                for (int i = 0; i < w_mp20200434.Count; i++)
                {
                    // _ = log.swan_core_log("Wealth_Customer_Central", " Ready to Parse : " + temp_data[i].ToString());
                    var temp_rs_20200417 = new Dictionary<object, object>();
                    var tms_20200417 = JObject.Parse(w_mp20200434[i].ToString());

                   // _ = log.swan_core_log("Wealth_sync", " Seal_data Raw temp : " + JsonConvert.SerializeObject(tms_20200417));
                    foreach (var item in tms_20200417)
                    {
                        
                        _ = log.swan_core_log("Wealth_sync", " Seal_data  Map Value : " + item.Key.ToString() + "|==>" +w_md20200422[item.Key.ToString()].ToString()+" : "+ item.Value.ToString());
                         temp_rs_20200417.Add(w_md20200422[item.Key.ToString()].ToString(), item.Value.ToString());
                       
                    }

                    var ins_data = wealth.data_ins(patten_map, JsonConvert.SerializeObject(temp_rs_20200417));
                    _ = log.swan_core_log("Wealth_sync", " Inse to api : " + JsonConvert.SerializeObject(ins_data));

                    rs_20200417.Add(temp_rs_20200417);
                }

                _ = log.swan_core_log("Wealth_sync", " Seal_data Mapdata Mapzila ===============================================================> ");


            }
            catch (Exception e)
            {

                _ = log.swan_core_log("Wealth_sync", " Error Index : " + e.ToString());
            }


            return Json(rs_20200417);
        }


        private static async Task<Dictionary<object, object>> mapping_interface(string mode, Dictionary<object, object> mappperdata)
        {

            Logmodel log = new Logmodel();
            var response_array = new Dictionary<object, object>();
            var config_map_interface = module_cfg(mode, "api");
            var mp1128 = JsonConvert.SerializeObject(mappperdata);
            var mp1129 = JObject.Parse(mp1128);
            var return_data = config_map_interface;
            foreach (var item in mp1129)
            {

                return_data = replace_value(return_data, "{{" + item.Key.ToString() + "}}", item.Value.ToString());
            }
            var mp1131 = JObject.Parse(return_data);
            foreach (var item in mp1131)
            {
                response_array.Add(item.Key.ToString(), item.Value.ToString());
            }
            //_ = log.swan_core_log("mapinterface_seal", " New Convert Value " + JsonConvert.SerializeObject(response_array));

            return response_array;
        }

        private static string replace_value(string txt, string patten, string replace)
        {

            return txt.Replace(patten, replace);
        }


        public static string module_cfg(string variable1, string variable2)
        {
            string path = Directory.GetCurrentDirectory();
            string configtext = System.IO.File.ReadAllText(path + "/Data/Wealth_Seal_cfg.json");
            JObject config_parse = JObject.Parse(configtext);
            return config_parse[variable1][variable2].ToString();
        }



        public async Task<string> Sync_seal_customer(Dictionary<object, object> sealvariable)
        {

            Logmodel log = new Logmodel();

            var rs_20200417 = new List<object>();
            try
            {
                var request_data = new Dictionary<string, string>();
                var se4533334 = await Wealth_Customer_Central.interface_andaman_seal(JsonConvert.SerializeObject(sealvariable));
               // _ = log.swan_core_log("Wealth_sync", "api data : " + JsonConvert.SerializeObject(se4533334));
                var temp_4545 = JsonConvert.SerializeObject(se4533334);
                var temp_4646 = JObject.Parse(temp_4545);
                if (temp_4646["status"].ToString() != "404")
                {
                    JArray temp_data = JArray.Parse(temp_4646["status"].ToString());
                    for (int i = 0; i < temp_data.Count; i++)
                    {
                        // _ = log.swan_core_log("Wealth_Customer_Central", " Ready to Parse : " + temp_data[i].ToString());

                        var temp_rs_20200417 = new Dictionary<object, object>();
                        var tms_20200417 = JObject.Parse(temp_data[i].ToString());
                        foreach (var item in tms_20200417)
                        {
                            temp_rs_20200417.Add(item.Key.ToString(), item.Value.ToString());
                        }

                        rs_20200417.Add(temp_rs_20200417);
                    }
                }
                else
                {
                   // _ = log.swan_core_log("Wealth_sync", "Sample parser status: " + temp_4646["status"].ToString());
                }
            }
            catch (Exception e)
            {

                _ = log.swan_core_log("Wealth_sync", " Error Sending data: " + e.ToString());
            }
           // _ = log.swan_core_log("Wealth_sync", " Reutrn value " + JsonConvert.SerializeObject(rs_20200417));
            //await Mail_helper.mail_monitor_wealth_with_message("Import Success", JsonConvert.SerializeObject(rs_20200417));
            return JsonConvert.SerializeObject(rs_20200417);

        }

        public async Task<string> wealthmodel(string apicode)
        {

            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            Mysqlswan swan = new Mysqlswan();
            Mysqlhawk hawk = new Mysqlhawk();
            Mysqlwealth wealth = new Mysqlwealth();
            var mapdata_return = new Dictionary<object, object>();

            _ = log.swan_core_log("Wealth_sync", "tbl : " + apicode.ToString());
            try
            {

                var command = "SELECT interface_key AS apikey , interface_map AS apivalue from mapzila_wealth WHERE apicode = @1 AND `status` = 'Y' ORDER BY sorting ASC";
                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("1", apicode);
                var data_8585 = await swan.data_with_col(command, param);
                var data_8586 = JsonConvert.SerializeObject(data_8585);
                var data_8587 = JArray.Parse(data_8586);
                for (int i = 0; i < data_8587.Count; i++)
                {
                    //_ = log.swan_core_log("Wealth_sync", "Key : " + data_8587[i]["apikey"].ToString());
                    //_ = log.swan_core_log("Wealth_sync", "value : " + data_8587[i]["apivalue"].ToString());


                    mapdata_return.Add(data_8587[i]["apikey"].ToString(), data_8587[i]["apivalue"].ToString());
                }
               // _ = log.swan_core_log("Wealth_sync", "Prepare_data : " + JsonConvert.SerializeObject(mapdata_return));
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("Wealth_sync", "Error" + e.ToString());
            }
            return JsonConvert.SerializeObject(mapdata_return);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

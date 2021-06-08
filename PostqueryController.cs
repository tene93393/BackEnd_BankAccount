using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;

namespace service.Controllers.Wealthline
{
    public class PostqueryController : Controller
    {
        Logmodel log = new Logmodel();
        Mysqlswan mysqlswan = new Mysqlswan();

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> post_query(string v1, string v2, string v3, string v4)
        {

            try
            {
                string sql = "CALL getDataOnQuery ('" + v1 + "','" + v2 + "','" + v3 + "','" + v4 + "')";

                //log.info("SQL: " + sql);

                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("", "");

                //log.info("Param :" + JsonConvert.SerializeObject(param));

                var data = await mysqlswan.data_with_col(sql, param);

                //log.info("post query : " + JsonConvert.SerializeObject(data));
                return Content(JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                log.info("post query Error: " + e.ToString());
                return StatusCode(404);

            }

        }

        public async Task<ActionResult> tmp_data(string v1, string v2, string v3, string v4, string v5, string v6, string v7)
        {
            Logmodel log = new Logmodel();
            Mysqlhawk mysqlhawk = new Mysqlhawk();
            try
            {
                string command = "update tmp_open_part1 SET customer_name = @2, remark1 = @3 WHERE flowid = @1";

                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("1", v1);
                param.Add("2", v2);
                param.Add("3", v3);

                var data = await mysqlhawk.data_with_col(command, param);

                return Content(JsonConvert.SerializeObject(data));
            }
            catch(Exception e)
            {
                log.info("tmp_data Error: " + e.ToString());
                return StatusCode(404);
            }

            
        }
        
        public async Task<ActionResult> get_codelookup(string lookupname)
        {
            Dictionary<object, object> lookup_arr = new Dictionary<object, object>();
            if (lookupname == "")
            {
                lookup_arr.Add("", "");
            }
            else if (lookupname == "ThaiNationCode")
            {
                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/ThaiNationCode.json");
                JArray temp_data = JArray.Parse(temp_data_file);
                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            else if (lookupname == "Amphur")
            {

                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/Amphur.json");
                JArray temp_data = JArray.Parse(temp_data_file);
                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            else if (lookupname == "Province")
            {
                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/Province.json");
                JArray temp_data = JArray.Parse(temp_data_file);
                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            else if (lookupname == "Tambol")
            {

                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/Tambol.json");
                JArray temp_data = JArray.Parse(temp_data_file);

                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            else if (lookupname == "Zipcode")
            {

                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/Zipcode.json");
                JArray temp_data = JArray.Parse(temp_data_file);

                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            else if (lookupname == "Bankbranch")
            {

                string path = Directory.GetCurrentDirectory();
                string temp_data_file = System.IO.File.ReadAllText(path + "/App_data/Lookup/Bankbranch.json");
                JArray temp_data = JArray.Parse(temp_data_file);

                foreach (var item in temp_data)
                {
                    lookup_arr.Add(item["keycode"].ToString(), item["keyvalue"].ToString());
                }

            }
            return Content(JsonConvert.SerializeObject(lookup_arr));
        }
    }
}
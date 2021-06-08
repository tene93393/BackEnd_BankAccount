using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Http;


namespace service.Controllers
{
   
    public class AngularController : Controller
    {
       
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

       
     
        [Proofpoint]
        public IActionResult Index()
        {
           
           // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            log.info("Session in page : " + value.ToString());
            log.info("Home Index");

            ViewBag.testconnect = "Welcome Page !!! " + JsonConvert.SerializeObject(config.initial_config("Data_vender"));


            return View("~/Views/web001/Angular/Index.cshtml");
        }

        [System.Web.Http.HttpPost]
        [Proofpoint]
        public string insert()
        {

            var ioutput = new Dictionary<string, object>();
            var body = "";
            var ierror = new List<object>();
            var imessage = new List<object>();
           //var ioutput = new Dictionary<object,object>();
            var val_parser = new Dictionary<object, object>();

            var json = "";

            var validation_error = "";
            //var first_name = "";
            //var last_name = "";


            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                 Request.Body.CopyTo(mem);
                 body = reader.ReadToEnd();
                 mem.Seek(0, SeekOrigin.Begin);
                 body = reader.ReadToEnd();               
            }

            log.info("All data post angular : Not send data" + body.ToString());


        
                 if (string.IsNullOrEmpty(body.ToString()))
                {
                    ierror.Add("data not found");
                   // imessage = "";
                    log.info("All data post angular : Not send data");
                 }
                else
                {



                    var jdata = JObject.Parse(body.ToString());
                    
                    log.info("All data post angular : " + body.ToString().ToString());
                    log.info("All data post angular parser : " + jdata.ToString().ToString());


                    foreach (var item in jdata)
                    {
                        val_parser.Add(item.Key.ToString(),item.Value.ToString());
                        log.info("parser ==>  : " + item.Key + " : " + item.Value);
                    }


                    log.info(" Inspect data :  "+val_parser["action"].ToString());


                if (val_parser["action"].ToString() == "fetch_single_data")
                {

                    string query = "SELECT * FROM tbl_sample WHERE id='" + jdata["id"].ToString() + "'";

                    Core_sqlite sqlite = new Core_sqlite();
                    //string command = "SELECT * FROM tbl_sample ORDER BY id";
                    var param = new Dictionary<object, object>();
                    param.Add("", "");
                    var result = sqlite.sqlite_pull(query, param);

                    log.info("Alldata : " + JsonConvert.SerializeObject(result));

                    var jresut = JArray.Parse(JsonConvert.SerializeObject(result));

                    ioutput.Add("first_name", jresut[0]["first_name"]);
                    ioutput.Add("last_name", jresut[0]["last_name"]);

                    //$output['first_name'] = $row['first_name'];
                    //$output['last_name'] = $row['last_name'];

                }
                else if (val_parser["action"].ToString() == "Delete")
                {

                    string query_delete = "DELETE FROM tbl_sample WHERE id = '" + jdata["id"].ToString() + "' ";

                    Core_sqlite sqlite = new Core_sqlite();
                    //string command = "SELECT * FROM tbl_sample ORDER BY id";
                    var param = new Dictionary<object, object>();
                    param.Add("", "");
                    var result = sqlite.sqlite_push(query_delete, param);

                    ioutput.Add("result", "Data Deleted");
                    // $output['message'] = 'Data Deleted';

                } else if(jdata["action"].ToString() == "Insert")
                    {

                    string query_ins = "INSERT INTO tbl_sample (first_name, last_name) VALUES ( '" + jdata["first_name"].ToString() + "', '" + jdata["last_name"].ToString() + "') ";

                    Core_sqlite sqlite = new Core_sqlite();
                    var param = new Dictionary<object, object>();
                    param.Add("", "");
                    var result = sqlite.sqlite_push(query_ins, param);

                    log.info("========= insert data  ===============");

                    imessage.Add("Data Inserted");


                }else if (jdata["action"].ToString() == "Edit"){


                    string query_upd = "UPDATE tbl_sample SET first_name = '" + jdata["first_name"].ToString() + "', last_name =  '" + jdata["last_name"].ToString() + "' WHERE id = '" + jdata["id"].ToString() + "' ";

                    Core_sqlite sqlite = new Core_sqlite();
                    var param = new Dictionary<object, object>();
                    param.Add("", "");
                    var result = sqlite.sqlite_push(query_upd, param);

                    log.info("========= Edit  data  ===============");


                    imessage.Add("Data Edited");



                }
                else
                {
    
                }



                var html_data = new Dictionary<string, string>();
                html_data.Add("method", "flow");
                html_data.Add("apiname", "sw02");
                html_data.Add("data", "");

                var data_level1 = JsonConvert.SerializeObject(html_data);
                
                log.info("ioutput ===>" + data_level1);



                ioutput.Add("error", validation_error);
                ioutput.Add("message", String.Join(", ", imessage.ToArray()));


                json = JsonConvert.SerializeObject(ioutput);

                log.info("ioutput ===>" + json);



            }






            //var ouput  =  new Dictionary<object, object>();
            //ioutput.Add("error", validation_error);
            //ioutput.Add("message", imessage.ToString());



            return json;

        }

        public List<object> fetch_data()
        {

            Core_sqlite sqlite = new Core_sqlite();
            string command = "SELECT * FROM tbl_sample ORDER BY id";
            var param = new Dictionary<object, object>();
            param.Add("", "");
            return sqlite.sqlite_pull(command, param);
        }


       
    }
}



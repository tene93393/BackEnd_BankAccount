using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;

using Microsoft.AspNetCore.Http;


namespace service.Controllers
{

    public class MatrixdataController : Controller
    {

        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();


        [Proofpoint]
        public async Task<List<object>> fetch_datasq(string api)
        {

               log.info("api get data id is : " + api);

                Core_sqlite sqlite = new Core_sqlite();
                string command = MapzilaAutoCrud.matrix_api_fetch_data(api);
                var param = new Dictionary<object, object>();
                param.Add("", "");
                return await Task.Run(() => sqlite.sqlite_pull(command, param));
          
        }


        [System.Web.Http.HttpPost]
        [Proofpoint]
        public string action()
        {
            string api = HttpContext.Request.Query["api"];


            log.info(" Action api is " + api);

            var ioutput = new Dictionary<string, object>();
            var body = "";
            var ierror = new List<object>();
            var imessage = new List<object>();
            var val_parser = new Dictionary<object, object>();

            var json = "";
            var validation_error = "";
          

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
                    val_parser.Add(item.Key.ToString(), item.Value.ToString());
                    log.info("parser ==>  : " + item.Key + " : " + item.Value);
                }


                log.info(" Inspect data :  " + val_parser["action"].ToString());
                if (val_parser["action"].ToString() == "fetch_single_data")
                {

                    string query = MapzilaAutoCrud.matrix_api_fetch_single_data(api); 
                    Core_sqlite sqlite = new Core_sqlite();
                    //string command = "SELECT * FROM tbl_sample ORDER BY id";
                    var param = new Dictionary<object, object>();
                    param.Add("1", jdata["id"].ToString());
                    var result = sqlite.sqlite_pull(query, param);

                    log.info("Alldata : " + JsonConvert.SerializeObject(result));

                    var jresut = JArray.Parse(JsonConvert.SerializeObject(result));


                    foreach(var item in MapzilaAutoCrud.frm_schema(api))
                    {                        
                        ioutput.Add(@item.Value.ToString(), jresut[0][@item.Value.ToString()]);
                    }

                    //ioutput.Add("first_name", jresut[0]["first_name"]);
                    //ioutput.Add("last_name", jresut[0]["last_name"]);

                }
                else if (val_parser["action"].ToString() == "Delete")
                {

                    string query_delete = MapzilaAutoCrud.matrix_api_delete_data(api);

                    Core_sqlite sqlite = new Core_sqlite();                
                    var param = new Dictionary<object, object>();
                    param.Add("1", jdata["id"].ToString());
                    var result = sqlite.sqlite_push(query_delete, param);

                    ioutput.Add("result", "Data Deleted");
                    imessage.Add("Data Deleted");
                    // $output['message'] = 'Data Deleted';

                }
                else if (jdata["action"].ToString() == "Insert")
                {

                    string query_ins = MapzilaAutoCrud.matrix_api_insert_data(api);
                    //"INSERT INTO tbl_sample (first_name, last_name) VALUES ( '" + jdata["first_name"].ToString() + "', '" + jdata["last_name"].ToString() + "') ";

                    Core_sqlite sqlite = new Core_sqlite();
                    var param = new Dictionary<object, object>();

                    foreach (var item in MapzilaAutoCrud.frm_schema(api))
                    {
                        param.Add(@item.Key.ToString(), jdata[@item.Value.ToString()].ToString());
                    }

                    //param.Add("1", jdata["first_name"].ToString());
                    //param.Add("2", jdata["last_name"].ToString());

                    var result = sqlite.sqlite_push(query_ins, param);

                    log.info("========= insert data  ===============");

                    imessage.Add("Data Inserted");
                }
                else if (jdata["action"].ToString() == "Edit")
                {
                    string query_upd = MapzilaAutoCrud.matrix_api_Update_data(api);
                    //"UPDATE tbl_sample SET first_name = '" + jdata["first_name"].ToString() + "', last_name =  '" + jdata["last_name"].ToString() + "' WHERE id = '" + jdata["id"].ToString() + "' ";

                    Core_sqlite sqlite = new Core_sqlite();
                    var param = new Dictionary<object, object>();
                    foreach (var item in MapzilaAutoCrud.frm_schema(api))
                    {
                        param.Add(@item.Key.ToString(), jdata[@item.Value.ToString()].ToString());
                    }

                    param.Add("id", jdata["id"].ToString());

                    var result = sqlite.sqlite_push(query_upd, param);
                    log.info("========= Edit  data  ===============");
                    imessage.Add("Data Edited");
                }
                else
                {

                }
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

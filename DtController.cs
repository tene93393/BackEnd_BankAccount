using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;

namespace service.Controllers
{
    public class DtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadData() {

            var draw = Request.Form["draw"].ToString();
            var start = Request.Form["start"].ToString();
            var length = Request.Form["length"].ToString();
            return Content(Datatabledm_model.data_datatable(draw, start, length));


        }

        //[Proofpoint]
        public async Task<IActionResult> Loadwip()
        {
            Logmodel log = new Logmodel();
            var value_token = HttpContext.Session.GetString("FirstSeen");
            var userid = await Client_info.info_logon(value_token, "userid");

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

          

            var page = "wip";
            var draw = this.Request.Form["draw"].ToString();
            var start = this.Request.Form["start"].ToString();
            var length = this.Request.Form["length"].ToString();


            //var url_data_fillter = HttpUtility.UrlDecode(body);
            Dictionary<string, string> url_data_fillter = new Dictionary<string, string>();
            string[] searchParams = HttpUtility.UrlDecode(body).Split('&');
            foreach (string param in searchParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                url_data_fillter.Add(key, value);
            }

            var json_url_data_fillter = JsonConvert.SerializeObject(url_data_fillter);
            _ = log.debug("search wip : ALL :" + JsonConvert.SerializeObject(url_data_fillter));






            //{ "draw":"3",
            //    "columns[0][data]":"status",
            //    "columns[0][name]":"status",
            //    "columns[0][searchable]":"true",
            //    "columns[0][orderable]":"false",
            //    "columns[0][search][value]":"",
            //    "columns[0][search][regex]":"false",
            //    "columns[1][data]":"ReqID",
            //    "columns[1][name]":"ReqID",
            //    "columns[1][searchable]":"true",
            //    "columns[1][orderable]":"false",
            //    "columns[1][search][value]":"77",
            //    "columns[1][search][regex]":"false",
            //    "columns[2][data]":"formname",
            //    "columns[2][name]":"formname",
            //    "columns[2][searchable]":"true",
            //    "columns[2][orderable]":"false",
            //    "columns[2][search][value]":"",
            //    "columns[2][search][regex]":"false",
            //    "columns[3][data]":"title",
            //    "columns[3][name]":"title",
            //    "columns[3][searchable]":"true",
            //    "columns[3][orderable]":"false",
            //    "columns[3][search][value]":"",
            //    "columns[3][search][regex]":"false",
            //    "columns[4][data]":"current_status",
            //    "columns[4][name]":"current_status",
            //    "columns[4][searchable]":"true",
            //    "columns[4][orderable]":"false",
            //    "columns[4][search][value]":"",
            //    "columns[4][search][regex]":"false",
            //    "columns[5][data]":"Requester",
            //    "columns[5][name]":"Requester",
            //    "columns[5][searchable]":"true",
            //    "columns[5][orderable]":"false",
            //    "columns[5][search][value]":"",
            //    "columns[5][search][regex]":"false",
            //    "columns[6][data]":"Sender",
            //    "columns[6][name]":"Sender",
            //    "columns[6][searchable]":"true",
            //    "columns[6][orderable]":"false",
            //    "columns[6][search][value]":"",
            //    "columns[6][search][regex]":"false",
            //    "columns[7][data]":"lastupdated",
            //    "columns[7][name]":"lastupdated",
            //    "columns[7][searchable]":"true",
            //    "columns[7][orderable]":"false",
            //    "columns[7][search][value]":"",
            //    "columns[7][search][regex]":"false",
            //    "columns[8][data]":"button",
            //    "columns[8][name]":"button",
            //    "columns[8][searchable]":"true",
            //    "columns[8][orderable]":"false",
            //    "columns[8][search][value]":"",
            //    "columns[8][search][regex]":"false",
            //    "start":"0",
            //    "length":"10",
            //    "search[value]":"",
            //    "search[regex]":"false"
            // }




            //    "columns[0][search][value]":"",

            //    "columns[1][search][value]":"77",

            //    "columns[2][search][value]":"",

            //    "columns[3][search][value]":"",

            //    "columns[4][search][value]":"",

            //    "columns[5][search][value]":"",

            //    "columns[6][search][value]":"",

            //    "columns[7][search][value]":"",

            //    "columns[8][search][value]":"",





            var list_url_data_fillter = JObject.Parse(json_url_data_fillter);
            List<object> itemsearch = new List<object>();
            Dictionary<object, object> paramsearch = new Dictionary<object, object>();

            paramsearch.Add("status", list_url_data_fillter["columns[0][search][value]"]);
            paramsearch.Add("flowid", list_url_data_fillter["columns[1][search][value]"]);
            paramsearch.Add("form", list_url_data_fillter["columns[2][search][value]"]);
            paramsearch.Add("title", list_url_data_fillter["columns[3][search][value]"]);
            paramsearch.Add("present_state", list_url_data_fillter["columns[4][search][value]"]);
            paramsearch.Add("requester", list_url_data_fillter["columns[5][search][value]"]);
            paramsearch.Add("last_user", list_url_data_fillter["columns[6][search][value]"]);
            paramsearch.Add("lastupdated", list_url_data_fillter["columns[7][search][value]"]);
            paramsearch.Add("button", list_url_data_fillter["columns[8][search][value]"]);



            Dictionary<object, object> postparam = new Dictionary<object, object>();


            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = HttpUtility.UrlDecode(kvPair[0].Replace("columns", ""));
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);

                if (key.Contains("draw"))
                {
                    draw = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("start"))
                {
                    start = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("length"))
                {
                    length = HttpUtility.UrlDecode(kvPair[1]);
                }
            }


            var count_column = JsonConvert.SerializeObject(postparam);
            Dictionary<object, object> searchkey = new Dictionary<object, object>();

            //return Content( await Swan_flow_data.sql_data_datatable(draw, start, length, page, userid));
            return Content("");

        }





        //[Proofpoint]
        public async Task<IActionResult> Loadcc()
        {
            Logmodel log = new Logmodel();

            var value_token = HttpContext.Session.GetString("FirstSeen");
            var userid = await Client_info.info_logon(value_token, "userid");

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            

            var page = "cc";
            var draw = "";
            var start = "";
            var length = "";

            Dictionary<string, string> url_data_fillter = new Dictionary<string, string>();
            string[] searchParams = HttpUtility.UrlDecode(body).Split('&');
            foreach (string param in searchParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                url_data_fillter.Add(key, value);
            }

            var json_url_data_fillter = JsonConvert.SerializeObject(url_data_fillter);
            _ = log.debug("search wip : ALL :" + JsonConvert.SerializeObject(url_data_fillter));
            var list_url_data_fillter = JObject.Parse(json_url_data_fillter);
            List<object> itemsearch = new List<object>();
            Dictionary<object, object> paramsearch = new Dictionary<object, object>();

            paramsearch.Add("status", list_url_data_fillter["columns[0][search][value]"]);
            paramsearch.Add("flowid", list_url_data_fillter["columns[1][search][value]"]);
            paramsearch.Add("form", list_url_data_fillter["columns[2][search][value]"]);
            paramsearch.Add("title", list_url_data_fillter["columns[3][search][value]"]);
            paramsearch.Add("present_state", list_url_data_fillter["columns[4][search][value]"]);
            paramsearch.Add("requester", list_url_data_fillter["columns[5][search][value]"]);
            paramsearch.Add("last_user", list_url_data_fillter["columns[6][search][value]"]);
            paramsearch.Add("lastupdated", list_url_data_fillter["columns[7][search][value]"]);
            paramsearch.Add("button", list_url_data_fillter["columns[8][search][value]"]);




 
            Dictionary<object, object> postparam = new Dictionary<object, object>();
            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = HttpUtility.UrlDecode(param.Replace("columns", "")).Split('=');

                if (kvPair[0].Contains("[name]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
                if (kvPair[0].Contains("[search][value]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
            } 
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = HttpUtility.UrlDecode(kvPair[0].Replace("columns", ""));
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);

                if (key.Contains("draw"))
                {
                    draw = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("start"))
                {
                    start = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("length"))
                {
                    length = HttpUtility.UrlDecode(kvPair[1]);
                }
            }

            var count_column = JsonConvert.SerializeObject(postparam);
            Dictionary<object, object> searchkey = new Dictionary<object, object>();

           /// return Content(await Swan_flow_data.sql_data_datatable(draw, start, length, page, userid));
            return Content("");

        }
        //[Proofpoint]
        public async Task<IActionResult> Loadcompelete()
        {
         
            Logmodel log = new Logmodel();

            var value_token = HttpContext.Session.GetString("FirstSeen");
            var userid = await Client_info.info_logon(value_token, "userid");

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }



            var page = "compelete";
            var draw = "";
            var start = "";
            var length = "";


            Dictionary<string, string> url_data_fillter = new Dictionary<string, string>();
            string[] searchParams = HttpUtility.UrlDecode(body).Split('&');
            foreach (string param in searchParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                url_data_fillter.Add(key, value);
            }

            var json_url_data_fillter = JsonConvert.SerializeObject(url_data_fillter);
            _ = log.debug("search wip : ALL :" + JsonConvert.SerializeObject(url_data_fillter));
            var list_url_data_fillter = JObject.Parse(json_url_data_fillter);
            List<object> itemsearch = new List<object>();
            Dictionary<object, object> paramsearch = new Dictionary<object, object>();

            paramsearch.Add("status", list_url_data_fillter["columns[0][search][value]"]);
            paramsearch.Add("flowid", list_url_data_fillter["columns[1][search][value]"]);
            paramsearch.Add("form", list_url_data_fillter["columns[2][search][value]"]);
            paramsearch.Add("title", list_url_data_fillter["columns[3][search][value]"]);
            paramsearch.Add("present_state", list_url_data_fillter["columns[4][search][value]"]);
            paramsearch.Add("requester", list_url_data_fillter["columns[5][search][value]"]);
            paramsearch.Add("last_user", list_url_data_fillter["columns[6][search][value]"]);
            paramsearch.Add("lastupdated", list_url_data_fillter["columns[7][search][value]"]);
            paramsearch.Add("button", list_url_data_fillter["columns[8][search][value]"]);

            Dictionary<object, object> postparam = new Dictionary<object, object>();


            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = HttpUtility.UrlDecode(param.Replace("columns", "")).Split('=');

                if (kvPair[0].Contains("[name]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
                if (kvPair[0].Contains("[search][value]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
            }


            List<object> temp = new List<object>();
            for (int i = 0; i < itemsearch.Count; ++i)
            {
                temp.Add(itemsearch[i]);
                if (temp.Count.ToString() == "2")
                {
                    paramsearch.Add(temp[0], temp[1]);
                    temp.Clear();
                }
            }

            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = HttpUtility.UrlDecode(kvPair[0].Replace("columns", ""));
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);

                if (key.Contains("draw"))
                {
                    draw = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("start"))
                {
                    start = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("length"))
                {
                    length = HttpUtility.UrlDecode(kvPair[1]);
                }
            }


            var count_column = JsonConvert.SerializeObject(postparam);
            Dictionary<object, object> searchkey = new Dictionary<object, object>();

           // return Content(await Swan_flow_data.sql_data_datatable(draw, start, length, page, userid));
            return Content("");
        }

       // [Proofpoint]
        public async Task<IActionResult> Loadcancel()
        {
           
            Logmodel log = new Logmodel();

            var value_token = HttpContext.Session.GetString("FirstSeen");
            var userid = await Client_info.info_logon(value_token, "userid");

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

          

            var page = "cancel";
          
            var draw = "";
            var start = "";
            var length = "";

            Dictionary<string, string> url_data_fillter = new Dictionary<string, string>();
            string[] searchParams = HttpUtility.UrlDecode(body).Split('&');
            foreach (string param in searchParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                url_data_fillter.Add(key, value);
            }

            var json_url_data_fillter = JsonConvert.SerializeObject(url_data_fillter);
            _ = log.debug("search wip : ALL :" + JsonConvert.SerializeObject(url_data_fillter));
            var list_url_data_fillter = JObject.Parse(json_url_data_fillter);
            List<object> itemsearch = new List<object>();
            Dictionary<object, object> paramsearch = new Dictionary<object, object>();

            paramsearch.Add("status", list_url_data_fillter["columns[0][search][value]"]);
            paramsearch.Add("flowid", list_url_data_fillter["columns[1][search][value]"]);
            paramsearch.Add("form", list_url_data_fillter["columns[2][search][value]"]);
            paramsearch.Add("title", list_url_data_fillter["columns[3][search][value]"]);
            paramsearch.Add("present_state", list_url_data_fillter["columns[4][search][value]"]);
            paramsearch.Add("requester", list_url_data_fillter["columns[5][search][value]"]);
            paramsearch.Add("last_user", list_url_data_fillter["columns[6][search][value]"]);
            paramsearch.Add("lastupdated", list_url_data_fillter["columns[7][search][value]"]);
            paramsearch.Add("button", list_url_data_fillter["columns[8][search][value]"]);

            Dictionary<object, object> postparam = new Dictionary<object, object>();


            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = HttpUtility.UrlDecode(param.Replace("columns", "")).Split('=');

                if (kvPair[0].Contains("[name]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
                if (kvPair[0].Contains("[search][value]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
            }
            

            List<object> temp = new List<object>();
            for (int i = 0; i < itemsearch.Count; ++i)
            {
                temp.Add(itemsearch[i]);
                if (temp.Count.ToString() == "2")
                {
                    paramsearch.Add(temp[0], temp[1]);
                    temp.Clear();
                }
            }
           
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = HttpUtility.UrlDecode(kvPair[0].Replace("columns", ""));
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);

                if (key.Contains("draw"))
                {
                    draw = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("start"))
                {
                    start = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("length"))
                {
                    length = HttpUtility.UrlDecode(kvPair[1]);
                }
            }


           

            var count_column = JsonConvert.SerializeObject(postparam);
            Dictionary<object, object> searchkey = new Dictionary<object, object>();

            //return Content(await Swan_flow_data.sql_data_datatable(draw, start, length, page, userid));
            return Content("");

        }
        //[Proofpoint]
        public async Task<IActionResult> loaddraft()
        {
           
            Logmodel log = new Logmodel();

            var value_token = HttpContext.Session.GetString("FirstSeen");
            var userid = await Client_info.info_logon(value_token, "userid");

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

           

            var page = "draft";
            var draw = "";
            var start = "";
            var length = "";

            Dictionary<string, string> url_data_fillter = new Dictionary<string, string>();
            string[] searchParams = HttpUtility.UrlDecode(body).Split('&');
            foreach (string param in searchParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                url_data_fillter.Add(key, value);
            }

            var json_url_data_fillter = JsonConvert.SerializeObject(url_data_fillter);
            _ = log.debug("search wip : ALL :" + JsonConvert.SerializeObject(url_data_fillter));
            var list_url_data_fillter = JObject.Parse(json_url_data_fillter);
            List<object> itemsearch = new List<object>();
            Dictionary<object, object> paramsearch = new Dictionary<object, object>();

            paramsearch.Add("status", list_url_data_fillter["columns[0][search][value]"]);
            paramsearch.Add("flowid", list_url_data_fillter["columns[1][search][value]"]);
            paramsearch.Add("form", list_url_data_fillter["columns[2][search][value]"]);
            paramsearch.Add("title", list_url_data_fillter["columns[3][search][value]"]);
            paramsearch.Add("present_state", list_url_data_fillter["columns[4][search][value]"]);
            paramsearch.Add("requester", list_url_data_fillter["columns[5][search][value]"]);
            paramsearch.Add("last_user", list_url_data_fillter["columns[6][search][value]"]);
            paramsearch.Add("lastupdated", list_url_data_fillter["columns[7][search][value]"]);
            paramsearch.Add("button", list_url_data_fillter["columns[8][search][value]"]);

            Dictionary<object, object> postparam = new Dictionary<object, object>();


            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = HttpUtility.UrlDecode(param.Replace("columns", "")).Split('=');

                if (kvPair[0].Contains("[name]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
                if (kvPair[0].Contains("[search][value]"))
                {
                    itemsearch.Add(kvPair[1]);
                }
            }
           

            List<object> temp = new List<object>();
            for (int i = 0; i < itemsearch.Count; ++i)
            {
                temp.Add(itemsearch[i]);
                if (temp.Count.ToString() == "2")
                {
                    paramsearch.Add(temp[0], temp[1]);
                    temp.Clear();
                }
            }
            
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = HttpUtility.UrlDecode(kvPair[0].Replace("columns", ""));
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);

                if (key.Contains("draw"))
                {
                    draw = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("start"))
                {
                    start = HttpUtility.UrlDecode(kvPair[1]);

                }
                if (key.Contains("length"))
                {
                    length = HttpUtility.UrlDecode(kvPair[1]);
                }
            }


            var count_column = JsonConvert.SerializeObject(postparam);
            Dictionary<object, object> searchkey = new Dictionary<object, object>();

            //return Content(await Swan_flow_data.sql_data_datatable(draw, start, length, page, userid));
            return Content("");

        }
    }
}
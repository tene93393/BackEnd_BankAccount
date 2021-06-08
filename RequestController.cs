using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace service.Controllers
{
   
    public class RequestController : Controller
    {
      
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();
        private static Dictionary<object, object> return_swan_logic;


        public static string Command { get; private set; }
        public static string Command1 { get; private set; }
        public static string Command2 { get; private set; }
        public static string Command3 { get; private set; }
        public static string Command4 { get; private set; }
        public static string Command5 { get; private set; }

        public static Dictionary<object, object> param { get; private set; }
        public static Dictionary<object, object> param1 { get; private set; }
        public static Dictionary<object, object> param2 { get; private set; }
        public static Dictionary<object, object> param3 { get; private set; }
        public static Dictionary<object, object> param4 { get; private set; }
        public static Dictionary<object, object> param5 { get; private set; }



        [Proofpoint]
      
       
        public async Task<IActionResult> Index(string formid, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            Stopwatch timefunction = new Stopwatch();
            timefunction.Start();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            var task_userid = await Client_info.info_logon(value, "userid");

            var form = await formdetail(system);
            var state = await statedetail(system);
            var buton = JsonConvert.SerializeObject(await get_button(system));

            ViewBag.formid = system;
            ViewBag.form_th_name = form["form_th_name"].ToString();
            ViewBag.form_en_name = form["form_en_name"].ToString();
            ViewBag.form_revision = form["revision"].ToString();

            ViewBag.stateno = state["stateno"].ToString();
            ViewBag.statename = state["statename"].ToString();

            ViewBag.templatecode = formid;
            ViewBag.system = "SWAN";

            ViewBag.access_token = await Client_info.info_logon(value, "current_token");
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.project_ref = "000";

           
            ViewBag.email = await Client_info.info_logon(value, "email");
            ViewBag.en_name = await Client_info.info_logon(value, "en_name");
            ViewBag.th_name = await Client_info.info_logon(value, "th_name");

           
            ViewBag.user_type = await Client_info.info_logon(value, "user_type");
            ViewBag.accesslevel = await Client_info.info_logon(value, "accesslevel");
            ViewBag.role = await Client_info.info_logon(value, "role");
            ViewBag.marketingid = await Client_info.info_logon(value, "marketingid");
            ViewBag.traderid = await Client_info.info_logon(value, "traderid");
            ViewBag.branch = await Client_info.info_logon(value, "branch");
            ViewBag.groupcode = await Client_info.info_logon(value, "groupcode");

            ViewBag.state = state["stateno"].ToString();
            ViewBag.formid = system;
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            ViewBag.button = buton;

            timefunction.Stop();

            _ = log.swan_core_log("Time_exeute", "Request Controller Time : {"+ system + "}" + timefunction.Elapsed.TotalMilliseconds);

            return View("~/Views/web001/Workflow/Request.cshtml");
        }
        [Proofpoint]
        public async  Task<ActionResult> Requestflow()
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();

            var sess_token = HttpContext.Session.GetString("FirstSeen");
            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            //log.info("Log Post Requestflow : " + body.ToString());

            Dictionary<string, string> postparam = new Dictionary<string, string>();
            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);
            }

            //var detail_request = JObject.Parse(JsonConvert.SerializeObject(postparam));

            log.workflow("Log Request Flow ========================= >  :  "+JsonConvert.SerializeObject(postparam));


            return_swan_logic = new Dictionary<object, object>();


            try
            {
                var status_request = await Trigger_action.trigger_request_flow(sess_token,JsonConvert.SerializeObject(postparam));

                //   Add to trigger action
                // -- require userid , formid , flow , state , action ,title , branch





                //   End  To Trigger action


                // Waite for Dev login // state

                return_swan_logic.Add("result", "Create Flow Done!");
            }
            catch (Exception e)
            {
                return_swan_logic.Add("result", "Error : " + e.ToString());
                log.workflow("Error state request : " + e.ToString());
            }

            return StatusCode(200);
        }

        private async Task<Dictionary<object, object>> get_button(string formid)
        {
            Logmodel log = new Logmodel();
            Mysqlswan mysqlswan = new Mysqlswan();

            //////////// Get button String //////////
            string Command_button = "SELECT to_stateno,action_condition FROM stateedge ";
            Command_button += "WHERE from_formid = @formid AND from_stateno = 0";

            Dictionary<object, object> param_button = new Dictionary<object, object>();
            param_button.Add("formid", formid);

            var result_button = await mysqlswan.data_with_col(Command_button, param_button);
            JArray temp_data2 = JArray.Parse(JsonConvert.SerializeObject(result_button));

            log.info("Button Request: " + temp_data2.ToString());

            var item_arr_output = new Dictionary<object, object>();
            int count = 0;
            foreach (var item in temp_data2)
            {
                var item_arr = new Dictionary<object, object>();
                item_arr.Add("stateno", item["to_stateno"].ToString());
                item_arr.Add("action_condition", item["action_condition"].ToString());
                item_arr_output.Add(count, item_arr);
                count = count + 1;
            }
            return item_arr_output;
        }

       
        private async Task<Dictionary<object, object>> formdetail(string formid) {

            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();
            
            string Command = "select * from form WHERE formid = @form";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("form", formid);
            var result = await mysqlswan.data_with_col(Command, param);


            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("form_th_name", item["form_th_name"].ToString());
                item_arr.Add("form_en_name", item["form_en_name"].ToString());
                item_arr.Add("revision", item["revision"].ToString());
            }
           
            return item_arr;
        }

        private async Task<Dictionary<object, object>> statedetail(string formid)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

            string Command = " SELECT * from state WHERE formid = @form AND stateno = @state_no";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("form", formid);
            param.Add("state_no", "0");
            var result = await mysqlswan.data_with_col(Command, param);

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("stateno", item["stateno"].ToString());
                item_arr.Add("statename", item["statename"].ToString());
               
            }
           
            return item_arr;
        }

        [Proofpoint]
        public IActionResult From(string formid, string system)
        {
            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = formid;
            ViewBag.system = system;

            return View("~/Views/web001/Request/Request.cshtml");
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

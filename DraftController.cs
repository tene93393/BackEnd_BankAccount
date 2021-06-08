using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace service.Controllers
{
   
    public class DraftController : Controller
    {

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

        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();


        [Proofpoint]
        public async Task<IActionResult> Index(string flowid)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");
            var sess_token = HttpContext.Session.GetString("FirstSeen");

            var task_userid = await Client_info.info_logon(sess_token, "userid");

            Dictionary<object,object> systemid = await getflowdetail(flowid);

            var findstate = await findState(flowid);
            var form = await  formdetail(systemid["formid"].ToString());
            var state = await statedetail(systemid["formid"].ToString(), findstate["present_state"].ToString());
            var button = JsonConvert.SerializeObject(get_button(flowid));
            var Info = await getInfoDetail(flowid);



            ViewBag.formkey =  systemid["formid"].ToString();
            ViewBag.form_th_name = form["form_th_name"].ToString();
            ViewBag.form_en_name = form["form_en_name"].ToString();
            ViewBag.form_revision = form["revision"].ToString();
            ViewBag.button = button;
            ViewBag.userid = await Client_info.info_logon(sess_token, "userid");
            ViewBag.flowid = flowid;
            ViewBag.title_flow = systemid["last_title"].ToString();
            ViewBag.comment = systemid["comment_state"].ToString();

            ViewBag.formname = Info["form_th_name"].ToString();
            ViewBag.current_status = Info["statename"].ToString();
            ViewBag.requestor = Info["starter"].ToString();
            ViewBag.stateDate = Info["start_date"].ToString();
            ViewBag.lastUser = Info["last_user"].ToString();
            ViewBag.lastUpdate = Info["last_updated"].ToString();

            ViewBag.stateno = state["stateno"].ToString();
            ViewBag.statename = state["statename"].ToString();
            ViewBag.formid = form["revision"].ToString();

            ViewBag.flowdata = JsonConvert.SerializeObject(await Swan_flow_data.workflow_data(flowid, systemid["formid"].ToString(), state["stateno"].ToString(), task_userid));

            ViewBag.system = "SWAN";

            ViewBag.access_token = await Client_info.info_logon(value, "current_token");
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
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());


            return View("~/Views/web001/Workflow/Draft.cshtml");
        }

        [Proofpoint]
        public async Task<ActionResult> ApproveFlow()
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

            //log.info("Log Post Approveflow : " + body.ToString());

            Dictionary<string, string> postparam = new Dictionary<string, string>();
            string[] rawParams = body.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                postparam.Add(key, value);
            }

            return_swan_logic = new Dictionary<object, object>();
            try
            {
                var status_approve = await Trigger_action.trigger_Request_draf(sess_token, JsonConvert.SerializeObject(postparam));
                //   Add to trigger action
                // -- require userid , formid , flow , state , action ,title , branch



                //   End  To Trigger action




                return_swan_logic.Add("result", "Create Flow Done!");
            }
            catch (Exception e)
            {
                return_swan_logic.Add("result", "Error : " + e.ToString());
                log.info("Error state request : " + e.ToString());
            }





            //return Content(JsonConvert.SerializeObject(body.ToString()));
            return StatusCode(200);
        }


        private async Task<Dictionary<object, object>> getflowdetail(string flowid)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

       
            string Command = " SELECT";
            Command += " flw.formid,";
            Command += " f.form_th_name,";
            Command += " flw.last_title,";
            Command += " flw.comment_state";
            Command += " FROM";
            Command += " flow flw";
            Command += " INNER JOIN form f ON f.formid = flw.formid";
            Command += " WHERE";
            Command += " flw.flowid = @flow";
            Command += "";


            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flow", flowid);
            var result = await mysqlswan.data_with_col(Command, param);

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("formid", item["formid"].ToString());
                item_arr.Add("form_th_name", item["form_th_name"].ToString());
                item_arr.Add("last_title", item["last_title"].ToString());
                item_arr.Add("comment_state", item["comment_state"].ToString());
            }
            return item_arr;
        }

        private async Task<Dictionary<object, object>> getInfoDetail(string flowid)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

 
            string Command = " SELECT";
            Command += " f.form_th_name,";
            Command += " st.statename,";
            Command += " (SELECT th_name From `user`  WHERE userid = flw.starter) AS starter,";
            Command += " flw.start_date,";
            Command += " (SELECT th_name From `user`  WHERE userid = flw.last_active_user) AS last_user,";
            Command += " flw.last_updated";
            Command += " FROM";
            Command += " action AS a";
            Command += " INNER JOIN form AS f ON f.formid = a.formid";
            Command += " INNER JOIN state AS st ON st.formid = a.formid";
            Command += " AND st.stateno = a.stateno";
            Command += " INNER JOIN flow AS flw ON flw.flowid = a.flowid";
            Command += " WHERE";
            Command += " a.flowid = @flowid";
            Command += " ORDER BY";
            Command += " a.actionid DESC";
            Command += " LIMIT 1";


            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flowid", flowid);
            var result = await mysqlswan.data_with_col(Command, param);

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("form_th_name", item["form_th_name"].ToString());
                item_arr.Add("statename", item["statename"].ToString());
                item_arr.Add("starter", item["starter"].ToString());
                item_arr.Add("start_date", item["start_date"].ToString());
                item_arr.Add("last_user", item["last_user"].ToString());
                item_arr.Add("last_updated", item["last_updated"].ToString());
            }         
            return item_arr;
        }


        private async Task<Dictionary<object,object>> get_button(string flowid)
        {
            Logmodel log = new Logmodel();
            Mysqlswan mysqlswan = new Mysqlswan();

            //////////// Get button String //////////
            string Command_button = " SELECT to_stateno,action_condition FROM flow AS t1";
            Command_button += " INNER JOIN statepresent AS t2";          
            Command_button += " ON t1.flowid = t2.flowid ";
            Command_button += " AND t1.flowid = @flowid ";
            Command_button += " AND t2.formid = t1.formid";
            Command_button += " AND t2.from_stateno = t1.present_state";

            Dictionary<object, object> param_button = new Dictionary<object, object>();
            param_button.Add("flowid", flowid);
          
            var result_button = await mysqlswan.data_with_col(Command_button, param_button);
            JArray temp_data2 = JArray.Parse(JsonConvert.SerializeObject(result_button));

            var item_arr_output = new Dictionary<object,object>();
            int count = 0;
            foreach (var item in temp_data2)
            {
                var item_arr = new Dictionary<object, object>();
                item_arr.Add("stateno", item["to_stateno"].ToString());
                item_arr.Add("action_condition", item["action_condition"].ToString());
                item_arr_output.Add(count,item_arr);
                count = count + 1;
            }            
            return item_arr_output;
        }

        private async Task<Dictionary<object, object>> formdetail(string formid)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

            string Command = "select form_th_name,form_en_name,revision from form WHERE formid = @form";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("form", formid);
            var result = await mysqlswan.data_with_col(Command, param);

            //log.info("formname data : " + JsonConvert.SerializeObject(result));

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("form_th_name", item["form_th_name"].ToString());
                item_arr.Add("form_en_name", item["form_en_name"].ToString());
                item_arr.Add("revision", item["revision"].ToString());
            }
            //log.info("formname : " + JsonConvert.SerializeObject(item_arr));
            return item_arr;
        }

        private async Task<Dictionary<object, object>> statedetail(string formid, string stateno)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

            string Command = " SELECT stateno,statename from state WHERE formid = @form AND stateno = @state_no";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("form", formid);
            param.Add("state_no", stateno);
            var result = await mysqlswan.data_with_col(Command, param);

            //log.info("statedetail data : " + JsonConvert.SerializeObject(result));

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("stateno", item["stateno"].ToString());
                item_arr.Add("statename", item["statename"].ToString());

            }
           // log.info("statedetail : " + JsonConvert.SerializeObject(item_arr));

            return item_arr;
        }


        private async Task<Dictionary<object, object>> findState(string flowid)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var item_arr = new Dictionary<object, object>();

            string Command = "SELECT flowid,formid,present_state,starter,last_active_user FROM flow WHERE flowid = @flowid";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flowid", flowid);
            var result = await mysqlswan.data_with_col(Command, param);

            //log.info("statedetail data : " + JsonConvert.SerializeObject(result));

            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("flowid", item["flowid"].ToString());
                item_arr.Add("formid", item["formid"].ToString());
                item_arr.Add("present_state", item["present_state"].ToString());
                item_arr.Add("starter", item["starter"].ToString());
                item_arr.Add("last_active_user", item["last_active_user"].ToString());
            }
            //log.info("statedetail : " + JsonConvert.SerializeObject(item_arr));

            return item_arr;
        }

        






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

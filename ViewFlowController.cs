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
   
    public class ViewFlowController : Controller
    {

        private static Dictionary<object, object> return_swan_logic;
        
        public static string Command { get; private set; }
        public static string Command1 { get; private set; }
        public static string Command2 { get; private set; }
        public static string Command3 { get; private set; }
        public static string Command4 { get; private set; }
        public static string Command5 { get; private set; }
        public static string Command6 { get; private set; }

        public static Dictionary<object, object> param { get; private set; }
        public static Dictionary<object, object> param1 { get; private set; }
        public static Dictionary<object, object> param2 { get; private set; }        
        public static Dictionary<object, object> param3 { get; private set; }
        public static Dictionary<object, object> param4 { get; private set; }
        public static Dictionary<object, object> param5 { get; private set; }
        public static Dictionary<object, object> param6 { get; private set; }

        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();


        [Proofpoint]
        public async Task<IActionResult> Index(string flowid)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

             Stopwatch timefunction = new Stopwatch();
            timefunction.Start();

          
            _ = log.swan_core_log("View_flow_Time", "ViewFlow Start : " + timefunction.Elapsed.TotalMilliseconds);


            var value = HttpContext.Session.GetString("FirstSeen");
            var sess_token = HttpContext.Session.GetString("FirstSeen");

            var task_userid = await Client_info.info_logon(sess_token, "userid");

            _ = log.swan_core_log("View_flow_Time", "ViewFlow task_userid : " + timefunction.Elapsed.TotalMilliseconds);

            Dictionary<object,object> systemid = await getflowdetail(flowid);

            var findstate = await findState(flowid);
            _ = log.swan_core_log("View_flow_Time", "ViewFlow findstate : " + timefunction.Elapsed.TotalMilliseconds);

            var form = await  formdetail(systemid["formid"].ToString());
            _ = log.swan_core_log("View_flow_Time", "ViewFlow form : " + timefunction.Elapsed.TotalMilliseconds);

            var state = await statedetail(systemid["formid"].ToString(), findstate["present_state"].ToString());
            _ = log.swan_core_log("View_flow_Time", "ViewFlow state : " + timefunction.Elapsed.TotalMilliseconds);

            var button = JsonConvert.SerializeObject(get_button(flowid));
            //var button = "";


            _ = log.swan_core_log("View_flow_Time", "ViewFlow button : " + timefunction.Elapsed.TotalMilliseconds);

            var Info = await getInfoDetail(flowid);
            _ = log.swan_core_log("View_flow_Time", "ViewFlow Info : " + timefunction.Elapsed.TotalMilliseconds);


            ViewBag.formid =  systemid["formid"].ToString();
            ViewBag.form_th_name = form["form_th_name"].ToString();
            ViewBag.form_en_name = form["form_en_name"].ToString();
            ViewBag.form_revision = form["revision"].ToString();

           

            ViewBag.userid = task_userid;
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
            ViewBag.templatecode = form["revision"].ToString();

            _ = log.swan_core_log("View_flow_Time", "ViewFlow flowdata start : " + timefunction.Elapsed.TotalMilliseconds);
            ViewBag.flowdata = JsonConvert.SerializeObject(await Swan_flow_data.workflow_data(flowid, systemid["formid"].ToString(), state["stateno"].ToString(), task_userid));
            _ = log.swan_core_log("View_flow_Time", "ViewFlow flowdata end : " + timefunction.Elapsed.TotalMilliseconds);

            ViewBag.system = "SWAN";


            var result_history_review = await history_review(task_userid, "cc", systemid["formid"].ToString(), flowid, state["stateno"].ToString());
         
            var chk_condition = await Notification_flow.waring_action(flowid);
      
            var flow_condition_require_edoc = await Swan_flow_utility.check_request_edoc(systemid["formid"].ToString(), state["stateno"].ToString());

            var flow_condition_require_kyc = await Swan_flow_utility.check_request_kyc(systemid["formid"].ToString(), state["stateno"].ToString());

            if (flow_condition_require_edoc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_edoc = "";

            } else if (flow_condition_require_edoc == "true") {

                ViewBag.notification_edoc = await Notification_flow.waring_viewflow_edoc(flowid);
            }


            _ = log.swan_core_log("KYC_flow", "Flow_condition_require_KYC : " + flow_condition_require_kyc);



            if (flow_condition_require_kyc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_kyc = "";

            }
            else if (flow_condition_require_kyc == "true")
            {

                _ = log.swan_core_log("KYC_flow", " Warning:  " + await Notification_flow.waring_viewflow_kyc(flowid));

                ViewBag.notification_kyc = await Notification_flow.waring_viewflow_kyc(flowid);

            }            
            _= log.workflow(" chk_condition  ================= > :  " + chk_condition.ToString());

            //if (chk_condition == "0")
            //{
            //    if (systemid["formid"].ToString() == "6" & state["stateno"].ToString() == "15")
            //    {
            //        button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
            //    }
            //    else if (systemid["formid"].ToString() == "7" & state["stateno"].ToString() == "15")
            //    {
            //        button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
            //    }
            //    else
            //    {
            //        button = JsonConvert.SerializeObject(get_button(flowid));
            //    }
            //}
            //else
            //{
            //    if (systemid["formid"].ToString() == "6" & state["stateno"].ToString() == "15")
            //    {
            //        button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
            //    }
            //    else if (systemid["formid"].ToString() == "7" & state["stateno"].ToString() == "15")
            //    {
            //        button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
            //    }
            //    else {
            //        button = JsonConvert.SerializeObject(get_button(flowid));
            //    }                    
            //}

          

            ViewBag.button = button;


            var log_edoc = await Edocument.Edocument_flow_require_document(flowid);

            _ = log.swan_core_log("View_flow_Time", "ViewFlow log_edoc : " + timefunction.Elapsed.TotalMilliseconds);


            ViewBag.edocument = JsonConvert.SerializeObject(log_edoc);
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


         

         

            return View("~/Views/web001/Workflow/Viewflow.cshtml");
        }


        public async Task<string> history_review(string userid , string action,string formid,string flowid,string state)
        {

            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();

            try
            {
                var count_history_flow = await sum_history_review(userid, formid, state, action);
                _= log.debug("history Flow count_history_flow : " + count_history_flow.ToString());

                if (Int32.Parse(count_history_flow) >= 1)
                {
                    _ = log.debug("history Flow count_history_flow Exits Event : ");
                }
                else
                {
                    Command1 = " INSERT INTO  `history_review_flow`";
                    Command1 += " (`userid`, `action_type`, `formid`, `flowid`, `stateid`, `reviewdate`)";
                    Command1 += " VALUES(";
                    Command1 += " @1,"; // userid
                    Command1 += " @2,"; // action type
                    Command1 += " @3,"; // formid
                    Command1 += " @4,"; // flowid
                    Command1 += " @5,"; // state
                    Command1 += " @6"; // date
                    Command1 += " )";

                    param1 = new Dictionary<object, object>();
                    param1.Add("1", userid);
                    param1.Add("2", action);
                    param1.Add("3", formid);
                    param1.Add("4", flowid);
                    param1.Add("5", state);
                    param1.Add("6", DateTime.Now.ToString("yyyy-MM-dd h:mm"));

                    var add_history = await mysqlswan.data_utility(Command1, param1);


                   _=  log.debug("history Flow count_history_flow Result : " + JsonConvert.SerializeObject(add_history));
                }
            }
            catch (Exception e)
            {

                _ = log.debug("history Flow insert Error " + e.ToString());
            }


            return "";

        }

        public async Task<string> sum_history_review(string userid,string formid,string stateid,string action)
        {

            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            var return_value = "";

            try
            {

                Command1 = " SELECT  COUNT(userid) AS sumtotal from history_review_flow WHERE userid = @1 AND formid = @2 AND stateid = @3 AND action_type = @4";
                param1 = new Dictionary<object, object>();
                param1.Add("1", userid);// userid
                param1.Add("2", formid);// formid
                param1.Add("3", stateid);//state
                param1.Add("4", action);

                var sum_history_review_flow = await mysqlswan.data_with_col_api(Command1, param1);

                var temp = JsonConvert.SerializeObject(sum_history_review_flow);

                var result = JArray.Parse(temp);
                var temp2 = JObject.Parse(result[0].ToString());

                return_value = temp2["sumtotal"].ToString();

                log.debug("history Flow sum data " + temp2["sumtotal"].ToString());

            }
            catch (Exception e) {

                log.debug("history Flow sum Error " + e.ToString());
            }

            return return_value;

        }

        [Proofpoint]
        public async Task<IActionResult> CC_Flow(string flowid)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");
            var sess_token = HttpContext.Session.GetString("FirstSeen");

            var task_userid = await Client_info.info_logon(sess_token, "userid");

            Dictionary<object, object> systemid = await getflowdetail(flowid);

            var findstate = await findState(flowid);
            var form = await formdetail(systemid["formid"].ToString());
            var state = await statedetail(systemid["formid"].ToString(), findstate["present_state"].ToString());
            var button = JsonConvert.SerializeObject(get_button(flowid));
            var Info = await getInfoDetail(flowid);

            ViewBag.formid = systemid["formid"].ToString();
            ViewBag.form_th_name = form["form_th_name"].ToString();
            ViewBag.form_en_name = form["form_en_name"].ToString();
            ViewBag.form_revision = form["revision"].ToString();

            ViewBag.access_token = await Client_info.info_logon(value, "current_token");

            ViewBag.userid = task_userid;
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
            ViewBag.templatecode = form["revision"].ToString();

            ViewBag.flowdata = JsonConvert.SerializeObject(await Swan_flow_data.workflow_data(flowid, systemid["formid"].ToString(), state["stateno"].ToString(), task_userid));
            ViewBag.system = "SWAN";
            var result_history_review = await history_review(task_userid, "cc", systemid["formid"].ToString(), flowid, state["stateno"].ToString());
            var chk_condition = await Notification_flow.waring_action(flowid);
            var flow_condition_require_edoc = await Swan_flow_utility.check_request_edoc(systemid["formid"].ToString(), state["stateno"].ToString());
            var flow_condition_require_kyc = await Swan_flow_utility.check_request_edoc(systemid["formid"].ToString(), state["stateno"].ToString());

            if (flow_condition_require_edoc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_edoc = "";
            }
            else if (flow_condition_require_edoc == "true")
            {
                ViewBag.notification_edoc = await Notification_flow.waring_viewflow_edoc(flowid);
            }
            else if (flow_condition_require_kyc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_kyc = "";
            }
            else if (flow_condition_require_kyc == "true")
            {
                ViewBag.notification_kyc = await Notification_flow.waring_viewflow_kyc(flowid);
            }
            log.workflow(" chk_condition  ================= > :  " + chk_condition.ToString());

            if (chk_condition == "0")
            {
                if (systemid["formid"].ToString() == "6" & state["stateno"].ToString() == "15")
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else if (systemid["formid"].ToString() == "7" & state["stateno"].ToString() == "15")
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else
                {
                    ViewBag.button = button;
                }
            }
            else
            {




                if (systemid["formid"].ToString() == "6" & state["stateno"].ToString() == "15")
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else if (systemid["formid"].ToString() == "7" & state["stateno"].ToString() == "15")
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_button(""));
                }



            }




            // ViewBag.edocument = await Edocument.Edocument_flow_require_document(flowid);


           // var log_edoc = await Edocument.Edocument_flow_require_document(flowid);

            // _=  log.info("Log Edocument : "+JsonConvert.SerializeObject(log_edoc));

            //ViewBag.edocument = JsonConvert.SerializeObject(log_edoc);

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


            return View("~/Views/web001/Workflow/Viewcc.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> CC(string flowid)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");
            var sess_token = HttpContext.Session.GetString("FirstSeen");

            var task_userid = await Client_info.info_logon(sess_token, "userid");

            Dictionary<object, object> systemid = await getflowdetail(flowid);

            var findstate = await findState(flowid);
            var form = await formdetail(systemid["formid"].ToString());
            var state = await statedetail(systemid["formid"].ToString(), findstate["present_state"].ToString());
            var button = JsonConvert.SerializeObject(get_button(flowid));
            var Info = await getInfoDetail(flowid);

            ViewBag.access_token = await Client_info.info_logon(value, "current_token");

            ViewBag.formid = systemid["formid"].ToString();
            ViewBag.form_th_name = form["form_th_name"].ToString();
            ViewBag.form_en_name = form["form_en_name"].ToString();
            ViewBag.form_revision = form["revision"].ToString();

            ViewBag.userid = task_userid;
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
            ViewBag.templatecode = form["revision"].ToString();
          
            ViewBag.flowdata = JsonConvert.SerializeObject(await Swan_flow_data.workflow_data(flowid, systemid["formid"].ToString(), state["stateno"].ToString(), task_userid));

            ViewBag.system = "SWAN";
            var result_history_review = await history_review(task_userid, "cc_view", systemid["formid"].ToString(), flowid, state["stateno"].ToString());
            var chk_condition = await Notification_flow.waring_action(flowid);
            var flow_condition_require_edoc = await Swan_flow_utility.check_request_edoc(systemid["formid"].ToString(), state["stateno"].ToString());
            var flow_condition_require_kyc = await Swan_flow_utility.check_request_edoc(systemid["formid"].ToString(), state["stateno"].ToString());

            if (flow_condition_require_edoc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_edoc = "";
            }
            else if (flow_condition_require_edoc == "true")
            {

                ViewBag.notification_edoc = await Notification_flow.waring_viewflow_edoc(flowid);
            }
            else if (flow_condition_require_kyc == "fail")
            {
                chk_condition = "0";
                ViewBag.notification_kyc = "";
            }
            else if (flow_condition_require_kyc == "true")
            {
                ViewBag.notification_kyc = await Notification_flow.waring_viewflow_kyc(flowid);
            }
            log.workflow(" chk_condition  ================= > :  " + chk_condition.ToString());
            if (chk_condition == "0")
            {
                if (systemid["formid"].ToString() == "6" & state["stateno"].ToString() == "15")
                {
                    ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else if (systemid["formid"].ToString() == "7" & state["stateno"].ToString() == "15")
                {
                   ViewBag.button = JsonConvert.SerializeObject(get_spcial_button_condition(flowid));
                }
                else
                {
                    ViewBag.button = button;
                }
            }
            else
            {
                ViewBag.button = JsonConvert.SerializeObject(get_button(""));
            }
            // ViewBag.edocument = await Edocument.Edocument_flow_require_document(flowid);
            var log_edoc = await Edocument.Edocument_flow_require_document(flowid);
            ViewBag.edocument = JsonConvert.SerializeObject(log_edoc);
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



            return View("~/Views/web001/Workflow/Viewcc.cshtml");
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

            var detail_request = JObject.Parse(JsonConvert.SerializeObject(postparam));

  
        
        

           _ = log.workflow("Log ==== > userid :  " + await Client_info.info_logon(sess_token, "userid"));
           _ = log.workflow("Log ==== > email :  " + await Client_info.info_logon(sess_token, "email"));
           _ = log.workflow("Log ==== > en_name :  " + await Client_info.info_logon(sess_token, "en_name"));
           _ = log.workflow("Log ==== > th_name :  " + await Client_info.info_logon(sess_token, "th_name"));
           _ = log.workflow("Log ==== > user_type :  " + await Client_info.info_logon(sess_token, "user_type"));
           _ = log.workflow("Log ==== > branch :  " + await Client_info.info_logon(sess_token, "branch"));
           _ = log.workflow("Log ==== > role :  " + await Client_info.info_logon(sess_token, "role"));
           _ = log.workflow("Log ==== > accesslevel :  " + await Client_info.info_logon(sess_token, "accesslevel"));

           
            var result_history_review = await history_review(await Client_info.info_logon(sess_token, "userid"), "Action", detail_request["formid"].ToString(), detail_request["flowid"].ToString(), detail_request["action"].ToString());




            _ = log.workflow("Log ============================ > Action :  " + JsonConvert.SerializeObject(postparam));


            return_swan_logic = new Dictionary<object, object>();


            try
            {


                var status_approve = await Trigger_action.trigger_Approve_flow(await Client_info.info_logon(sess_token, "userid"), JsonConvert.SerializeObject(postparam));
                //   Add to trigger action
                // -- require userid , formid , flow , state , action ,title , branch
                //   End  To Trigger action
                return_swan_logic.Add("result", "Create Flow Done!");




            }
            catch (Exception e)
            {
                return_swan_logic.Add("result", "Error : " + e.ToString());
                log.workflow("Error state request : " + e.ToString());
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
            Command_button += " GROUP BY t2.action_condition";
            Command_button += " ORDER BY t2.id ";

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


        private async Task<Dictionary<object, object>> get_spcial_button_condition(string flowid)
        {
            Logmodel log = new Logmodel();
            Mysqlswan mysqlswan = new Mysqlswan();

            string Command_button = "";

           string Command_condition = "SELECT COUNT(*) AS chk_condition FROM action WHERE flowid = @1 AND stateno = '14' AND action_type LIKE '%Deviation%' ORDER BY actionid DESC LIMIT 1";

            Dictionary<object, object> param_condition = new Dictionary<object, object>();
            param_condition.Add("1", flowid);

            var query_result = await mysqlswan.data_with_col_api(Command_condition, param_condition);


            var temp_data = JArray.Parse(JsonConvert.SerializeObject(query_result));

       

            if (temp_data[0]["chk_condition"].ToString() == "0")
            {
                Command_button = " SELECT to_stateno,action_condition FROM flow AS t1";
                Command_button += " INNER JOIN statepresent AS t2";
                Command_button += " ON t1.flowid = t2.flowid ";
                Command_button += " AND t1.flowid = @flowid ";
                Command_button += " AND t2.formid = t1.formid";
                Command_button += " AND t2.from_stateno = t1.present_state";
                Command_button += " AND t2.action_condition NOT LIKE '%Deviation%' ";
                Command_button += " GROUP BY t2.action_condition";
                Command_button += " ORDER BY t2.id ";

            }
            else {

                Command_button = " SELECT to_stateno,action_condition FROM flow AS t1";
                Command_button += " INNER JOIN statepresent AS t2";
                Command_button += " ON t1.flowid = t2.flowid ";
                Command_button += " AND t1.flowid = @flowid ";
                Command_button += " AND t2.formid = t1.formid";
                Command_button += " AND t2.from_stateno = t1.present_state";
                Command_button += " AND t2.action_condition NOT LIKE '%Approve%' ";
                Command_button += " GROUP BY t2.action_condition";
                Command_button += " ORDER BY t2.id ";



            }




            //////////// Get button String //////////

            Dictionary<object, object> param_button = new Dictionary<object, object>();
            param_button.Add("flowid", flowid);

            var result_button = await mysqlswan.data_with_col(Command_button, param_button);
            JArray temp_data2 = JArray.Parse(JsonConvert.SerializeObject(result_button));

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
            JArray temp_data = JArray.Parse(JsonConvert.SerializeObject(result));

            foreach (var item in temp_data)
            {
                item_arr.Add("flowid", item["flowid"].ToString());
                item_arr.Add("formid", item["formid"].ToString());
                item_arr.Add("present_state", item["present_state"].ToString());
                item_arr.Add("starter", item["starter"].ToString());
                item_arr.Add("last_active_user", item["last_active_user"].ToString());
            }
            return item_arr;
        }     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

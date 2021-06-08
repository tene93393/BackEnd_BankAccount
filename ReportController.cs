using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using service.Extension;
using Newtonsoft.Json.Linq;
using System.IO;

namespace service.Controllers
{
   
    public class ReportController : Controller
    {
       
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

        [Proofpoint]
        public async Task<IActionResult> Index(string formid, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = formid;
            ViewBag.system = system;

            ViewBag.access_token = await Client_info.info_logon(value, "current_token");
            ViewBag.userid = await Client_info.info_logon(value, "userid");
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


            ViewBag.state = "0";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            return View("~/Views/web001/Report/Index.cshtml");
        }


        private static async Task<string> api_value(string values)
        {
            Logmodel log = new Logmodel();
            var func_value = "";
            char[] chars = { '*', ' ', '\'', '<', '>' };
            try
            {
                if (String.IsNullOrEmpty(values))
                {
                    func_value = "";
                }
                else
                {
                    func_value = values.Trim(' ');
                }
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("Error", "api_value Error" + e.ToString());
            }
            return func_value;
        }


        [Proofpoint]
        public async Task<IActionResult> Wealth(string reportid, wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            // const string sessionKey = "FirstSeen";

            _ = log.swan_core_log("REPORTBUILD", "Start test Print  : " + reportid.ToString());

            var value_token = HttpContext.Session.GetString("FirstSeen");


            var url = cfg.initial_config("ploter");

            //if (HttpContext.Request.IsHttps.ToString() == "True")
            //{
            //    url = "https://" + HttpContext.Request.Host;
           // }
            //else
            //{
            //    url = "http://" + HttpContext.Request.Host;
            //}

            _ = log.swan_core_log("REPORTBUILD", "Start url  : " + url.ToString());

         

            ViewBag.formid = "";
            ViewBag.system = "SWAN";

            ViewBag.state = "0";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            var passdata_wealth1 = new Dictionary<object, object>();
            passdata_wealth1.Add("v1", await api_value(passdata.v1));
            passdata_wealth1.Add("v2", await api_value(passdata.v2));
            passdata_wealth1.Add("v3", await api_value(passdata.v3));
            passdata_wealth1.Add("v4", await api_value(passdata.v4));
            passdata_wealth1.Add("v5", await api_value(passdata.v5));
            passdata_wealth1.Add("v6", await api_value(passdata.v6));
            passdata_wealth1.Add("v7", await api_value(passdata.v7));
            passdata_wealth1.Add("v8", await api_value(passdata.v8));
            passdata_wealth1.Add("v9", await api_value(passdata.v9));

            passdata_wealth1.Add("v10", await api_value(passdata.v10));
            passdata_wealth1.Add("v11", await api_value(passdata.v11));
            passdata_wealth1.Add("v12", await api_value(passdata.v12));
            passdata_wealth1.Add("v13", await api_value(passdata.v13));
            passdata_wealth1.Add("v14", await api_value(passdata.v14));
            passdata_wealth1.Add("v15", await api_value(passdata.v15));
            passdata_wealth1.Add("v16", await api_value(passdata.v16));
            passdata_wealth1.Add("v17", await api_value(passdata.v17));
            passdata_wealth1.Add("v18", await api_value(passdata.v18));
            passdata_wealth1.Add("v19", await api_value(passdata.v19));
            passdata_wealth1.Add("v20", await api_value(passdata.v20));


            var report_encode = await encode.encode_aes("encrypt", await Client_info.info_logon(value_token, "userid") + "|" + Date.date_now());

            var user_info = new Dictionary<object, object>();
            user_info.Add("userid", await Client_info.info_logon(value_token, "userid"));
            user_info.Add("username", await Client_info.info_logon(value_token, "username"));
            user_info.Add("email", await Client_info.info_logon(value_token, "email"));
            user_info.Add("engname", await Client_info.info_logon(value_token, "en_name"));
            user_info.Add("thainame", await Client_info.info_logon(value_token, "th_name"));
            user_info.Add("marketingid", await Client_info.info_logon(value_token, "marketingid"));
            user_info.Add("traderid", await Client_info.info_logon(value_token, "traderid"));
            user_info.Add("branch", await Client_info.info_logon(value_token, "branch"));
            user_info.Add("groupcode", await Client_info.info_logon(value_token, "groupcode"));
            user_info.Add("user_type", await Client_info.info_logon(value_token, "user_type"));
            user_info.Add("accesslevel", await Client_info.info_logon(value_token, "accesslevel"));
            user_info.Add("date_th", Date.date_now());
            user_info.Add("date_en", Date.date_en());
            user_info.Add("date_th_word", Date.date_th_string());
            user_info.Add("date_en_word", Date.date_en_string());
            user_info.Add("report_name", report_encode);
            user_info.Add("release", "SWAN : " + Appconfig.appfcg("release"));


            string FilePath = "";

            try
            {


                if (reportid == "")
                {
                    FilePath = await Pdf_model.Create_pdf_wizard_custom("", url, reportid);
                }
                else
                {
                    
                    //string viewFromAnotherController = await this.Report_Async("~/Views/web001/Reportbuild/Master.cshtml", await wealth_internal(passdata_wealth1), passdata_wealth1, user_info, url);
                    // string html = viewFromAnotherController;

                    string html = await service.Models.Pdf_Html.htdata(reportid);

                    var temp = new Dictionary<object, object>();
                    temp.Add("data",JsonConvert.SerializeObject(await wealth_internal(passdata_wealth1)));

                    var msg = html;


                    var param_loop = JObject.Parse(JsonConvert.SerializeObject(passdata_wealth1));
                 
                    foreach (var item in param_loop)
                    {
                        msg = replace_text(msg, "{{"+item.Key.ToString()+"}}", item.Value.ToString());
                    }

                    var param_user_info = JObject.Parse(JsonConvert.SerializeObject(user_info));

                    foreach (var item_userinfo in param_user_info)
                    {
                        msg = replace_text(msg, "{{" + item_userinfo.Key.ToString() + "}}", item_userinfo.Value.ToString());
                    }


                    var param_data = JObject.Parse(JsonConvert.SerializeObject(temp));

                    foreach (var item_data in param_data)
                    {
                        msg = replace_text(msg, "{{" + item_data.Key.ToString() + "}}", item_data.Value.ToString());
                    }

                    msg = replace_text(msg, "{{url}}", url);


                    _ = log.swan_core_log("REPORTBUILD", "==================================");
                    _ = log.swan_core_log("REPORTBUILD", "HTML Create PDF : " + msg);
                    _ = log.swan_core_log("REPORTBUILD", "==================================");

                    FilePath = await Pdf_model.Create_pdf_wizard_custom(msg, url, reportid);

                }


            }
            catch (Exception e)
            {
                _ = log.swan_core_log("REPORTBUILD", "log Error : " + e.ToString());
            }

            _ = log.swan_core_log("REPORTBUILD", "FilePath : " + FilePath.ToString());

            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");
        }




        [Proofpoint]
        public async Task<IActionResult> Wealth_seconds(string reportid, wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            // const string sessionKey = "FirstSeen";

            _ = log.swan_core_log("REPORTBUILD", "Start test Print  : " + reportid.ToString());

            var value_token = HttpContext.Session.GetString("FirstSeen");


            var url = cfg.initial_config("ploter");

            //if (HttpContext.Request.IsHttps.ToString() == "True")
            //{
            //    url = "https://" + HttpContext.Request.Host;
            // }
            //else
            //{
            //    url = "http://" + HttpContext.Request.Host;
            //}

            _ = log.swan_core_log("REPORTBUILD", "Start url  : " + url.ToString());



            ViewBag.formid = "";
            ViewBag.system = "SWAN";

            ViewBag.state = "0";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            var passdata_wealth1 = new Dictionary<object, object>();
            passdata_wealth1.Add("v1", await api_value(passdata.v1));
            passdata_wealth1.Add("v2", await api_value(passdata.v2));
            passdata_wealth1.Add("v3", await api_value(passdata.v3));
            passdata_wealth1.Add("v4", await api_value(passdata.v4));
            passdata_wealth1.Add("v5", await api_value(passdata.v5));
            passdata_wealth1.Add("v6", await api_value(passdata.v6));
            passdata_wealth1.Add("v7", await api_value(passdata.v7));
            passdata_wealth1.Add("v8", await api_value(passdata.v8));
            passdata_wealth1.Add("v9", await api_value(passdata.v9));

            passdata_wealth1.Add("v10", await api_value(passdata.v10));
            passdata_wealth1.Add("v11", await api_value(passdata.v11));
            passdata_wealth1.Add("v12", await api_value(passdata.v12));
            passdata_wealth1.Add("v13", await api_value(passdata.v13));
            passdata_wealth1.Add("v14", await api_value(passdata.v14));
            passdata_wealth1.Add("v15", await api_value(passdata.v15));
            passdata_wealth1.Add("v16", await api_value(passdata.v16));
            passdata_wealth1.Add("v17", await api_value(passdata.v17));
            passdata_wealth1.Add("v18", await api_value(passdata.v18));
            passdata_wealth1.Add("v19", await api_value(passdata.v19));
            passdata_wealth1.Add("v20", await api_value(passdata.v20));


            var report_encode = await encode.encode_aes("encrypt", await Client_info.info_logon(value_token, "userid") + "|" + Date.date_now());

            var user_info = new Dictionary<object, object>();
            user_info.Add("userid", await Client_info.info_logon(value_token, "userid"));
            user_info.Add("username", await Client_info.info_logon(value_token, "username"));
            user_info.Add("email", await Client_info.info_logon(value_token, "email"));
            user_info.Add("engname", await Client_info.info_logon(value_token, "en_name"));
            user_info.Add("thainame", await Client_info.info_logon(value_token, "th_name"));
            user_info.Add("marketingid", await Client_info.info_logon(value_token, "marketingid"));
            user_info.Add("traderid", await Client_info.info_logon(value_token, "traderid"));
            user_info.Add("branch", await Client_info.info_logon(value_token, "branch"));
            user_info.Add("groupcode", await Client_info.info_logon(value_token, "groupcode"));
            user_info.Add("user_type", await Client_info.info_logon(value_token, "user_type"));
            user_info.Add("accesslevel", await Client_info.info_logon(value_token, "accesslevel"));
            user_info.Add("date_th", Date.date_now());
            user_info.Add("date_en", Date.date_en());
            user_info.Add("date_th_word", Date.date_th_string());
            user_info.Add("date_en_word", Date.date_en_string());
            user_info.Add("report_name", report_encode);
            user_info.Add("release", "SWAN : " + Appconfig.appfcg("release"));


            _ = log.swan_core_log("REPORTBUILD", "user_info Start   :"+JsonConvert.SerializeObject(user_info));


            string FilePath = "";

            try
            {


                if (reportid == "")
                {
                    FilePath = await Pdf_model.Create_pdf_wizard_custom("", url, reportid);
                }
                else
                {

                    //string viewFromAnotherController = await this.Report_Async("~/Views/web001/Reportbuild/Master.cshtml", await wealth_internal(passdata_wealth1), passdata_wealth1, user_info, url);
                    // string html = viewFromAnotherController;

                    string html = await service.Models.Pdf_Html.htdata(reportid);

                    var temp = new Dictionary<object, object>();
                    temp.Add("data", JsonConvert.SerializeObject(await wealth_internals(passdata_wealth1)));



                    _ = log.swan_core_log("REPORTBUILD", "Data form db Start   :" + JsonConvert.SerializeObject(temp));


                    var msg = html;


                    var param_loop = JObject.Parse(JsonConvert.SerializeObject(passdata_wealth1));

                    foreach (var item in param_loop)
                    {
                        msg = replace_text(msg, "{{" + item.Key.ToString() + "}}", item.Value.ToString());
                    }

                    var param_user_info = JObject.Parse(JsonConvert.SerializeObject(user_info));

                    foreach (var item_userinfo in param_user_info)
                    {
                        msg = replace_text(msg, "{{" + item_userinfo.Key.ToString() + "}}", item_userinfo.Value.ToString());
                    }


                    var param_data = JObject.Parse(JsonConvert.SerializeObject(temp));

                    foreach (var item_data in param_data)
                    {
                        msg = replace_text(msg, "{{" + item_data.Key.ToString() + "}}", item_data.Value.ToString());
                    }

                    msg = replace_text(msg, "{{url}}", url);


                    _ = log.swan_core_log("REPORTBUILD", "==================================");
                    _ = log.swan_core_log("REPORTBUILD", "HTML Create PDF : " + msg);
                    _ = log.swan_core_log("REPORTBUILD", "==================================");

                    FilePath = await Pdf_model.Create_pdf_wizard_custom(msg, url, reportid);

                }


            }
            catch (Exception e)
            {
                _ = log.swan_core_log("REPORTBUILD", "log Error : " + e.ToString());
            }

            _ = log.swan_core_log("REPORTBUILD", "FilePath : " + FilePath.ToString());

            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");
        }



        private static string replace_text(string txt, string patten, string replace)
        {
            return txt.Replace(patten, replace);
        }

        public async Task<List<object>> wealth_internals(Dictionary<object, object> data)
        {
            Logmodel log = new Logmodel();
            Mysqlwealth_spc wealth_spc = new Mysqlwealth_spc();


            _ = log.swan_core_log("REPORTBUILD", "wealth_internal Start   : " + JsonConvert.SerializeObject(data));

            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");

            var temp_data = new List<object>();
            var temp = "";

            string Command = "CALL Wealth_project (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20)";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("1", data["v1"].ToString());
            param.Add("2", data["v2"].ToString());
            param.Add("3", data["v3"].ToString());
            param.Add("4", data["v4"].ToString());
            param.Add("5", data["v5"].ToString());
            param.Add("6", data["v6"].ToString());
            param.Add("7", data["v7"].ToString());
            param.Add("8", data["v8"].ToString());
            param.Add("9", data["v9"].ToString());
            param.Add("10", data["v10"].ToString());
            param.Add("11", data["v11"].ToString());
            param.Add("12", data["v12"].ToString());
            param.Add("13", data["v13"].ToString());
            param.Add("14", data["v14"].ToString());
            param.Add("15", data["v15"].ToString());
            param.Add("16", data["v16"].ToString());
            param.Add("17", data["v17"].ToString());
            param.Add("18", data["v18"].ToString());
            param.Add("19", data["v19"].ToString());
            param.Add("20", data["v20"].ToString());



            //var data_1 = await  wealth_spc.data_with_col(project_request, Command, param);
            var data_1 = await wealth_spc.data_with_col_api(Command, param);
            temp_data.Add(data_1);



            _ = log.swan_core_log("REPORTBUILD", "wealth_internals data   : " + JsonConvert.SerializeObject(data_1));


            return data_1;

        }



        public async Task<List<object>> wealth_internal(Dictionary<object, object> data)
        {
            Logmodel log = new Logmodel();
            _ = log.swan_core_log("REPORTBUILD", "wealth_internal Start   : " + JsonConvert.SerializeObject(data));

            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            var temp = "";

            string Command = "EXEC  Wealth_project @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("1", data["v1"].ToString());
            param.Add("2", data["v2"].ToString());
            param.Add("3", data["v3"].ToString());
            param.Add("4", data["v4"].ToString());
            param.Add("5", data["v5"].ToString());
            param.Add("6", data["v6"].ToString());
            param.Add("7", data["v7"].ToString());
            param.Add("8", data["v8"].ToString());
            param.Add("9", data["v9"].ToString());
            param.Add("10", data["v10"].ToString());
            param.Add("11", data["v11"].ToString());
            param.Add("12", data["v12"].ToString());
            param.Add("13", data["v13"].ToString());
            param.Add("14", data["v14"].ToString());
            param.Add("15", data["v15"].ToString());
            param.Add("16", data["v16"].ToString());
            param.Add("17", data["v17"].ToString());
            param.Add("18", data["v18"].ToString());
            param.Add("19", data["v19"].ToString());
            param.Add("20", data["v20"].ToString());



            var data_1 = Core_mssql.data_with_col(project_request, Command, param);



           


            return data_1;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

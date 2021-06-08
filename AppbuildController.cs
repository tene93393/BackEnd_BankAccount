using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;
using service.Extension;

namespace service.Controllers
{
   
    public class AppbuildController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

        public AppbuildController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Proofpoint]
        public async Task<IActionResult> Index()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            ViewBag.formid = "";

            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";


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


            return View("~/Views/web001/Appbuild/Index.cshtml");
        }
       
       
        [Proofpoint]
        public async Task<IActionResult> Flexlist(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.fetchdata = "fetch_data";
            ViewBag.method = "action";
            ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

            ViewBag.api = apiname;
            ViewBag.token = value;

            ViewBag.formid = "";

            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";


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




            return View("~/Views/web001/Appbuild/Flexlist.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> lineBuild(string apiname,string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.apiname = apiname;

            ViewBag.js_content = await Get_config_flex(apiname, system);
            ViewBag.json_content = await Get_content_flex(apiname, system);

            ViewBag.formid = "";

            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";


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

            return View("~/Views/web001/Appbuild/LineEditor.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> Maillist(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.fetchdata = "fetch_data";
            ViewBag.method = "action";
            ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

            ViewBag.api = apiname;
            ViewBag.token = value;

            ViewBag.formid = "";

            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";


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

            return View("~/Views/web001/Appbuild/Maillist.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> MailBuild(string apiname, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();


            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.apiname = apiname;

            ViewBag.js_content = await Get_config_mail(apiname, system);
            ViewBag.json_content = await Get_content_mail(apiname, system);

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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


            return View("~/Views/web001/Appbuild/MailEditor.cshtml");

        }


        ///  Job  Create Report  

        [Proofpoint]
        public async Task<IActionResult> Reportpdflist(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.fetchdata = "fetch_data";
            ViewBag.method = "action";
            ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

            ViewBag.api = apiname;
            ViewBag.token = value;

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Reportpdflist.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> ReportpdfBuild(string apiname, string system)
        {
            Logmodel log = new Logmodel();

            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.apiname = apiname;

            try
            {
                _= log.swan_core_log("REPORTBUILD", "log jquery : " + await Get_config_reportpdf(apiname, system, "jquery"));
                _= log.swan_core_log("REPORTBUILD", "log config : " + await Get_config_reportpdf(apiname, system, "config"));
                _= log.swan_core_log("REPORTBUILD", "log css : " + await Get_config_reportpdf(apiname, system, "css"));
                _= log.swan_core_log("REPORTBUILD", "log templates : " + await Get_config_reportpdf(apiname, system, "templates"));

                ViewBag.js = await Get_config_reportpdf(apiname, system, "jquery");
                ViewBag.config = await Get_config_reportpdf(apiname, system, "config");
                ViewBag.css = await Get_config_reportpdf(apiname, system, "css");
                ViewBag.html = await Get_config_reportpdf(apiname, system, "templates");

            } catch (Exception e) {

                _ = log.swan_core_log("REPORTBUILD", "log Error : " + e.ToString());

            }

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/ReportpdfEditor.cshtml");

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
        public async Task<FileResult> report(string reportid , wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            // const string sessionKey = "FirstSeen";

            _ = log.swan_core_log("REPORTBUILD", "Start test Print  : " + reportid.ToString());

            var value_token = HttpContext.Session.GetString("FirstSeen");


            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }

            _ = log.swan_core_log("REPORTBUILD", "Start url  : " + url.ToString());

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

            try { 

         
            if (reportid == "")
            {
                    FilePath = await Pdf_model.Create_pdf_wizard_custom("", url, reportid);
            }
            else
            {
                    _ = log.swan_core_log("REPORTBUILD", "log jquery : " + await Get_config_reportpdf(reportid, "SWAN", "jquery"));
                    _ = log.swan_core_log("REPORTBUILD", "log config : " + await Get_config_reportpdf(reportid, "SWAN", "config"));
                    _ = log.swan_core_log("REPORTBUILD", "log css : " + await Get_config_reportpdf(reportid, "SWAN", "css"));
                    _ = log.swan_core_log("REPORTBUILD", "log templates : " + await Get_config_reportpdf(reportid, "SWAN", "templates"));


                    var js_template = await Get_config_reportpdf(reportid, "SWAN", "jquery");
                    var config_template = await Get_config_reportpdf(reportid, "SWAN", "config");
                    var css_template = await Get_config_reportpdf(reportid, "SWAN", "css");
                    var html_template = await Get_config_reportpdf(reportid, "SWAN", "templates");

                    string viewFromAnotherController = await this.Report_Async("~/Views/web001/Reportbuild/Master.cshtml", await wealth_internal(passdata_wealth1), passdata_wealth1, user_info, url);
                    string html = viewFromAnotherController;
                    FilePath = await Pdf_model.Create_pdf_wizard_custom(html, url, reportid);
                  
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


        public async Task<List<object>> wealth_internal(Dictionary<object, object> data)
        {
            Logmodel log = new Logmodel();
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

            log.info("log Data wealth jArray : " + JsonConvert.SerializeObject(data_1));


            return data_1;

        }

        [Proofpoint]
        public async Task<FileResult> report1(string reportid)
        {

            var value = HttpContext.Session.GetString("FirstSeen");
            string FilePath = "";
            var url = "";

            try
            {


                if (HttpContext.Request.IsHttps.ToString() == "True")
                {
                    url = "https://" + HttpContext.Request.Host;
                }
                else
                {
                    url = "https://" + HttpContext.Request.Host;
                }
                log.info("url " + url);


                if (reportid == "")
                {
                    FilePath = await Pdf_model.Create_pdf("", url);
                }
                else
                {
                    FilePath = await Pdf_model.Create_pdf(reportid, url);
                }
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("REPORTBUILD", "log jquery : " + e.ToString());


            }

            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
                System.IO.File.Delete(FilePath);
                return File(pdfByte, "application/pdf");

          
          
        }




        ///  Job  Create Report  


        [Proofpoint]
        public async Task<IActionResult> Dmlist(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();


            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.fetchdata = "fetch_data";
            ViewBag.method = "action";
            ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

            ViewBag.api = apiname;
            ViewBag.token = value;

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Dmlist.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> DmBuild(string apiname, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();


            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.apiname = apiname;

            ViewBag.js = await Get_config_reportpdf(apiname, system, "jquery");
            ViewBag.config = await Get_config_reportpdf(apiname, system, "config");
            ViewBag.css = await Get_config_reportpdf(apiname, system, "css");
            ViewBag.html = await Get_config_reportpdf(apiname, system, "templates");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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


            return View("~/Views/web001/Appbuild/DmEditor.cshtml");

        }







        [Proofpoint]
        public async Task<IActionResult> Formslist(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.fetchdata = "fetch_data";
            ViewBag.method = "action";
            ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

            ViewBag.api = apiname;
            ViewBag.token = value;


            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Formslist.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> FormsBuild(string apiname, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            ViewBag.apiname = apiname;

            ViewBag.config = await Get_config_Formbuild(apiname, system, "config");
            ViewBag.jquery = await Get_config_Formbuild(apiname, system, "jquery");
            ViewBag.html = await Get_config_Formbuild(apiname, system, "templates");
            ViewBag.css = await Get_config_Formbuild(apiname, system, "css");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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


            return View("~/Views/web001/Appbuild/FormsEditor.cshtml");

        }
        [Proofpoint]
        public async Task<IActionResult> Formpreview(string formid,string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");
           
            ViewBag.formid = formid;
            ViewBag.system = system;

          
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Formpreview.cshtml");
        }

        [Proofpoint]
        private async Task<string> Get_config_Formbuild(string formid, string system,string type)
        {
            Mysqlswan mysqlswan = new Mysqlswan();

            //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

            string command = "select * from forms_template WHERE templatecode = @formid AND `system` = @system AND status = 'Y' ";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("formid", formid);
            param.Add("system", system);
            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0][type].ToString();
        }


        [Proofpoint]
        private async Task<string> Get_config_reportpdf(string reportid, string system,string type)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            // connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
           // var data = Core_mssql.data_with_col(connect_extension.connect_db, command, param);

            string command = "select * from reportpdf_template WHERE templatecode = @reportid AND  `system` = @system AND status = @3 ";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("reportid", reportid);
            param.Add("system", system);
            param.Add("3", "Y");

            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0][type].ToString();
        }

      

        [Proofpoint]
        public async Task<ActionResult> Fireswan_service(string reportid)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Fireswan_service.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> Htmlreport()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Htmlreport.cshtml");
        }

        [Proofpoint]
        public async Task<IActionResult> Line_flex()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Html.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> Editor(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");


            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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


            string apidata = "";
            switch(apiname)
            {
                case "Swan":
                    apidata = "Swan";
                    break;
                case "Fireswan":
                    apidata = "Fireswan";
                    break;
                case "Hawknet":
                    apidata = "Hawknet";
                    break;
                case "Internal":
                    apidata = "Internal";
                    break;
                case "Tiger":
                    apidata = "Tiger";
                    break;
                case "":
                    apidata = "Temp";
                    break;
                default:
                    apidata = "Temp";
                    break;
            }
            ViewBag.json_content = await Get_content_json(apidata);
            ViewBag.apiname = apidata;

            return View("~/Views/web001/Appbuild/Editor.cshtml");
        }
        [Proofpoint]
        public async Task<ActionResult> Postsave(string formid,string type,string data)
        {
            Encryption_model encode = new Encryption_model();
        
            switch (encode.base64_decode(type))
            {
                case "json":
                    write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "html":
                     write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "css":
                     write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "js":
                     write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
            }
            return View("~/Views/web001/Appbuild/Index.cshtml");

        }
        [Proofpoint]
        public async Task<ActionResult> Saveflex(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Encryption_model encode = new Encryption_model();
           

            switch (encode.base64_decode(type))
            {
                case "json":
                    connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

                    string command_json = "UPDATE line_flex_template SET templates = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param_json = new Dictionary<object, object>();
                    param_json.Add("value", encode.base64_decode(data));
                    param_json.Add("formid", encode.base64_decode(formid));

                    var result_json = await mysqlswan.data_utility(command_json, param_json);

                   

                    break;
                case "html":
                    //write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "css":
                   /// write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "js":
                   //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                    string command = "UPDATE line_flex_template SET config = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("value",encode.base64_decode(data));
                    param.Add("formid", encode.base64_decode(formid));
                    var result = await mysqlswan.data_utility(command, param);
                    break;
            }
            return View("~/Views/web001/Appbuild/Index.cshtml");

        }

        [Proofpoint]
        public async Task<ActionResult> Savemail(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Encryption_model encode = new Encryption_model();
        
            switch (encode.base64_decode(type))
            {
                case "json":
                  


                    break;
                case "html":
                   // connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

                    string command_json = "UPDATE mail_template SET templates = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param_json = new Dictionary<object, object>();
                    param_json.Add("value", encode.base64_decode(data));
                    param_json.Add("formid", encode.base64_decode(formid));

                    var result_json = await mysqlswan.data_utility(command_json, param_json);

                    break;
                case "css":
                    /// write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    break;
                case "js":
                    //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                    string command = "UPDATE mail_template SET config = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("value", encode.base64_decode(data));
                    param.Add("formid", encode.base64_decode(formid));

                    var result = await mysqlswan.data_utility(command, param);
                    break;
            }
            return View("~/Views/web001/Appbuild/Index.cshtml");

        }
        [Proofpoint]
        public async Task<ActionResult> Savereportpdf(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();

            Encryption_model encode = new Encryption_model();
          
            switch (encode.base64_decode(type))
            {
                case "json":

                    string command_json = "UPDATE reportpdf_template SET config = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param_json = new Dictionary<object, object>();
                    param_json.Add("value", encode.base64_decode(data));
                    param_json.Add("formid", encode.base64_decode(formid));

                    var result_json = await mysqlswan.data_utility(command_json, param_json);

                    break;
                case "html":
                    //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

                    string command_html = "UPDATE reportpdf_template SET templates = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param_html = new Dictionary<object, object>();
                    param_html.Add("value", encode.base64_decode(data));
                    param_html.Add("formid", encode.base64_decode(formid));

                    var result_html = await mysqlswan.data_utility(command_html, param_html);

                    break;
                case "css":
                    /// write_content(encode.base64_decode(formid), encode.base64_decode(data));
                    string command_css= "UPDATE reportpdf_template SET css = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param_css = new Dictionary<object, object>();
                    param_css.Add("value", encode.base64_decode(data));
                    param_css.Add("formid", encode.base64_decode(formid));

                    var result_css = await mysqlswan.data_utility(command_css, param_css);

                    break;
                case "js":
                  //  connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                    string command = "UPDATE reportpdf_template SET jquery = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("value", encode.base64_decode(data));
                    param.Add("formid", encode.base64_decode(formid));

                    var result = await mysqlswan.data_utility(command, param);
                    break;
            }
            return Content("");

        }

        [Proofpoint]
        public async  Task<ActionResult> Saveformstemplate_html(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();

            Stopwatch httime = new Stopwatch();
            httime.Start();

            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate Start " + httime.Elapsed.TotalMilliseconds);

            try
            {
               
                        //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                        string command_html = "UPDATE forms_template SET templates = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                        Dictionary<object, object> param_html = new Dictionary<object, object>();
                        param_html.Add("value", encode.base64_decode(data));
                        param_html.Add("formid", encode.base64_decode(formid));
                        var result_html = await mysqlswan.data_utility(command_html, param_html);


                        _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate html " + httime.Elapsed.TotalMilliseconds);

                    
                
            }
            catch (Exception e) {
                log.err("Save html error "+e.ToString());
            }

            httime.Stop();
            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate " + httime.Elapsed.TotalMilliseconds);
            return Content("Save:" + httime.Elapsed.TotalMilliseconds);

        }

        [Proofpoint]
        public async Task<ActionResult> Saveformstemplate_css(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();

            Stopwatch httime = new Stopwatch();
            httime.Start();

            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate Start " + httime.Elapsed.TotalMilliseconds);

            try
            {
                
                        // connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                        string command_css = "UPDATE forms_template SET css = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                        Dictionary<object, object> param_css = new Dictionary<object, object>();
                        param_css.Add("value", encode.base64_decode(data));
                        param_css.Add("formid", encode.base64_decode(formid));
                        var result_css = await mysqlswan.data_utility(command_css, param_css);

                        _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate css " + httime.Elapsed.TotalMilliseconds);

                        //log.info("Debug Save From css " + JsonConvert.SerializeObject(result_css));

                    
            }
            catch (Exception e)
            {
                log.err("Save html error " + e.ToString());
            }

            httime.Stop();
            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate " + httime.Elapsed.TotalMilliseconds);
            return Content("Save:" + httime.Elapsed.TotalMilliseconds);

        }


        [Proofpoint]
        public async Task<ActionResult> Saveformstemplate_js(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();

            Stopwatch httime = new Stopwatch();
            httime.Start();

            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate Start " + httime.Elapsed.TotalMilliseconds);

            try
            {
               
                        // connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                        string command_js = "UPDATE forms_template SET jquery = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                        Dictionary<object, object> param_js = new Dictionary<object, object>();
                        param_js.Add("value", encode.base64_decode(data));
                        param_js.Add("formid", encode.base64_decode(formid));
                        var result_js = await mysqlswan.data_utility(command_js, param_js);


                        _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate js " + httime.Elapsed.TotalMilliseconds);

                    
            }
            catch (Exception e)
            {
                log.err("Save html error " + e.ToString());
            }

            httime.Stop();
            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate " + httime.Elapsed.TotalMilliseconds);
            return Content("Save:" + httime.Elapsed.TotalMilliseconds);

        }

        [Proofpoint]
        public async Task<ActionResult> Saveformstemplate_json(string formid, string type, string data)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();

            Stopwatch httime = new Stopwatch();
            httime.Start();

            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate Start " + httime.Elapsed.TotalMilliseconds);

            try
            {
               
                        //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                        string command_json = "UPDATE forms_template SET config = @value   WHERE templatecode = @formid AND  status = 'Y' ";
                        Dictionary<object, object> param_json = new Dictionary<object, object>();
                        param_json.Add("value", encode.base64_decode(data));
                        param_json.Add("formid", encode.base64_decode(formid));

                        var result_json = await mysqlswan.data_utility(command_json, param_json);

                        _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate json " + httime.Elapsed.TotalMilliseconds);

                      
            }
            catch (Exception e)
            {
                log.err("Save html error " + e.ToString());
            }

            httime.Stop();
            _ = log.swan_core_log("HTML_Time_exeute", "Saveformstemplate " + httime.Elapsed.TotalMilliseconds);


            return Content("Save:" + httime.Elapsed.TotalMilliseconds);

        }

        private async Task<string> Get_config_mail(string flexid, string system)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

            string command = "select config from mail_template WHERE templatecode = @flex AND `system` = @system AND status = 'Y' ";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flex", flexid);
            param.Add("system", system);

            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0]["config"].ToString();
        }

        private async  Task<string> Get_content_mail(string flexid, string system)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            // connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

            string command = "select templates from mail_template WHERE templatecode = @flex AND `system` = @system AND status = 'Y' ";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flex", flexid);
            param.Add("system", system);

            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0]["templates"].ToString();
        }

        private async Task<string> Get_config_flex(string flexid,string system ) {
            Mysqlswan mysqlswan = new Mysqlswan();
            //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
            string command = "select config from line_flex_template WHERE templatecode = @flex AND `system` = @system AND status = 'Y' ";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flex", flexid);
            param.Add("system", system);

            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0]["config"].ToString();
        }

        private async Task<string> Get_content_flex(string flexid, string system)
        {
            Mysqlswan mysqlswan = new Mysqlswan();
            //connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
            string command = "select templates from line_flex_template WHERE templatecode = @flex AND `system` = @system AND status = 'Y' ";

            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("flex", flexid);
            param.Add("system", system);

            var data = await mysqlswan.data_with_col(command, param);
            var Jdata = JsonConvert.SerializeObject(data);

            JArray jArray = JArray.Parse(Jdata);
            return jArray[0]["templates"].ToString();
        }
    
        public async Task<string> Get_content_html(string request_form)
        {
            // string content_html = Path.Combine(Server.MapPath("~/App_data/app_store"), request_form + "/frm_" + request_form + "_html.master");
            // string data_content = System.IO.File.ReadAllText(content_html);
            // return data_content;
            return "";
        }
        public async Task<string> Get_content_css(string request_form)
        {
            //string content_html = Path.Combine(Server.MapPath("~/App_data/app_store"), request_form + "/frm_" + request_form + "_css.master");
            //string data_content = System.IO.File.ReadAllText(content_html);
            //return data_content;
            return "";
        }
        public async Task<string> Get_content_js(string request_form)
        {
            //string content_html = Path.Combine(Server.MapPath("~/App_data/app_store"), request_form + "/frm_" + request_form + "_js.master");
            //string data_content = System.IO.File.ReadAllText(content_html);
            //return data_content;
            return "";
        }
        public async Task<string> Get_content_json(string data)
        {
            string path = Directory.GetCurrentDirectory();
            string data_content = System.IO.File.ReadAllText(path + "/App_data/Api/"+data+"_api.json"); 
            return data_content;
           
        }
        public async  void write_content(string filename, string text)
        {
           

            string path = Directory.GetCurrentDirectory();
            System.IO.File.WriteAllText(path+"/App_data/Api/"+filename+"_api.json",text);
            
        }

        [Proofpoint]
        public async Task<IActionResult> Project_initial()
        {
            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            Core_authen authen = new Core_authen();
            //await authen.initial_project();

            await authen.initital_user();
            //await authen.client_authen("surawat@it-element.com","1234");

            return View("~/Views/web001/Appbuild/Project_initial.cshtml");
        }

        [Proofpoint]
        public async Task<IActionResult> ListUpload()
        {
            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            return View("~/Views/web001/Tiger/Index.cshtml");
        }

      
        public List<object> fetch_data()
        {


            DirectoryInfo filePaths = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Upload/temp/");

            var list_document_temp = new List<object>();

            foreach (var fi in filePaths.GetFiles())
            {
                


                string[] document_string = fi.Name.Split('.');

                var document_temp = new Dictionary<object, object>();
                document_temp.Add("first_name", fi.Name.ToString());
                document_temp.Add("last_name", String.Join("| ", document_string.ToArray()));

                list_document_temp.Add(document_temp);

             
                //debug doc_group
               
                //debug extension


            }
            return list_document_temp;
        }


        [Proofpoint]
        public async Task<IActionResult> Swan(string apiname)
        {


            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
              


                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                ViewBag.formid = "";
                ViewBag.userid = await Client_info.info_logon(value, "userid");
                ViewBag.flowid = "";

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

                return View("~/Views/web001/CRUD/SWAN.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> FireSwan(string apiname)
        {

            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                ViewBag.formid = "";
                ViewBag.userid = await Client_info.info_logon(value, "userid");
                ViewBag.flowid = "";

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

                return View("~/Views/web001/CRUD/FireSwan.cshtml");
            }

        }


        [Proofpoint]
        public async Task<IActionResult> Tiger(string apiname)
        {

            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
               
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;


                ViewBag.formid = "";
                ViewBag.userid = await Client_info.info_logon(value, "userid");
                ViewBag.flowid = "";

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

                return View("~/Views/web001/CRUD/Tiger.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> Hawknet(string apiname)
        {

            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
               
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                ViewBag.formid = "";
                ViewBag.userid = await Client_info.info_logon(value, "userid");
                ViewBag.flowid = "";

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

                return View("~/Views/web001/CRUD/Hawknet.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> toolencode(string apiname)
        {
             
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");
           

            Encryption_model encode = new Encryption_model();
            var client_authen = await encode.encode_aes("encrypt", apiname);

           

            ViewBag.hashpassword = client_authen;

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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


            return View("~/Views/web001/Appbuild/toolencode.cshtml");
        }



        //[Proofpoint]
        public async Task<IActionResult> DatatableReport(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/DatatableReport.cshtml");
        }

        //[Proofpoint]
        public async Task<IActionResult> HighstockReport(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/HighStockReport.cshtml");
        }

        [Proofpoint]
        public async Task<IActionResult> Calendarevent(string apiname)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Calendarevent.cshtml");
        }

        [Proofpoint]
        public async Task<IActionResult> Add_user()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/AddUser.cshtml");
        }

      

        [Proofpoint]
        public async Task<IActionResult> Html()
        {

            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/Html.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> codelookup()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = "";
            ViewBag.userid = await Client_info.info_logon(value, "userid");
            ViewBag.flowid = "";

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

            return View("~/Views/web001/Appbuild/codelookup.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Proofpoint]
        public async Task<IActionResult> test_form1(string formid, string system)
        {
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = formid;
            ViewBag.system = system;

            return View("~/Views/web001/Appbuild/test_form1.cshtml");
        }
    }
}

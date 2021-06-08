using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace service.Controllers
{
   
    public class SettingController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

        public SettingController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Proofpoint]
        public async Task<IActionResult> Index(string apiname)
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

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                
                ViewBag.fetchdata = "fetch_datasq";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                return View("~/Views/web001/Appbuild/AddNew_user.cshtml");
            }
           
        }

        [Proofpoint]
        public async Task<IActionResult> Menu(string apiname)
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

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
               
                ViewBag.fetchdata = "fetch_datasq";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                return View("~/Views/web001/Appbuild/AddNew_user.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> Swan(string apiname)
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

                return View("~/Views/web001/CRUD/SWAN.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> FireSwan(string apiname)
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

                return View("~/Views/web001/CRUD/FireSwan.cshtml");
            }

        }


        [Proofpoint]
        public async Task<IActionResult> Tiger(string apiname)
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

                return View("~/Views/web001/CRUD/Tiger.cshtml");
            }

        }

        [Proofpoint]
        public async Task<IActionResult> Hawknet(string apiname)
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

                return View("~/Views/web001/CRUD/Hawknet.cshtml");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

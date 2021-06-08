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
   
    public class PageController : Controller
    {
       
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

      
        [Proofpoint]
        public async Task<IActionResult> Index()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();
            Logmodel log = new Logmodel();
            var value = HttpContext.Session.GetString("FirstSeen");

            log.info("Session client login ========> > >=============> " + await Client_info.info_logon(value, "userid"));





            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;
            ViewBag.formid = "SW081";
            ViewBag.system = "SWAN";

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
            ViewBag.formid = "SW081";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            return View("~/Views/web001/Page/Index.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> Wip()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");



            var url = "";
            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;

            ViewBag.formid = "SW081";
            ViewBag.system = "SWAN";
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
            ViewBag.formid = "SW081";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(),Date.date_now());


            return View("~/Views/web001/Page/Wip.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> CC()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");
            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;

            ViewBag.formid = "SW082";
            ViewBag.system = "SWAN";
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
            ViewBag.formid = "SW082";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            return View("~/Views/web001/Page/Cc.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> Compelete()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");

            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;
            ViewBag.formid = "SW083";
            ViewBag.system = "SWAN";

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
            ViewBag.formid = "SW083";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());


            return View("~/Views/web001/Page/Compelete.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> Cancel()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");

            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;

            ViewBag.formid = "SW084";
            ViewBag.system = "SWAN";

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
            ViewBag.formid = "SW084";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());


            return View("~/Views/web001/Page/Cancel.cshtml");
        }
        [Proofpoint]
        public async Task<IActionResult> Draf()
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            var value = HttpContext.Session.GetString("FirstSeen");

            var url = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }
            ViewBag.url = url;

            ViewBag.formid = "SW085";
            ViewBag.system = "SWAN";

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
            ViewBag.formid = "SW085";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            return View("~/Views/web001/Page/Draf.cshtml");
        }
      

                 
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

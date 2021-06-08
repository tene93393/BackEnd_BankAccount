using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace service.Controllers
{

    public class DatamanagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

        public ClaimsIdentity Subject { get; private set; }

       

        public DatamanagementController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Proofpoint]
        public async Task<IActionResult> Dashboard()
        {
            Encryption_model encode = new Encryption_model();
            var value = HttpContext.Session.GetString("FirstSeen");
            Appconfig cfg = new Appconfig();

            Stopwatch timefunction = new Stopwatch();
            timefunction.Start();

            ViewBag.formid = "";
            ViewBag.system = "";


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

            ViewBag.imge_dm = await imagedm();



           ViewBag.state = "0";
            ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

            timefunction.Stop();

            _ = log.swan_core_log("Time_exeute", "Datamanagement Controller Time : {Dashboard}" + timefunction.Elapsed.TotalMilliseconds);

            return View("~/Views/web001/Datamanagement/Dashboard.cshtml");

        }


        [Line_Proofpoint]
        public async Task<IActionResult> Liff(string access,string formid, string system)
        {
            Encryption_model encode = new Encryption_model();
            var value = HttpContext.Session.GetString("FirstSeen");
            Appconfig cfg = new Appconfig();

            try
            {
                _ = log.swan_core_log("Liff", "access " + access.ToString());
                _ = log.swan_core_log("Liff", "formid " + formid.ToString());
                _ = log.swan_core_log("Liff", "system " + system.ToString());

                var page_selected = "";

                Stopwatch timefunction = new Stopwatch();
                timefunction.Start();

                ViewBag.formid = formid;
                ViewBag.system = system;

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

                ViewBag.imge_dm = await imagedm();




                switch (access)
                {
                    case "Info":
                        page_selected = "Info";
                        break;
                    case "Wip":
                        page_selected = "Wip";
                        break;
                    case "Port":
                        page_selected = "Port";
                        break;
                    case "Request":
                        page_selected = "Request";
                        break;
                    case "Noti":
                        page_selected = "Noti";
                        break;
                    case "Setting":
                        page_selected = "Setting";
                        break;
                    default:
                        page_selected = "Dashboard";
                        break;


                }



                ViewBag.state = "0";
                ViewBag.anticsrf_token = encode.Encrypt(cfg.initial_config("swan_key").ToString(), Date.date_now());

                timefunction.Stop();

                _ = log.swan_core_log("Time_exeute", "Datamanagement Controller Time : {Dashboard}" + timefunction.Elapsed.TotalMilliseconds);

                return View("~/Views/web001/Liff/" + page_selected + ".cshtml");
            }
            catch (Exception e) {

                _ = log.swan_core_log("Liff", "Datamanagement Error" + e.ToString());

                return View("~/Views/web001/Liff/authen_line.cshtml");
            }

        }


      
        public async Task<IActionResult> Line_authen()
        {
            try
            {
                _ = log.swan_core_log("Liff", "Line_authen Request authen");
                Encryption_model encode = new Encryption_model();

                ViewBag.imge_dm = await imagedm();

                return View("~/Views/web001/Liff/Authen_line.cshtml");
            }
            catch (Exception e) {
                _ = log.swan_core_log("Liff", "Line_authen Error" + e.ToString());
                return View("~/Views/web001/Liff/authen_line.cshtml");
            }
        }


        public async Task<IActionResult> Line_verifly(string linetoken)
        {
            Encryption_model encode = new Encryption_model();
            Logmodel log = new Logmodel();

            try
            {

                _ = log.swan_core_log("Liff", "Request line auten is : " + linetoken);



                if (linetoken.ToString() == "")
                {

                    return Content("404");

                }




                const string sessionKey = "FirstSeen";
                var value = HttpContext.Session.GetString(sessionKey);


                Core_authen authen = new Core_authen();
                var client_authen = await authen.client_authen_lineliff(linetoken);
                var sess_login = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();


                _ = log.swan_core_log("Authen", "Authen Status : " + JsonConvert.SerializeObject(sess_login));

                if (sess_login["status"].ToString() == "fail")
                {

                    return Content("404");

                }
                else if (sess_login["status"].ToString() == "Pass")
                {
                    var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, sess_login["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, sess_login["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, sess_login["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName,sess_login["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));


                    var token = new JwtSecurityToken(
                         issuer: _configuration["JwtIssuer"],
                         audience: _configuration["JwtAudience"],
                         claims: claims,
                         expires: expires,
                         signingCredentials: creds
                  );


                    var tokene_login = new JwtSecurityTokenHandler().WriteToken(token);
                    Response.Cookies.Delete(".AspNetCore.CookieAuthentication");


                    _ = log.swan_core_log("Liff", "line auten is : " + linetoken);
                    _ = log.swan_core_log("Liff", "SWAN Token is : " + tokene_login);

                    string varidatetoken = tokene_login;
                    await authen.client_update_authentication_mssql("login", sess_login["email"].ToString(), varidatetoken.ToString());


                    var client_info = await service.Models.Client_info.information_all(tokene_login);
                    var client_data = JObject.Parse(client_info);


                    HttpContext.Session.SetString(sessionKey, varidatetoken);
                    HttpContext.Session.SetString("client_profile", client_data.ToString());
                    HttpContext.Session.SetString("userid", client_data["userid"].ToString());
                }


                return Content("200");

            } catch (Exception e) {

                _ = log.swan_core_log("Liff", "Line_verifly Error" + e.ToString());
                return Content("404");
            }

        }






        private static async Task<string> imagedm() {

            Appconfig config = new Appconfig();

            Logmodel log = new Logmodel();

            var site = config.initial_config("Data_vender");

            log.swan_core_log("Style","valueload image "+ site);

            var imgdata = "";

            if (site == "FSS")
            {
                imgdata = "data:image/png;base64,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";
            }
            else if (site == "ASP")
            {
                imgdata = " data:image/png;base64,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";
            }
            else {

                imgdata = "";
            }


            return imgdata;
        } 


        [Proofpoint]
        public async Task<IActionResult> Index(string formid, string system)
        {
            Encryption_model encode = new Encryption_model();
            Appconfig cfg = new Appconfig();

            Stopwatch timefunction = new Stopwatch();
            timefunction.Start();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");

            ViewBag.formid = formid;
            ViewBag.system = system;

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



            timefunction.Stop();

            _ = log.swan_core_log("Time_exeute", "Datamanagement Controller Time : {" + system + "}" + timefunction.Elapsed.TotalMilliseconds);

            return View("~/Views/web001/Datamanagement/Index.cshtml");
        }

        [Proofpoint]
        public IActionResult Swan(string apiname)
        {         
            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                var value = HttpContext.Session.GetString("FirstSeen");              
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                return View("~/Views/web001/CRUD/SWAN.cshtml");
            }
        }

        [Proofpoint]
        public IActionResult FireSwan(string apiname)
        {
            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                var value = HttpContext.Session.GetString("FirstSeen");


                log.info("Session in page : " + value.ToString());
                log.info("Home Index");
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                return View("~/Views/web001/CRUD/FireSwan.cshtml");
            }

        }


        [Proofpoint]
        public IActionResult Tiger(string apiname)
        {

          
            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                var value = HttpContext.Session.GetString("FirstSeen");


            
                ViewBag.fetchdata = "fetch_data";
                ViewBag.method = "action";
                ViewBag.warningmessage = @service.Models.Language.login_language("200", @service.Models.Language.default_lang());

                ViewBag.api = apiname;
                ViewBag.token = value;

                return View("~/Views/web001/CRUD/Tiger.cshtml");
            }

        }

        [Proofpoint]
        public IActionResult Hawknet(string apiname)
        {

            if (String.IsNullOrEmpty(apiname))
            {
                return View("~/Views/web001/Setting/Index.cshtml");
            }
            else
            {
                var value = HttpContext.Session.GetString("FirstSeen");


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

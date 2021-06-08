using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace service.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        const string SessionName = "_Name";
        const string SessionAge = "_Age";

        public ClaimsIdentity Subject { get; private set; }

        public Logmodel log = new Logmodel();
        Mail_Data mail = new Mail_Data();
        Core_authen authentication = new Core_authen();
        Encryption_model encode = new Encryption_model();

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: /<controller>/
        public IActionResult Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/web001/Account/index.cshtml");
        }

        public IActionResult OTP(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("OTP", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/web001/Account/OTP.cshtml");
        }

        public IActionResult PIN(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PIN");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/web001/Account/PIN.cshtml");
        }


        public IActionResult forgotpassword()
        {
            return View("~/Views/web001/Account/forgotpassword.cshtml");
        }

      


        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/web001/Account/login.cshtml");
        }


        private static async Task<string> cfg_config(string variable1)
        {
            string path = Directory.GetCurrentDirectory();
            string configtext = System.IO.File.ReadAllText(path + "/Data/Appsetting.json");
            JObject config_parse = JObject.Parse(configtext);
            return config_parse[variable1].ToString().Trim();
        }

        [HttpPost]
        public async Task<IActionResult> Adduser()
        {
            Core_authen authen = new Core_authen();
            Logmodel log = new Logmodel();

            var status_fun = new Dictionary<object, object>();
            var data_new_user = new Dictionary<object, object>();

            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }



            try
            {
                var temp_data = JObject.Parse(body);

                switch (temp_data["apiname"].ToString())
                {
                    case "localdb":

                        Random r = new Random();
                        int rnum = r.Next(1000000);
                        string n_running = rnum.ToString("D6");
                        var date_run = DateTime.Now.ToString("yyyyMMdd");

                        data_new_user.Add("userid", date_run+n_running);
                        data_new_user.Add("name", temp_data["name"].ToString());
                        data_new_user.Add("email", temp_data["email"].ToString());
                        data_new_user.Add("System", "System");
                        data_new_user.Add("th_name", temp_data["th_name"].ToString());
                        data_new_user.Add("en_name", temp_data["en_name"].ToString());
                      
                        var status_localdb = await authen.new_user_login(JsonConvert.SerializeObject(data_new_user));
                        status_fun.Add("status", status_localdb);


                        break;
                    case "ldap":

                        var status_ldap = await authen.new_user_login(JsonConvert.SerializeObject(data_new_user));
                        status_fun.Add("status", status_ldap);

                        break;
                    default:
                        status_fun.Add("status", "Error : NO functions");
                        break;
                }
            }
            catch(Exception e) {
                status_fun.Add("status", "Error : "+ e.ToString());

            }
            return Json(status_fun);
        }


            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            Stopwatch timefunction = new Stopwatch();
            timefunction.Start();

            try
            {

                const string sessionKey = "FirstSeen";
                var value = HttpContext.Session.GetString(sessionKey);

                if (model.Email == "" || model.Password == "" || model.Email == null || model.Password == null)
                {
                    return RedirectToAction("Index", "Account");
                }
                Core_authen authen = new Core_authen();
                //var client_authen = await authen.client_authen_mysql(model.Email, await encode.encode_aes("encrypt", model.Password));
                var client_authen = await authen.client_authen_mysql(model.Email, model.Password);
                var sess_login = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();

                if (sess_login["status"].ToString() == "fail")
                {

                    return RedirectToAction("Index", "Account");

                }
                else if (sess_login["status"].ToString() == "Pass")
                {
                    var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, model.Email),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName,model.Email),
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

                    // string varidatetoken = await gen_session();

                    string varidatetoken = tokene_login;
                    await authen.client_update_authentication_mssql("login", model.Email, varidatetoken.ToString());

                    _ = log.swan_core_log("authen_ldap", "token : " + varidatetoken);


                    var client_info = await service.Models.Client_info.information_all(tokene_login);
                    var client_data = JObject.Parse(client_info);


                    _ = log.swan_core_log("c", "client_data : " + JsonConvert.SerializeObject(client_data));

                    HttpContext.Session.SetString(sessionKey, varidatetoken);
                    HttpContext.Session.SetString("client_profile", client_data.ToString());
                    HttpContext.Session.SetString("userid", client_data["userid"].ToString());

                    // Batch Send mail login

                    var param_batch = new Dictionary<object, object>();
                    param_batch.Add("email", model.Email);
                    param_batch.Add("mailtemplate", "M001");
                    await Batch_swan.Add_batch_job("notification_email", JsonConvert.SerializeObject(param_batch));


                    timefunction.Stop();

                    _ = log.swan_core_log("Time_exeute", "Authen Page Time :" + timefunction.Elapsed.TotalMilliseconds);

                    return RedirectToAction(await cfg_config("screen_page"), await cfg_config("default_page"));
                }
            } catch (Exception e) {

                _ = log.swan_core_log("authen_ldap", "Error Authen page : " + e.ToString());

            }
            return View("~/Views/web001/Account/login.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> forgot_password(string email, string returnUrl = null)
        {
            const string sessionKey = "FirstSeen";

            var value = HttpContext.Session.GetString(sessionKey);

            Core_authen authen = new Core_authen();

         
            var client_email = await authen.client_email_check(email);

          
            var sess_login = JsonConvert.DeserializeObject<JArray>(client_email).ToObject<List<JObject>>().FirstOrDefault();

          
            if (sess_login["status"].ToString() == "fail")
            {
              
                return RedirectToAction("Index", "Account");
            }
            else if (sess_login["status"].ToString() == "Pass")
            {

               
                string txt_newpass = encode.sha("welcome"+DateTime.Now.ToString());
                await authen.client_update_authentication_mssql("reset",email, await encode.encode_aes("encrypt", txt_newpass));


                var maildata = new Dictionary<object, object>();
                maildata.Add("subject", "แจ้งการเข้าใช้งาน / Notice of Forgot Password");
                maildata.Add("{{content1}}", " ท่านได้ทำรายการ Reset รหัสผ่าน  Cloud @it-element ");
                maildata.Add("{{content2}}", "  เมื่อ วันที่  " + DateTime.Now.ToString("dddd, dd MM yyyy hh:mm:ss") + " ได้ทำการ สร้างรหัสผ่านใหม่ คือ    :  " + txt_newpass);
                maildata.Add("{{content99}}", "Notice of Forgot Password");

                await Mail_Data.Mail_single_sender("SWAN", "M002", email, JsonConvert.SerializeObject(maildata));



                return RedirectToAction("Index", "Account");
            }
            return View("~/Views/web001/Account/login.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> get_token()
        {
           
            var session_consent = HttpContext.Session.GetString("FirstSeen");
            Core_authen authen = new Core_authen();
            var client_authen = await authen.consent_token(session_consent);

            log.info(" Log  client get_token " + client_authen.ToString());

            var sess_state = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();
            log.info(" Session get_token : " + sess_state.ToString());

            if (sess_state["status"].ToString() == "fail")
            {

                log.info(" Id login is Fail : " + session_consent);
                return RedirectToAction("Index", "Account");

            }
            else if (sess_state["status"].ToString() == "Pass")
            {

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, sess_state["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, sess_state["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, sess_state["clientid"].ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, sess_state["email"].ToString()),
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

                ViewBag.token = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
            }

            return View("~/Views/web001/Account/get_token.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Logoff()
        {


            var session_consent = HttpContext.Session.GetString("FirstSeen");

            if (String.IsNullOrEmpty(session_consent))
            {
                return RedirectToAction("Index", "Account");
            }

           
            Core_authen authen = new Core_authen();
            await authen.client_update_authentication_mssql("logout", "", session_consent.ToString());
            await Task.Run(() => HttpContext.Session.Clear());

            return RedirectToAction("Index", "Account");
            //return View();
        }


        private async Task<string>  gen_session() {

            var session_consent = HttpContext.Session.GetString("FirstSeen");

            Core_authen authen = new Core_authen();

            var client_authen = await authen.consent_token(session_consent);

            log.info(" Log  client get_token " + client_authen.ToString());

            var sess_state = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();

            log.info(" Session get_token : " + sess_state.ToString());

            if (sess_state["status"].ToString() == "fail")
            {
                log.info(" Id login is Fail : " + session_consent);
                return await Task.Run(() => "Fail");

            }
      
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, sess_state["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, sess_state["email"].ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, sess_state["clientid"].ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, sess_state["email"].ToString()),
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


              return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> Loginmobile(LoginViewModel model, string returnUrl = null)
        {

            log.info(" Log start  ================================================> mobile authen ");

            const string sessionKey = "FirstSeen";

            var value = HttpContext.Session.GetString(sessionKey);

            if (model.Email == "" || model.Password == "" || model.Email == null || model.Password == null)
            {
                return "In correct data";
            }



            Core_authen authen = new Core_authen();

            var client_authen = await authen.client_authen_mssql(model.Email, await encode.encode_aes("encrypt", model.Password));
            var sess_login = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();

            if (sess_login["status"].ToString() == "fail")
            {

                log.info(" Id login is Fail : " + model.Email);
                return "Id login is Fail";

            }
            else if (sess_login["status"].ToString() == "Pass")
            {

                log.info(" Id login is Pass : ");


                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, model.Email),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName,model.Email),
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

                //var claims = new List<Claim>
                //{
                //    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                //    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                //    new Claim(JwtRegisteredClaimNames.NameId, sess_login["clientid"].ToString()),
                //    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                //    new Claim(JwtRegisteredClaimNames.GivenName, sess_login["clientid"].ToString()),
                //    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //};

                //var userIdentity = new ClaimsIdentity(claims, "login");
                //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                //await HttpContext.Authentication.SignOutAsync("CookieAuthentication");
                Response.Cookies.Delete(".AspNetCore.CookieAuthentication");

                // string varidatetoken = await gen_session();

                string varidatetoken = tokene_login;

                await authen.client_update_authentication_mssql("login", model.Email, varidatetoken.ToString());


                var client_info = await service.Models.Client_info.information_all(tokene_login);
                var client_data = JObject.Parse(client_info);


                HttpContext.Session.SetString(sessionKey, varidatetoken);
                HttpContext.Session.SetString("client_profile", client_data.ToString());


                //mail.send_maillogin(model.Email);


              

                var param_batch = new Dictionary<object, object>();
                param_batch.Add("email", model.Email);
                param_batch.Add("mailtemplate", "M001");


                await Batch_swan.Add_batch_job("notification_email", JsonConvert.SerializeObject(param_batch));

                //Otp otp = new Otp();
                //await otp.otp_new("email", "SWAN", "SWAN", model.Email);

                Encryption_model encode = new Encryption_model();

                return encode.base64_encode(varidatetoken.ToString());
                /// return RedirectToAction("Index", "Page");
            }
            return "Wrong login";
            //return View("~/Views/web001/Account/login.cshtml");
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Dictionary<object,object>> Authentiger(LoginViewModel model, string returnUrl = null)
        {

            Logmodel log = new Logmodel();
            Dictionary<object, object> tiger_result_auth = new Dictionary<object, object>();

            const string sessionKey = "FirstSeen";



            _ = log.swan_core_log("Tiger_upload", "Authen Email " + model.Email);
            _ = log.swan_core_log("Tiger_upload", "Authen Password " + model.Password);







            var value = HttpContext.Session.GetString(sessionKey);

            if (model.Email == "" || model.Password == "" || model.Email == null || model.Password == null)
            {
                //return tiger_result_auth.Add("status", "In correct data");
                 tiger_result_auth.Add("status", "In correct data");
            }



            Core_authen authen = new Core_authen();

            var client_authen = await authen.client_authen_mssql(model.Email, await encode.encode_aes("encrypt", model.Password));
            var sess_login = JsonConvert.DeserializeObject<JArray>(client_authen).ToObject<List<JObject>>().FirstOrDefault();


          
            if (sess_login["status"].ToString() == "fail")
            {
                //return tiger_result_auth.Add("status", "Id login is Fail");
                 tiger_result_auth.Add("status", "Id login is Fail");

            }
            else if (sess_login["status"].ToString() == "Pass")
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, model.Email),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName,model.Email),
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

            
                string varidatetoken = tokene_login;



                var tiger_check = await authen.role_tiger(sess_login["clientid"].ToString());
                var tiger_result_check = JsonConvert.DeserializeObject<JArray>(tiger_check).ToObject<List<JObject>>().FirstOrDefault();


                _ = log.swan_core_log("Tiger_upload", "Return tiger_check " + JsonConvert.SerializeObject(tiger_result_check));


                if (tiger_result_check["status"].ToString() == "fail")
                {

                    tiger_result_auth.Add("status", "Don't Permission ");

                }
                else {

                    Encryption_model encode = new Encryption_model();

                    tiger_result_auth.Add("token", encode.base64_encode(varidatetoken.ToString()));
                    tiger_result_auth.Add("clientid", sess_login["clientid"].ToString());
                    tiger_result_auth.Add("email", sess_login["email"].ToString());
                    tiger_result_auth.Add("th_name", sess_login["th_name"].ToString());
                    tiger_result_auth.Add("en_name", sess_login["en_name"].ToString());
                    tiger_result_auth.Add("date", Date.date_now());


                    await authen.client_update_authentication_mssql("login", model.Email, varidatetoken.ToString());


                    var client_info = await service.Models.Client_info.information_all(tokene_login);
                    var client_data = JObject.Parse(client_info);


                    HttpContext.Session.SetString(sessionKey, varidatetoken);
                    HttpContext.Session.SetString("client_profile", client_data.ToString());

                    //mail.send_maillogin(model.Email);


                    var maildata = new Dictionary<object, object>();
                    maildata.Add("subject", "แจ้งการเข้าใช้งาน mobile Application / Notice of Log In Mobile Application");
                    maildata.Add("{{content1}}", "ท่านผู้ใช้งาน " + model.Email + " ท่านเข้าใช้งานระบบ Cloud @it-element ");
                    maildata.Add("{{content2}}", " เมื่อ วันที่  " + DateTime.Now.ToString("dddd, dd MM yyyy hh:mm:ss"));
                    maildata.Add("{{content99}}", "Notice of Log In");

                    //await mail.Mail_single_sender("SWAN", "M001", model.Email, JsonConvert.SerializeObject(maildata));

                    //Otp otp = new Otp();
                    //await otp.otp_new("email", "SWAN", "SWAN", model.Email);


                }


                _ = log.swan_core_log("Tiger_upload", "Return Status " + JsonConvert.SerializeObject(tiger_result_auth));

                return tiger_result_auth;
                /// return RedirectToAction("Index", "Page");
            }

           
           
            tiger_result_auth.Add("status", "Wrong login");

            return tiger_result_auth;

           
        }

    }
}

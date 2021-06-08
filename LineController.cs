using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace service.Controllers
{

    public class LineController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();
        Mail_Data mail = new Mail_Data();
        private List<object>  temp_data_line_alert;

        public LineController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            // const string sessionKey = "FirstSeen";
            //var value = HttpContext.Session.GetString("FirstSeen");

            var body = "";

            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            string channel = HttpContext.Request.Query["ch"];

            log.info("Channel Request : " + channel);
            log.info("mesage income : " + body.ToString());


            var exits_line_account = await Task.Run(() => Line_Data.Line_exits_user(channel, Line_message.line_userid(body.ToString())));
            log.info("exits_line_account : " + exits_line_account.ToString());
            var detail_account = JsonConvert.DeserializeObject<JArray>(exits_line_account).ToObject<List<JObject>>().FirstOrDefault();


            if (String.IsNullOrEmpty(detail_account["status"].ToString()))
            {

            } else if (detail_account["status"].ToString()=="New") {

                log.info("New user Create Otp By Email : ");

                //Otp otp = new Otp();
                //await otp.otp_new("email", "SWAN", "SWAN", model.Email);

                //=====================================================================
                var reply_word = new Dictionary<object, object>();

                JObject obj = JObject.Parse(body.ToString());

                string message_type = obj["events"][0]["message"]["type"].ToString();
                switch (message_type)
                {

                    case "image":
                        reply_word.Add("type", "text");
                        reply_word.Add("text", " เราไม่สามารถรับรูปของท่านได้ ");

                        break;
                    case "message":
                        break;
                    case "text":
                        
                        log.info("check Email this word : " + obj["events"][0]["message"]["text"].ToString());

                        string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

                        if (Regex.IsMatch(obj["events"][0]["message"]["text"].ToString(), pattern))
                        {

                            log.info("check Email match : ");
                            //return true;
                            string chk_email = obj["events"][0]["message"]["text"].ToString().ToLower();

                                var exits_email_account = await Task.Run(() => Line_Data.Exits_email_user(chk_email));
                                log.info("exits_email_account : " + exits_email_account.ToString());
                                var exits_email_result = JsonConvert.DeserializeObject<JArray>(exits_email_account).ToObject<List<JObject>>().FirstOrDefault();

                                if (String.IsNullOrEmpty(exits_email_result["status"].ToString()))
                                {
                                reply_word.Add("type", "text");
                                reply_word.Add("text", "กรุณาระบุอีเมล์ ที่ลงทะเบียนไว้กับระบบ หรือ ตรวจสอบ Link Activate ที่อีเมล์ของท่าน : ");

                                }
                                else if (exits_email_result["status"].ToString() == "New")
                                {
                                    reply_word.Add("type", "text");
                                    reply_word.Add("text", "กรุณาระบุอีเมล์ ที่ลงทะเบียนไว้กับระบบ หรือ ตรวจสอบ Link Activate ที่อีเมล์ของท่าน : ");

                                }
                                else if (exits_email_result["status"].ToString() == "Exits")
                                {
                                    log.info("exits_email_account status : Exits ");

                                    reply_word.Add("type", "text");
                                    reply_word.Add("text", "ท่านเข้าใช้งาน Line  : "+channel+"  ได้แล้วครับ");

                                   await Line_Data.add_user_line(channel, chk_email, Line_message.line_userid(body.ToString()));


                                    var maildata = new Dictionary<object, object>();
                                    maildata.Add("subject", "แจ้งการเข้าใช้งาน Line / Notice of Register Line Account");
                                    maildata.Add("{{content1}}", "ท่านผู้ใช้งาน " + chk_email + " ท่าน สามารถเข้าใช้งานระบบ : "+ channel+" โดย ทำรายการ เมื่อ วันที่  " + DateTime.Now.ToString("dddd, dd MM yyyy hh: mm:ss"));
                                    maildata.Add("{{content2}}", " xxxxxxxxxxxx ");

                                    await Mail_Data.Mail_single_sender("SWAN", "M001", chk_email, JsonConvert.SerializeObject(maildata));

                            }
                                else {

                                reply_word.Add("type", "text");
                                reply_word.Add("text", "กรุณาระบุอีเมล์ ที่ลงทะเบียนไว้กับระบบ หรือ ตรวจสอบ Link Activate ที่อีเมล์ของท่าน : ");


                            }

                        }
                        else{

                            reply_word.Add("type", "text");
                            reply_word.Add("text", "กรุณาระบุอีเมล์ ที่ลงทะเบียนไว้กับระบบ หรือ ตรวจสอบ Link Activate ที่อีเมล์ของท่าน : ");

                        }



                        break;

                    case "postback":

                        break;

                    default:

                        reply_word.Add("type", "text");
                        reply_word.Add("text", " ไม่เข้าใจ ข้อความ หรือการกระทำ ของท่าน โปรดลองใหม่อีกครั้ง ");


                        break;

                }
                //=======================================================================
                var msg_return = JsonConvert.SerializeObject(reply_word);

                Line_Data line_data = new Line_Data();
                Line_message linemessage = new Line_message();
                var status = Line_message.line_reply_message(Line_message.line_userid(body.ToString()), msg_return, line_data.channel_access_token(channel));

                log.info("Log answer email new user  : " + status.ToString());

            }
            else {

                connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
                Line_message.line_extract_message_db(channel, body.ToString());
                Line_reply_message(channel, body.ToString());


            }

            return View("~/Views/web001/Line/Index.cshtml");
        }


        [HttpPost]
        public async  Task<string> line_message_feed()
        {
           
            var body = "";

            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }
            var request_data = JObject.Parse(body);
            string channel = request_data["ch"].ToString();
         


            switch (channel)
            {
                case "stardust":

                temp_data_line_alert = await Line_Data.line_notification_tab("stardust", "ite-000120190421-101");

                    break;

                case "fireswan":

                temp_data_line_alert = await Line_Data.line_notification_tab("fireswan", "ite-000120190421-101");

                    break;

                case "fireswan_wealth":

                temp_data_line_alert = await Line_Data.line_notification_tab("fireswan_wealth", "ite-000120190421-101");

                    break;
                case "Ite_Securities":

                temp_data_line_alert = await Line_Data.line_notification_tab("Ite_Securities", "ite-000120190421-101");

                    break;
                case "cseal":
                
                temp_data_line_alert = await Line_Data.line_notification_tab("cseal", "ite-000120190421-101");

                    break;

                default:

                    break;
            }

            return JsonConvert.SerializeObject(temp_data_line_alert);

        }

        [Proofpoint]
        public async  Task<IActionResult> line_sim(string channel, string templateid, string role) {
            Line_Data linedata = new Line_Data();
            await linedata.Line_template(channel, templateid, role);

            return View("~/Views/web001/Line/Debug.cshtml");
        }


        [Proofpoint]
        public async Task<IActionResult> Debug()
        {
            connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

            var resutlt = await Line_chatbot.bot_message("ดีครับ");

            return View("~/Views/web001/Line/Debug.cshtml");
        }


       
        public async Task<IActionResult> bash_message(string bash_token)
        {
            if (bash_token == "SJnZvKplSH9wuOoh5ScvqlS")
            {
                string filejson = DateTime.Now.ToString("MMddyyyyHHmmss");

                var img_link = new Dictionary<object, object>();
                img_link.Add("Monday", "https://service.apptest.website/img/Mon.jpg");
                img_link.Add("Tuesday", "https://service.apptest.website/img/Tues.jpg");
                img_link.Add("Tednesday", "https://service.apptest.website/img/wednesday.png");
                img_link.Add("Thursday", "https://service.apptest.website/img/tursday.png");
                img_link.Add("Friday", "https://service.apptest.website/img/friday.png");



                string path = Directory.GetCurrentDirectory();
                string msg = System.IO.File.ReadAllText(path + "/App_data/Flex/200.json");



                var arr_replace = new Dictionary<object, object>();
                arr_replace.Add("{{imageday}}", img_link[DateTime.Now.ToString("dddd").ToString()].ToString());
                arr_replace.Add("{{urlsite}}", "https://service.apptest.website");



                var send_user = new Dictionary<object, object>();
                send_user.Add("1", "U9b9c2da598a0517a8a42ecebe415e286");
                send_user.Add("2", "U2f6c58b7983e19b26137f26fa3bac19f");
                send_user.Add("3", "Uf5f1873294cbcc8640f3e24edde2d8a1");
                send_user.Add("4", "U5e8843e4bf6c9b6f454aa7797e575ba9");


                foreach (var item in arr_replace)
                {
                    msg = replace_line(msg, item.Key.ToString(), item.Value.ToString());
                }

                System.IO.File.WriteAllText(path + "/App_data/Flex/" + filejson + ".json", msg);
                string msg_send = System.IO.File.ReadAllText(path + "/App_data/Flex/" + filejson + ".json");
                string patten = msg_send;

                string channel = "stardust";

                foreach (var itemuser in send_user)
                {
                    var sendstatus = await Task.Run(() => Line_message.flex_message_bash(channel, patten, itemuser.Value.ToString()));
                    log.info("send Line bash status : " + sendstatus.ToString());
                }
            }
            return View("~/Views/web001/Line/flex.cshtml");
        }

        private string replace_line(string txt, string patten, string replace)
        {

            return txt.Replace(patten, replace);
        }

        private async void Line_reply_message(string channel, string data)
        {
            Line_Data linedata = new Line_Data();
           

            connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");
           
            string access_token = linedata.channel_access_token(channel);

            JObject obj = JObject.Parse(data);

            log.line("Log JObject message line parse ");


            string lineid = obj["events"][0]["source"]["userId"].ToString();
            string type_message = obj["events"][0]["type"].ToString();
            string replyToken = obj["events"][0]["replyToken"].ToString();
            string message_type = obj["events"][0]["message"]["type"].ToString();
            string message_id = obj["events"][0]["message"]["id"].ToString();



            log.line("Log message_type :" + message_type);

           

            string msg_return = "";
            var reply_word = new Dictionary<object, object>();

            switch (message_type)
            {

                case "image":

                    log.line("Log start download image");

                    var client_download = new RestClient("https://api.line.me/v2/bot/message/" + message_id + "/content");
                    var request_download = new RestRequest(Method.GET);
                    request_download.AddHeader("Cache-Control", "no-cache");
                    request_download.AddHeader("Accept", "*/*");

                    request_download.AddHeader("Authorization", "Bearer " + access_token);
                    IRestResponse response = client_download.Execute(request_download);


                    string path = Directory.GetCurrentDirectory() + "/Upload/temp/" + message_id + ".jpeg";

                    System.IO.File.WriteAllBytes(path, response.RawBytes);

                    log.line("write file");

                    string base64String = "";

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }


                    log.line("enocde base64 ");

                    connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "h3_db");
                    Encryption_model encode = new Encryption_model();
                    var hash = await encode.encode_aes("encrypt", base64String);
                    Edocumentmng.insertDocument(connect_extension.connect_db, base64String, message_id + ".jpg", "Line Content",hash,"", "webupload", "web");

                    reply_word.Add("type", "text");
                    reply_word.Add("text", "เราได้รับ ไพล์ที่ส่งมาแล้ว");
                    msg_return = JsonConvert.SerializeObject(reply_word);


                    break;
                case "message":

                    string message_text = obj["events"][0]["message"]["text"].ToString();
                    log.line("Log ans text : "+message_text);


                    var temp_word = await Line_chatbot.bot_message(message_text);
                    log.info("Temp word is : " + temp_word.ToString());
                    var word_detail = JsonConvert.DeserializeObject<JArray>(temp_word).ToObject<List<JObject>>().FirstOrDefault();

                    if (String.IsNullOrEmpty(word_detail["answer"].ToString())) {

                        log.info("Word Respond " + temp_word);

                        reply_word.Add("type", "text");
                        reply_word.Add("text", "ไม่เข้าใจสิ่งที่ส่งมา");

                        msg_return = JsonConvert.SerializeObject(reply_word);

                        log.info("Word meg_return " + msg_return);

                    }
                    else {

                        log.info("Word Respond " + temp_word);

                        reply_word.Add("type", "text");
                        reply_word.Add("text", word_detail["answer"].ToString());

                        msg_return = JsonConvert.SerializeObject(reply_word);

                        log.info("Word meg_return " + msg_return);

                    }
                    break;
                case "text":

                    string message_text_txt = obj["events"][0]["message"]["text"].ToString();
                    log.line("Log ans text : " + message_text_txt);


                    var temp_word_txt = await Line_chatbot.bot_message(message_text_txt);
                    log.info("Temp word is : " + temp_word_txt.ToString());
                    var word_detail_txt = JsonConvert.DeserializeObject<JArray>(temp_word_txt).ToObject<List<JObject>>().FirstOrDefault();

                    if (String.IsNullOrEmpty(word_detail_txt["answer"].ToString()))
                    {

                        log.info("Word Respond " + temp_word_txt);

                        reply_word.Add("type", "text");
                        reply_word.Add("text", "ไม่เข้าใจสิ่งที่ส่งมา"); 
                        msg_return = JsonConvert.SerializeObject(reply_word);

                        log.info("Word meg_return " + msg_return);

                    }
                    else
                    {

                        log.info("Word Respond " + temp_word_txt);

                        reply_word.Add("type", "text");
                        reply_word.Add("text", word_detail_txt["answer"].ToString());

                        msg_return = JsonConvert.SerializeObject(reply_word);

                        log.info("Word meg_return " + msg_return);

                    }
                    break;

                case "postback":

                    message_text = obj["events"][0]["postback"]["data"].ToString();

                    reply_word.Add("type", "text");
                    reply_word.Add("text", "กำลัง ดำเนืนการตามที่สั่งน่ะครับ");
                    msg_return = JsonConvert.SerializeObject(reply_word);

                    break;

                default:

                    reply_word.Add("type", "text");
                    reply_word.Add("text", "ไม่เข้าใจสิ่งที่ส่งมา");
                    msg_return = JsonConvert.SerializeObject(reply_word);


                    break;

            }

            Line_message linemessage = new Line_message();
            var status = Line_message.line_reply_message(lineid, msg_return, access_token);
            log.info("Log response_line  : " + status.ToString());

        }

      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

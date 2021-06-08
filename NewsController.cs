using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using service.Models;

namespace service.Controllers
{
    public class NewsController : Controller
    {
       
        Logmodel log = new Logmodel();
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View("~/Views/web001/News/Index.cshtml");
        }
        public IActionResult Postnews()
        {
            return View();
        }
        [HttpPost]
        public IActionResult news_feed()
        {
            Encryption_model encode = new Encryption_model();
            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }
            //var request_data = JObject.Parse(body);

            log.info("Iswan receive_message " + body.ToString());
            log.info("Iswan receive_message " + encode.base64_decode(body.ToString()));

            var request_data = JObject.Parse(encode.base64_decode(body.ToString()));
            return Content("Receivedata : " + Utility.numConvertChar("1020000.21"));
        }

        [HttpPost]
        public IActionResult receive_message()
        {
            Encryption_model encode = new Encryption_model();
            var body = "";
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }
            //var request_data = JObject.Parse(body);

            log.info("Iswan receive_message " + body.ToString());
            log.info("Iswan receive_message " + encode.base64_decode(body.ToString()));

            var request_data = JObject.Parse(encode.base64_decode(body.ToString()));
            return Content("Receivedata : "+Utility.numConvertChar("1020000.21"));
        }



    }
}

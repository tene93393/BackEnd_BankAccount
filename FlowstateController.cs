using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;

namespace service.Controllers.Management
{
    public class FlowstateController : Controller
    {
        Logmodel log = new Logmodel();
        private string body;

        public IActionResult Index()
        {
            return View("~/Views/web001/Createstate/Index.cshtml");
        }

        //public string CreateJson(string format1,string format2) {
        //    JObject videogameRatings = new JObject(
        //    new JProperty("formname", format1),
        //    new JProperty("formtype", format2));

        //    var path = @Directory.GetCurrentDirectory() + "\\App_data\\HTML\\JsonCreatestate\\";

        //    log.info("JSON STATE: " + path);

        //    System.IO.File.WriteAllText(path + "jsonstate.json", videogameRatings.ToString());

        //}

        [System.Web.Http.HttpPost]
        [Proofpoint]
        public string action()
        {
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            //log.info("body data: " + body.ToString());

            var jdata = JObject.Parse(body.ToString());

            var path = @Directory.GetCurrentDirectory() + "\\App_data\\HTML\\JsonCreatestate\\";
            System.IO.File.WriteAllText(path + "jsonstate.json", jdata.ToString());

            return jdata.ToString();
        }

        [System.Web.Http.HttpPost]
        [Proofpoint]
        public string Create_Json_flow()
        {
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
                body = reader.ReadToEnd();
                mem.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            //log.info("body data: " + body.ToString());

            var jdata = JObject.Parse(body.ToString());

            var path = @Directory.GetCurrentDirectory() + "\\App_data\\HTML\\JsonCreatestate\\";
            System.IO.File.WriteAllText(path + "flowstate.json", jdata.ToString());

            return jdata.ToString();
        }
    }
}
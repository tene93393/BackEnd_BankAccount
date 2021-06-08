using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using service.Extension;
using service.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace service.Controllers.Thirdparty
{
    public class ManageconfigreportController : Controller
    {
        Logmodel log = new Logmodel();

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("FirstSeen");

            ViewBag.client_token = token;

            string viewFromAnotherController = await this.RenderViewToStringAsync("~/Views/web001/Reportbuild/Index.cshtml", "xxxxxx");

            ViewBag.ssview = viewFromAnotherController;

            return View("~/Views/web001/Reportbuild/Index2.cshtml");
        }
    }
}
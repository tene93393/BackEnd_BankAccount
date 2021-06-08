using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;


namespace service.Controllers
{
   
    public class TemplateController : Controller
    {
     
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

    
        //[Proofpoint]
        public IActionResult Index()
        {   
            return View("~/Views/web001/Template/Index.cshtml");
        }
        //[Proofpoint]
        public IActionResult Dashboardone()
        {
            return View("~/Views/web001/Template/Dashboardone.cshtml");
        }
        //[Proofpoint]
        public IActionResult Dashboardtwo()
        {
            return View("~/Views/web001/Template/Dashboardtwo.cshtml");
        }
        //[Proofpoint]
        public IActionResult Dashboardthree()
        {
            return View("~/Views/web001/Template/Dashboardthree.cshtml");
        }
        //[Proofpoint]
        public IActionResult Dashboardfour()
        {
            return View("~/Views/web001/Template/Dashboardfour.cshtml");
        }


        //[Proofpoint]
        public IActionResult analytics()
        {
            return View("~/Views/web001/Template/analytics.cshtml");
        }


        //[Proofpoint]
        public IActionResult widgets()
        {
            return View("~/Views/web001/Template/widgets.cshtml");
        }


        //[Proofpoint]
        public IActionResult inbox()
        {
            return View("~/Views/web001/Template/inbox.cshtml");
        }
        //[Proofpoint]
        public IActionResult view_email()
        {
            return View("~/Views/web001/Template/view_email.cshtml");
        }
        //[Proofpoint]
        public IActionResult compose_email()
        {
            return View("~/Views/web001/Template/compose_email.cshtml");
        }
       
        //[Proofpoint]
        public IActionResult image_cropper()
        {
            return View("~/Views/web001/Template/image_cropper.cshtml");
        }

        //[Proofpoint]
        public IActionResult bar_chart()
        {
            return View("~/Views/web001/Template/bar_chart.cshtml");
        }
        //[Proofpoint]
        public IActionResult normal_table()
        {
            return View("~/Views/web001/Template/normal_table.cshtml");
        }
        //[Proofpoint]
        public IActionResult data_table()
        {
            return View("~/Views/web001/Template/data_table.cshtml");
        }
        //[Proofpoint]
        public IActionResult form_element()
        {
            return View("~/Views/web001/Template/form_element.cshtml");
        }
        //[Proofpoint]
        public IActionResult form_component()
        {
            return View("~/Views/web001/Template/form_component.cshtml");
        }

        //[Proofpoint]
        public IActionResult form_example()
        {
            return View("~/Views/web001/Template/formexample.cshtml");
        }


        //[Proofpoint]
        public IActionResult notification()
        {
            return View("~/Views/web001/Template/notification.cshtml");
        }

        ///[Proofpoint]
        public IActionResult alert()
        {
            return View("~/Views/web001/Template/alert.cshtml");
        }
        //[Proofpoint]
        public IActionResult modal()
        {
            return View("~/Views/web001/Template/modal.cshtml");
        }
        //[Proofpoint]
        public IActionResult buttons()
        {
            return View("~/Views/web001/Template/buttons.cshtml");
        }
        //[Proofpoint]
        public IActionResult tabs()
        {
            return View("~/Views/web001/Template/tabs.cshtml");
        }
        //[Proofpoint]
        public IActionResult popover()
        {
            return View("~/Views/web001/Template/popover.cshtml");
        }
        //[Proofpoint]
        public IActionResult tooltip()
        {
            return View("~/Views/web001/Template/tooltip.cshtml");
        }
        //[Proofpoint]
        public IActionResult dropdown()
        {
            return View("~/Views/web001/Template/dropdown.cshtml");
        }

        //[Proofpoint]
        public IActionResult prototype(string form)
        {
            return View("~/Views/web001/modifytemplate/"+form+".cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

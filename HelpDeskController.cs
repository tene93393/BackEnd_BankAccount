
using service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace service.Controllers
{
    public class HelpDeskController : Controller
    {
        public Logmodel log = new Logmodel();
        Encryption_model encode = new Encryption_model();


        public  async Task<IActionResult> information(string statuscode)
        {
            var page = "";
            switch (statuscode)
            {

                case "301":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "302":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "400":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "401":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "402":
                    try
                    {
                        page = statuscode;
                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "403":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "404":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "405":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "500":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "501":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "502":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "503":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "504":
                    try
                    {
                        page = statuscode;
                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;
                case "505":
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());
                    }
                    break;

                default:
                    try
                    {
                        page = statuscode;

                    }
                    catch (Exception e)
                    {
                        _ = log.swan_core_log("Information Page", "Error  : " + e.ToString());

                    }
                    break;
            }

                    return View("~/Views/web001/HelpDesk/"+page+".cshtml");

        }


      

    }
}

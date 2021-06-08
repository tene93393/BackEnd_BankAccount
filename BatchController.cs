
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Http;


namespace service.Controllers
{

    public class BatchController : Controller
    {

        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();
 
        public async Task<IActionResult> batch_add_line_job() {


                log.info("Start Batch Process");

                connect_extension.connect_db = Appconfig.client_config("ite-000120190421", "swan_db");

                string command = "EXEC batchline";

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("", "");
               

                var data = await Task.Run(() => Core_mssql.data_utility(connect_extension.connect_db, command, param));


                log.info("Add Batch Process status "+ data.ToString());

                return Content("Add compelete");
        }

      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

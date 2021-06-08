using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using service.Extension;
using service.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;

namespace service.Controllers
{
    public class ReportBuildController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("FirstSeen");

            ViewBag.client_token = token;

            string viewFromAnotherController = await this.RenderViewToStringAsync("~/Views/web001/Reportbuild/Index.cshtml","xxxxxx");
          

            ViewBag.ssview = viewFromAnotherController;

            return View("~/Views/web001/Reportbuild/Index2.cshtml");
        }

        private static async Task<string> api_value(string values) {
            Logmodel log = new Logmodel();
            var func_value = "";
            char[] chars = { '*', ' ', '\'','<','>' };
            try {
                if (String.IsNullOrEmpty(values))
                {
                    func_value = "";
                }
                else {
                    func_value = values.Trim(' ');
                }
            }
            catch (Exception e) {
                _ = log.swan_core_log("Error","api_value Error"+e.ToString());
            }
            return func_value;
        }



        [Proofpoint]
        public async Task<FileResult> report(string wealth, wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();

            // const string sessionKey = "FirstSeen";
            var value_token = HttpContext.Session.GetString("FirstSeen");

            _ = log.swan_core_log("PDF", "User Request Report : " + await Client_info.info_logon(value_token, "username"));

            var url = "";
            var tablefield = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }


            var passdata_wealth1 = new Dictionary<object, object>();
            passdata_wealth1.Add("v1",await api_value(passdata.v1));
            passdata_wealth1.Add("v2",await api_value(passdata.v2));
            passdata_wealth1.Add("v3",await api_value(passdata.v3));
            passdata_wealth1.Add("v4",await api_value(passdata.v4));
            passdata_wealth1.Add("v5",await api_value(passdata.v5));
            passdata_wealth1.Add("v6",await api_value(passdata.v6));
            passdata_wealth1.Add("v7",await api_value(passdata.v7));
            passdata_wealth1.Add("v8",await api_value(passdata.v8));
            passdata_wealth1.Add("v9",await api_value(passdata.v9));

            passdata_wealth1.Add("v10",await api_value(passdata.v10));
            passdata_wealth1.Add("v11",await api_value(passdata.v11));
            passdata_wealth1.Add("v12",await api_value(passdata.v12));
            passdata_wealth1.Add("v13",await api_value(passdata.v13));
            passdata_wealth1.Add("v14",await api_value(passdata.v14));
            passdata_wealth1.Add("v15",await api_value(passdata.v15));
            passdata_wealth1.Add("v16",await api_value(passdata.v16));
            passdata_wealth1.Add("v17",await api_value(passdata.v17));
            passdata_wealth1.Add("v18",await api_value(passdata.v18));
            passdata_wealth1.Add("v19",await api_value(passdata.v19));
            passdata_wealth1.Add("v20",await api_value(passdata.v20));


            var report_encode = await encode.encode_aes("encrypt", await Client_info.info_logon(value_token, "userid") + "|" + Date.date_now());

            var user_info = new Dictionary<object, object>();
            user_info.Add("userid", await Client_info.info_logon(value_token, "userid"));
            user_info.Add("username", await Client_info.info_logon(value_token, "username"));
            user_info.Add("email", await Client_info.info_logon(value_token, "email"));
            user_info.Add("engname", await Client_info.info_logon(value_token, "en_name"));
            user_info.Add("thainame", await Client_info.info_logon(value_token, "th_name"));
            user_info.Add("marketingid", await Client_info.info_logon(value_token, "marketingid"));
            user_info.Add("traderid", await Client_info.info_logon(value_token, "traderid"));
            user_info.Add("branch", await Client_info.info_logon(value_token, "branch"));
            user_info.Add("groupcode", await Client_info.info_logon(value_token, "groupcode"));
            user_info.Add("user_type", await Client_info.info_logon(value_token, "user_type"));
            user_info.Add("accesslevel", await Client_info.info_logon(value_token, "accesslevel"));
            user_info.Add("date_th", Date.date_now());
            user_info.Add("date_en", Date.date_en());
            user_info.Add("date_th_word", Date.date_th_string());
            user_info.Add("date_en_word", Date.date_en_string());
            user_info.Add("report_name", report_encode);
            user_info.Add("release", "SWAN : "+Appconfig.appfcg("release"));

            Tablereportcs report = new Tablereportcs();

            string playload_report = "";
            if (wealth == "wealth8")
            {
                playload_report = "wealth8.cshtml";
            }
            else if (wealth == "wealth9")
            {
                playload_report = "wealth9.cshtml";
            }
            else if (wealth == "wealth10")
            {
                playload_report = "wealth10.cshtml";
            }
            else {

                playload_report = "Default.cshtml";
            }
           
            _ = log.swan_core_log("PDF", "Data Wealth : " + JsonConvert.SerializeObject(await wealth_internal(passdata_wealth1)));

            string viewFromAnotherController = await this.Wealth_Report_Async("~/Views/web001/Reportbuild/"+ playload_report, await wealth_internal(passdata_wealth1), passdata_wealth1, user_info,url);
            string html = viewFromAnotherController;

            string FilePath = "";

            try
            {
                _ = log.swan_core_log("PDF", "File Url : " + url);
                FilePath = await Pdf_model.Create_pdf_wizard_custom(html, url, passdata.refcode);
                _ = log.swan_core_log("PDF", "File form Create " + FilePath);
            }
            catch (Exception e) {
                _ = log.swan_core_log("PDF", "Error Create PDF " + e.ToString());
            }
        
            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");


        }



        // [Proofpoint]
        public async Task<FileResult> report_babayaka(string wealth, wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();

            // const string sessionKey = "FirstSeen";
            var value = HttpContext.Session.GetString("FirstSeen");
            var url = "";
            var tablefield = "";

            if (HttpContext.Request.IsHttps.ToString() == "True")
            {
                url = "https://" + HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + HttpContext.Request.Host;
            }


            var passdata_wealth1 = new Dictionary<object, object>();
            passdata_wealth1.Add("v1", await api_value(passdata.v1));
            passdata_wealth1.Add("v2", await api_value(passdata.v2));
            passdata_wealth1.Add("v3", await api_value(passdata.v3));
            passdata_wealth1.Add("v4", await api_value(passdata.v4));
            passdata_wealth1.Add("v5", await api_value(passdata.v5));
            passdata_wealth1.Add("v6", await api_value(passdata.v6));
            passdata_wealth1.Add("v7", await api_value(passdata.v7));
            passdata_wealth1.Add("v8", await api_value(passdata.v8));
            passdata_wealth1.Add("v9", await api_value(passdata.v9));

            passdata_wealth1.Add("v10", await api_value(passdata.v10));
            passdata_wealth1.Add("v11", await api_value(passdata.v11));
            passdata_wealth1.Add("v12", await api_value(passdata.v12));
            passdata_wealth1.Add("v13", await api_value(passdata.v13));
            passdata_wealth1.Add("v14", await api_value(passdata.v14));
            passdata_wealth1.Add("v15", await api_value(passdata.v15));
            passdata_wealth1.Add("v16", await api_value(passdata.v16));
            passdata_wealth1.Add("v17", await api_value(passdata.v17));
            passdata_wealth1.Add("v18", await api_value(passdata.v18));
            passdata_wealth1.Add("v19", await api_value(passdata.v19));
            passdata_wealth1.Add("v20", await api_value(passdata.v20));


            var user_info = new Dictionary<object, object>();
            user_info.Add("v1", "");


            Tablereportcs report = new Tablereportcs();

            string playload_report = "";

            if (wealth == "wealth1")
            {
                playload_report = "wealth1.cshtml";
                var getDatafield = await wealth_internal(passdata_wealth1);
                tablefield = await report.Razerreport_format1(getDatafield, passdata.v9, passdata.v2, passdata.v3);

            }
            else if (wealth == "wealth2")
            {
                playload_report = "wealth2.cshtml";
                var getDatafield = await wealth_internal(passdata_wealth1);
                tablefield = await report.Razerreport_format2(getDatafield, passdata.v9, passdata.v2, passdata.v3);
            }
            else if (wealth == "wealth3")
            {
                playload_report = "wealth3.cshtml";
                var getDatafield = await wealth_internal(passdata_wealth1);
                tablefield = await report.Razerreport_format3(getDatafield, passdata.v9, passdata.v2, passdata.v3);

            }
            else if (wealth == "wealth4")
            {
                playload_report = "wealth4.cshtml";
            }
            else if (wealth == "wealth5")
            {
                playload_report = "wealth5.cshtml";
            }
            else if (wealth == "wealth6")
            {
                playload_report = "wealth6.cshtml";
                tablefield = JsonConvert.SerializeObject(passdata_wealth1);
            }
            else if (wealth == "wealth7")
            {
                playload_report = "wealth7.cshtml";
            }
            else if (wealth == "wealth8")
            {
                playload_report = "wealth8.cshtml";
            }
            else if (wealth == "wealth9")
            {
                playload_report = "wealth9.cshtml";
            }
            else if (wealth == "wealth10")
            {

                //Ploy Build

                playload_report = "wealth10.cshtml";
            }
            else
            {

                playload_report = "Default.cshtml";
            }
            //send_report.Add("playload_form", playload_report);


            _ = log.swan_core_log("PDF", "Data Wealth : " + JsonConvert.SerializeObject(await wealth_internal(passdata_wealth1)));

            string viewFromAnotherController = await this.Wealth_Report_Async("~/Views/web001/Reportbuild/" + playload_report, await wealth_internal(passdata_wealth1), passdata_wealth1, user_info,url);
            string html = viewFromAnotherController;

            string FilePath = "";

            try
            {
                _ = log.swan_core_log("PDF", "File Url : " + url);
                FilePath = await Pdf_model.Create_pdf_wizard_custom(html, url, passdata.refcode);
                _ = log.swan_core_log("PDF", "File form Create " + FilePath);
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("PDF", "Error Create PDF " + e.ToString());
            }


            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");


        }




        public async Task<FileResult> Ploter1(string wealth)
        {
            // const string sessionKey = "FirstSeen";
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

            var send_report = new Dictionary<object, object>();
            send_report.Add("userid", value);
            send_report.Add("dateprint", Date.date_en_string());
            send_report.Add("reportid", wealth);

            string playload_report = "";

            if (wealth == "wealth1")
            {
                playload_report = "wealth1.cshtml";
            }
            else if (wealth == "wealth2")
            {
                playload_report = "wealth2.cshtml";
            }
            else if (wealth == "wealth3")
            {
                playload_report = "wealth3.cshtml";
            }
            else if (wealth == "wealth4")
            {
                playload_report = "wealth4.cshtml";
            }
            else if (wealth == "wealth5")
            {
                playload_report = "wealth5.cshtml";
            }
            else if (wealth == "wealth6")
            {
                playload_report = "wealth6.cshtml";
            }
            else if (wealth == "wealth7")
            {
                playload_report = "wealth7.cshtml";
            }
            else if (wealth == "wealth8")
            {
                playload_report = "wealth8.cshtml";
            }
            else if (wealth == "wealth9")
            {
                playload_report = "wealth9.cshtml";
            }
            else if (wealth == "wealth10")
            {
                playload_report = "wealth10.cshtml";
            }
            else
            {

                playload_report = "Default.cshtml";
            }

            send_report.Add("playload_form", playload_report);



            string viewFromAnotherController = await this.RenderViewToStringAsync("~/Views/web001/Reportbuild/" + playload_report, JsonConvert.SerializeObject(send_report));
            string html = viewFromAnotherController;

            string FilePath = "";

            FilePath = await Pdf_model.Create_pdf_wizard_custom(html, url,"RP031");


            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");
        }

        // For Pass parameter

        public async Task<FileResult> Ploter2(string wealth)
        {            

            // const string sessionKey = "FirstSeen";
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

            var send_report = new Dictionary<object, object>();
            send_report.Add("userid", value);
            send_report.Add("dateprint", Date.date_en_string());
            send_report.Add("reportid", wealth);            

            string playload_report = "";

            if (wealth == "wealth1")
            {
                playload_report = "wealth1.cshtml";
            }
            else if (wealth == "wealth2")
            {
                playload_report = "wealth2.cshtml";
            }
            else if (wealth == "wealth3")
            {
                playload_report = "wealth3.cshtml";
            }
            else if (wealth == "wealth4")
            {
                playload_report = "wealth4.cshtml";
            }
            else if (wealth == "wealth5")
            {
                playload_report = "wealth5.cshtml";
            }
            else if (wealth == "wealth6")
            {
                playload_report = "wealth6.cshtml";
            }
            else if (wealth == "wealth7")
            {
                playload_report = "wealth7.cshtml";
            }
            else if (wealth == "wealth8")
            {
                playload_report = "wealth8.cshtml";
            }
            else if (wealth == "wealth9")
            {
                playload_report = "wealth9.cshtml";
            }
            else if (wealth == "wealth10")
            {
                playload_report = "wealth10.cshtml";
            }
            else
            {

                playload_report = "Default.cshtml";
            }

            send_report.Add("playload_form", playload_report);
                       
            string viewFromAnotherController = await this.RenderViewToStringAsync("~/Views/web001/Reportbuild/" + playload_report, JsonConvert.SerializeObject(send_report));
            string html = viewFromAnotherController;

            string FilePath = "";

            FilePath = await Pdf_model.Create_pdf_wizard(html, url);


            byte[] pdfByte = Pdf_model.GetBytesFromFile(FilePath);
            System.IO.File.Delete(FilePath);
            return File(pdfByte, "application/pdf");
        }

        public async Task<List<object>> wealth_internal(Dictionary<object, object> data)
        {
            Logmodel log = new Logmodel();
            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            var temp = "";

            string Command = "EXEC  Wealth_project @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("1", data["v1"].ToString());
            param.Add("2", data["v2"].ToString());
            param.Add("3", data["v3"].ToString());
            param.Add("4", data["v4"].ToString());
            param.Add("5", data["v5"].ToString());
            param.Add("6", data["v6"].ToString());
            param.Add("7", data["v7"].ToString());
            param.Add("8", data["v8"].ToString());
            param.Add("9", data["v9"].ToString());
            param.Add("10", data["v10"].ToString());
            param.Add("11", data["v11"].ToString());
            param.Add("12", data["v12"].ToString());
            param.Add("13", data["v13"].ToString());
            param.Add("14", data["v14"].ToString());
            param.Add("15", data["v15"].ToString());
            param.Add("16", data["v16"].ToString());
            param.Add("17", data["v17"].ToString());
            param.Add("18", data["v18"].ToString());
            param.Add("19", data["v19"].ToString());
            param.Add("20", data["v20"].ToString());



            var data_1 = Core_mssql.data_with_col(project_request, Command, param);

            log.info("log Data wealth jArray : " + JsonConvert.SerializeObject(data_1));


            return data_1;

        }


        public async Task<List<object>> wealth_1(wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            var temp = "";
         
            string Command = "EXEC  Wealth_project_report @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("1", passdata.v1.ToString());
            param.Add("2", passdata.v2.ToString());
            param.Add("3", passdata.v3.ToString());
            param.Add("4", passdata.v4.ToString());
            param.Add("5", passdata.v5.ToString());
            param.Add("6", passdata.v6.ToString());
            param.Add("7", passdata.v7.ToString());
            param.Add("8", passdata.v8.ToString());
            param.Add("9", passdata.v9.ToString());
            param.Add("10", passdata.v10.ToString());
            param.Add("11", passdata.v11.ToString());
            param.Add("12", passdata.v12.ToString());
            param.Add("13", passdata.v13.ToString());
            param.Add("14", passdata.v14.ToString());
            param.Add("15", passdata.v15.ToString());
            param.Add("16", passdata.v16.ToString());
            param.Add("17", passdata.v17.ToString());
            param.Add("18", passdata.v18.ToString());
            param.Add("19", passdata.v19.ToString());
            param.Add("20", passdata.v20.ToString());

            var data_1 =  Core_mssql.data_with_col(project_request, Command, param);

            log.info("log Data wealth jArray : " + JsonConvert.SerializeObject(data_1));


            return data_1;

        }


        public async Task<ActionResult> wealth_debug_data(wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            var temp = "";
          
            try
            {
                string Command = "EXEC  Wealth_project @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("1", passdata.v1.ToString());
                param.Add("2", passdata.v2.ToString());
                param.Add("3", passdata.v3.ToString());
                param.Add("4", passdata.v4.ToString());
                param.Add("5", passdata.v5.ToString());
                param.Add("6", passdata.v6.ToString());
                param.Add("7", passdata.v7.ToString());
                param.Add("8", passdata.v8.ToString());
                param.Add("9", passdata.v9.ToString());
                param.Add("10", passdata.v10.ToString());
                param.Add("11", passdata.v11.ToString());
                param.Add("12", passdata.v12.ToString());
                param.Add("13", passdata.v13.ToString());
                param.Add("14", passdata.v14.ToString());
                param.Add("15", passdata.v15.ToString());
                param.Add("16", passdata.v16.ToString());
                param.Add("17", passdata.v17.ToString());
                param.Add("18", passdata.v18.ToString());
                param.Add("19", passdata.v19.ToString());
                param.Add("20", passdata.v20.ToString());

                var data_1 = Core_mssql.data_with_col(project_request, Command, param);

                log.info(JsonConvert.SerializeObject(data_1));

                return Content(JsonConvert.SerializeObject(data_1));

            }
            catch (Exception e) {

                log.info("Error "+e.ToString());

                return Content("Error "+e.ToString());
            }
        }

        public async Task<ActionResult> wealth_debug_data2(wealth_passthru passdata)
        {
            Logmodel log = new Logmodel();
            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            var temp = "";

            try
            {

                string Command = "EXEC  Wealth_project_report @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("1", passdata.v1.ToString());
                param.Add("2", passdata.v2.ToString());
                param.Add("3", passdata.v3.ToString());
                param.Add("4", passdata.v4.ToString());
                param.Add("5", passdata.v5.ToString());
                param.Add("6", passdata.v6.ToString());
                param.Add("7", passdata.v7.ToString());
                param.Add("8", passdata.v8.ToString());
                param.Add("9", passdata.v9.ToString());
                param.Add("10", passdata.v10.ToString());
                param.Add("11", passdata.v11.ToString());
                param.Add("12", passdata.v12.ToString());
                param.Add("13", passdata.v13.ToString());
                param.Add("14", passdata.v14.ToString());
                param.Add("15", passdata.v15.ToString());
                param.Add("16", passdata.v16.ToString());
                param.Add("17", passdata.v17.ToString());
                param.Add("18", passdata.v18.ToString());
                param.Add("19", passdata.v19.ToString());
                param.Add("20", passdata.v20.ToString());

                var data_1 = Core_mssql.data_with_col(project_request, Command, param);

                log.info(JsonConvert.SerializeObject(data_1));

                return Content(JsonConvert.SerializeObject(data_1));

            }
            catch (Exception e)
            {

                log.info("Error " + e.ToString());

                return Content("Error " + e.ToString());
            }
        }


    }
}

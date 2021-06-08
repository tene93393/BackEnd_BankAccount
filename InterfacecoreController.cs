using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using service.Models;

namespace service.Controllers.Interfacecore
{
    public class InterfacecoreController : Controller
    {
        object result_function = "";


        public IActionResult Index()
        {
            return View("~/Views/web001/Interfacecore/index.cshtml");
        }
        //[Responsese(Location = ResponseCacheLocation.None, NoStore = true)]
        //[Proofpoint]
        public async Task<ActionResult> etl_swan_data(string v1, string v2,string token)
        {
            Logmodel log = new Logmodel();
            log.swan_core_log("Debug_etl_swan", "data : " + v2.ToString());
            try
            {
                var request_data = new Dictionary<string, string>();
                request_data.Add("v1", v1);
                request_data.Add("v2", v2);

                Mysqlswan swandb = new Mysqlswan();

                string Command = "swan_data";
                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("@v1", v1);
                param.Add("@v2", v2);

                try
                {
                    result_function = await swandb.data_with_col_procedures(Command, param);
                 
                }
                catch(Exception e){

                    _ = log.interfacedata("Error data  : " + e.ToString());
                    result_function = "nodata";
                }


                return Content(JsonConvert.SerializeObject(result_function));
            }
            catch(Exception e)
            {
                _ = log.interfacedata("Exception: " + e.ToString());
                return Content("No Data");
            }
            

        }


        //[Responsese(Location = ResponseCacheLocation.None, NoStore = true)]
        //[Proofpoint]
        public async Task<ActionResult> etl_swan_report(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlswan swandb = new Mysqlswan();

            string Command = "swan_report";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            { 
                result_function = await swandb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }
            return Content(JsonConvert.SerializeObject(result_function));

        }


        //[Proofpoint]
        public async Task<ActionResult> etl_hawknet_data(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            try
            {
                _ = log.swan_core_log("Debug_etl_hawk", "data : " + v2.ToString());





                //var request_data = new Dictionary<string, string>();
                //request_data.Add("v1", v1);
                //request_data.Add("v2", v2);
                Mysqlhawk hawkdb = new Mysqlhawk();

                if (v1 == "list_customer_update")
                {
                    string Command = "CALL Hawknet_data (@v1,@v2)";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("v1", v1);
                    param.Add("v2", v2);
                    result_function = await hawkdb.data_with_col_api(Command, param);
                }
                else {

                    string Command = "Hawknet_data";
                    Dictionary<object, object> param = new Dictionary<object, object>();
                    param.Add("@v1", v1);
                    param.Add("@v2", v2);
                    result_function = await hawkdb.data_with_col_procedures(Command, param);


                }


              

                _ = log.swan_core_log("Debug_etl_hawk", "return data : " + JsonConvert.SerializeObject(result_function));


            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }

        public async Task<ActionResult> etl_hawknet_data_maintenance(Customer_maintenance passdata)
        {
            Appconfig cfg = new Appconfig();
            Logmodel log = new Logmodel();


            var hawknet_data_maintenance = new Dictionary<object, object>();

            try
            {
                _ = log.swan_core_log("Debug_etl_hawk", "data : " + passdata.v1.ToString());

                var request_data = new Dictionary<string, string>();
                request_data.Add("v1", passdata.v1);
                request_data.Add("v2", passdata.v2);
                request_data.Add("v3", passdata.v3);
                request_data.Add("v4", passdata.v4);
                request_data.Add("v5", passdata.v5);
                request_data.Add("v6", passdata.v6);
                request_data.Add("v7", passdata.v7);
                request_data.Add("v8", passdata.v8);
                request_data.Add("v9", passdata.v9);
                request_data.Add("v10", passdata.v10);
                request_data.Add("v11", passdata.v11);
                request_data.Add("v12", passdata.v12);
                request_data.Add("v13", passdata.v13);
                request_data.Add("v14", passdata.v14);
                request_data.Add("v15", passdata.v15);
                request_data.Add("v16", passdata.v16);
                request_data.Add("v17", passdata.v17);
                request_data.Add("v18", passdata.v18);
                request_data.Add("v19", passdata.v19);
                request_data.Add("v20", passdata.v20);

                request_data.Add("v21", passdata.v21);
                request_data.Add("v22", passdata.v22);
                request_data.Add("v23", passdata.v23);
                request_data.Add("v24", passdata.v24);
                request_data.Add("v25", passdata.v25);
                request_data.Add("v26", passdata.v26);
                request_data.Add("v27", passdata.v27);
                request_data.Add("v28", passdata.v28);
                request_data.Add("v29", passdata.v29);
                request_data.Add("v30", passdata.v30);


                request_data.Add("v31", passdata.v31);
                request_data.Add("v32", passdata.v32);
                request_data.Add("v33", passdata.v33);
                request_data.Add("v34", passdata.v34);
                request_data.Add("v35", passdata.v35);
                request_data.Add("v36", passdata.v36);
                request_data.Add("v37", passdata.v37);
                request_data.Add("v38", passdata.v38);
                request_data.Add("v39", passdata.v39);
                request_data.Add("v40", passdata.v40);

                hawknet_data_maintenance.Add("result", await  HawknetData.Hawkdata_full_mysql(JsonConvert.SerializeObject(request_data)));


                _ = log.swan_core_log("Debug_etl_hawk", "result  : " + JsonConvert.SerializeObject(hawknet_data_maintenance));

            }
            catch (Exception e)
            {
                _ = log.swan_core_log("Debug_etl_hawk", "Error : " + e.ToString());

                hawknet_data_maintenance.Add("result", "nodata");
            }

            return Content(JsonConvert.SerializeObject(hawknet_data_maintenance));

        }


        //[Proofpoint]
        public async Task<ActionResult> etl_hawknet_data_import(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlhawk hawkdb = new Mysqlhawk();

            string Command = "Hawknet_data_import";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await hawkdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }
        
        //[Proofpoint]
        public async Task<ActionResult> etl_hawknet_report(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);
           
            // await
            Mysqlhawk hawkdb = new Mysqlhawk();

            string Command = "Hawknet_report";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await hawkdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }
       
        //[Proofpoint]
        public async Task<ActionResult> etl_tiger_data(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);

            Mysqltiger tigerdb = new Mysqltiger();

            string Command = "tiger_data";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await tigerdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }
       // [Proofpoint]
        public async Task<ActionResult> etl_tiger_report(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);

            Mysqltiger tigerdb = new Mysqltiger();

            string Command = "tiger_data";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await tigerdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }
       // [Proofpoint]
        public async Task<ActionResult> etl_wealth_data(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();
            Mysqlwealth wealthdb = new Mysqlwealth();




            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");

            string Command = "EXEC wealth_data @1,@2";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("1", v1);
            param.Add("2", v2);

            try
            {
                result_function = Core_mssql.data_with_col(project_request, Command, param);
                //result_function = await wealthdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }

        private static string projectsetting()
        {
            string path = Directory.GetCurrentDirectory();
            string configtext = System.IO.File.ReadAllText(path + "/Data/Appsetting.json");
            JObject config_parse = JObject.Parse(configtext);
            return config_parse["projectid"].ToString();
        }

        private static string dbselect(string var_name)
        {
            string path = Directory.GetCurrentDirectory();
            string configtext = System.IO.File.ReadAllText(path + "/Clients/Clients.json");
            JObject config_parse = JObject.Parse(configtext);
            return config_parse[projectsetting()][var_name].ToString();
        }



        //[Proofpoint]
        public async Task<ActionResult> etl_wealth_data_import(string v1, string v2, string token)
        {
            // for mssql  Only  EXEC Wealth_data_import 'sdddd' , '';
            Logmodel log = new Logmodel();


            //string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            //string Command = "EXEC  Wealth_data_import @1,@2";
            //Dictionary<string, string> param = new Dictionary<string, string>();
            //param.Add("1", v1);
            //param.Add("2", v2);

            //try
            //{
            //    result_function = Core_mssql.data_with_col(project_request, Command, param);
            //    //result_function = await wealthdb.data_with_col_procedures(Command, param);
            //}
            //catch (Exception e)
            //{
            //    _ = log.interfacedata("Error data  : " + e.ToString());
            //    result_function = "nodata";
            //}




            var db_select = dbselect("database");
            if (db_select == "mssql")
            {
                string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
                string Command = "EXEC  Wealth_data_import @1,@2";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("1", v1);
                param.Add("2", v2);

                try
                {
                    result_function = Core_mssql.data_with_col(project_request, Command, param);
                    //result_function = await wealthdb.data_with_col_procedures(Command, param);
                }
                catch (Exception e)
                {
                    _ = log.swan_core_log("wealth_import","Error data  : " + e.ToString());
                    result_function = "nodata";
                }
            }
            else if (db_select == "mysql")
            {
                Mysqlwealth wealth = new Mysqlwealth();
                string Command = "CALL Wealth_data_import (@1,@2)";
                Dictionary<object, object> param = new Dictionary<object, object>();
                param.Add("1", v1);
                param.Add("2", v2);

                _ = log.swan_core_log("wealth_import", "variable1  : " + v1);
                _ = log.swan_core_log("wealth_import", "variable2  : " + v2);

                result_function = await wealth.data_with_col_api(Command, param);


                _ = log.swan_core_log("wealth_import", "date _return  : " + JsonConvert.SerializeObject(result_function));
            }





            return Content(JsonConvert.SerializeObject(result_function));

        }



        [Proofpoint]
        public async Task<ActionResult> etl_wealth_report(string v1, string v2, string token)
        {
            Logmodel log = new Logmodel();


            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");

            string Command = "EXEC wealth_data_report @1,@2";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = Core_mssql.data_with_col(project_request, Command, param);
                //result_function = await wealthdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Content(JsonConvert.SerializeObject(result_function));

        }

        [Proofpoint]
        public async Task<ActionResult> etl_seal_data(string v1, string v2)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlseal sealdb = new Mysqlseal();

            string Command = "Seal_data";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await sealdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _= log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Json(result_function);

        }


        [Proofpoint]
        public async Task<ActionResult> etl_seal_report(string v1, string v2)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlseal sealdb = new Mysqlseal();

            string Command = "Seal_report";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await sealdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Json(result_function);

        }


        [Proofpoint]
        public async Task<ActionResult> etl_bos_data(string v1, string v2)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlbos bosdb = new Mysqlbos();

            string Command = "Bos_data";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await bosdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Json(result_function);

        }


        [Proofpoint]
        public async Task<ActionResult> etl_bos_report(string v1, string v2)
        {
            Logmodel log = new Logmodel();

            var request_data = new Dictionary<string, string>();
            request_data.Add("v1", v1);
            request_data.Add("v2", v2);


            Mysqlbos bosdb = new Mysqlbos();

            string Command = "Bos_report";
            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("@v1", v1);
            param.Add("@v2", v2);

            try
            {
                result_function = await bosdb.data_with_col_procedures(Command, param);
            }
            catch (Exception e)
            {
                _ = log.interfacedata("Error data  : " + e.ToString());
                result_function = "nodata";
            }

            return Json(result_function);

        }
    }
}
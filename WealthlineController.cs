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

namespace service.Controllers.Wealthline
{
    public class WealthlineController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/web001/Wealthline/Index.cshtml");
        }

        public IActionResult Portfolio()
        {
            return View("~/Views/web001/Wealthline/Portfolio.cshtml");
        }

        //[Responsese(Location = ResponseCacheLocation.None, NoStore = true)]report_passthru
        //public ActionResult wealth_1(string v1, string v2, string v3, string v4, string v5, string v6, string v7, string v8)
        
        public async Task <ActionResult> wealth_1(report_passthru passdata)
        {
            Appconfig cfg = new Appconfig();
            Logmodel log = new Logmodel();

            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            connect_extension.connect_db = project_request;

            _= log.info("V2 Pass True _data : " + passdata.v2);



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

            var url_api = cfg.initial_config("Domain");

            var alldata = new Dictionary<object, object>();
            alldata.Add("method", "wealth001");
            alldata.Add("webapi_access_token", "y9hUKvmeOpLNmNG9kMq/Hf3XJvJLqVYBC28q9fUwRVk=");
            alldata.Add("apiname", "wealth001");
            alldata.Add("version", "1");
            alldata.Add("data", request_data);

            log.info("Wealth_1 Domain :" + url_api);


            log.info("Wealth_1 :" + JsonConvert.SerializeObject(alldata));








            //var client = new RestClient(url_api+"/api/Wealth");
            //var request = new RestRequest(Method.POST);           
            //request.AddHeader("Content-Type", "application/json");

            //request.AddParameter("application/json", JsonConvert.SerializeObject(alldata), ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            //log.info("Wealth_1 response:" + response.Content.ToString());

            //HttpStatusCode statusCode_wealth = response.StatusCode;
            //int resulthttpstatus = (int)statusCode_wealth;


            int resulthttpstatus = 200;
            if (resulthttpstatus == 200)
            {
                //var temp_data = JObject.Parse(response.Content.ToString());
                //var data = JObject.Parse(temp_data["response"].ToString());
                var return_wealth = new Dictionary<object, object>();


                if (passdata.v1.ToString() == "PL_CASHEQ")
                {

                    var request_data_spcial = new Dictionary<string, string>();

                    request_data_spcial.Add("v2", passdata.v2);
                    request_data_spcial.Add("v3", passdata.v3);
                    request_data_spcial.Add("v4", passdata.v4);
                    request_data_spcial.Add("v5", passdata.v5);
                    request_data_spcial.Add("v6", passdata.v6);
                    request_data_spcial.Add("v7", passdata.v7);
                    request_data_spcial.Add("v8", passdata.v8);
                    request_data_spcial.Add("v9", passdata.v9);
                    request_data_spcial.Add("v10", passdata.v10);
                    request_data_spcial.Add("v11", passdata.v11);
                    request_data_spcial.Add("v12", passdata.v12);
                    request_data_spcial.Add("v13", passdata.v13);
                    request_data_spcial.Add("v14", passdata.v14);
                    request_data_spcial.Add("v15", passdata.v15);
                    request_data_spcial.Add("v16", passdata.v16);
                    request_data_spcial.Add("v17", passdata.v17);
                    request_data_spcial.Add("v18", passdata.v18);
                    request_data_spcial.Add("v19", passdata.v19);
                    request_data_spcial.Add("v20", passdata.v20);


                 

                    try
                    {
                        var temp_data = new List<object>();

                        var temp_data_border = new List<object>();

                        var data_mode1 = await tbl_multi_condition("1", JsonConvert.SerializeObject(request_data_spcial));

                        _ = log.swan_core_log("debug_tran", ": data_mode1" + JsonConvert.SerializeObject(data_mode1));

                        if (data_mode1.Count.ToString() == "0" | data_mode1.Count.ToString() == "")
                        {
                            _ = log.swan_core_log("debug_tran", ": No data mode1");
                        }
                        else
                        {
                            _ = log.swan_core_log("wealth", "========================================");

                            string[] G1arr = { "1", "2", "3", "4", "5", "6","7" };
                            for (int i = 0; i < G1arr.Length; i++)
                            {
                                _ = log.swan_core_log("wealth", "request data modedata : " + i);

                                var modedata = await tbl_multi_condition(G1arr[i].ToString(), JsonConvert.SerializeObject(request_data_spcial));

                                _ = log.swan_core_log("wealth", "request data modedata : " + G1arr[i].ToString() + "data : "+JsonConvert.SerializeObject(modedata));
                                _ = log.swan_core_log("wealth", "request data modedata_g2 : " + G1arr[i].ToString() + "Count : " + modedata.Count.ToString());
                                if (modedata.Count.ToString() == "0" | modedata.Count.ToString() == "")
                                {
                                    // temp_data.Add("Empty");
                                    _ = log.swan_core_log("debug_tran", ": No data mode1");
                                }
                                else
                                {
                                   
                                    for (int xi = 0; xi < modedata.Count; xi++)
                                    {
                                        temp_data.Add(modedata[xi]);
                                    }
                                }
                            }
                            _ = log.swan_core_log("wealth", "========================================");
                            //
                        }

                        _ = log.swan_core_log("wealth", "modedata : " + JsonConvert.SerializeObject(temp_data));

                        var data_mode2 = await tbl_multi_condition("8", JsonConvert.SerializeObject(request_data_spcial));

                        _ = log.swan_core_log("debug_tran", ": data_mode2" + JsonConvert.SerializeObject(data_mode2));

                        if (data_mode2.Count.ToString() == "0" | data_mode2.Count.ToString() == "")
                        {
                            _ = log.swan_core_log("debug_tran", ": No data mode2");
                        }
                        else
                        {
                            _ = log.swan_core_log("wealth", "========================================");
                            string[] G2arr = {"8", "9", "10", "11", "12","13" };
                            for (int i = 0; i < G2arr.Length; i++)
                            {
                                _ = log.swan_core_log("wealth", "request data modedata_g2 : " + i);

                                var modedata_g2 = await tbl_multi_condition(G2arr[i].ToString(), JsonConvert.SerializeObject(request_data_spcial));

                                _ = log.swan_core_log("wealth", "request data modedata_g2 : " + G2arr[i].ToString() + "data : " + JsonConvert.SerializeObject(modedata_g2));


                                _ = log.swan_core_log("wealth", "request data modedata_g2 : " + G2arr[i].ToString() + "Count : " + modedata_g2.Count.ToString());


                                if (modedata_g2.Count.ToString() == "0" | modedata_g2.Count.ToString() == "")
                                {
                                    // temp_data.Add("Empty");
                                    _ = log.swan_core_log("debug_tran", ": No data mode2");
                                }
                                else
                                {
                                    _ = log.swan_core_log("wealth", "modedata_g2 : " + JsonConvert.SerializeObject(modedata_g2));
                                    for (int xb = 0; xb < modedata_g2.Count; xb++)
                                    {
                                        temp_data.Add(modedata_g2[xb]);
                                    }
                                }
                            }

                            _ = log.swan_core_log("wealth", "========================================");
                            //
                        }


                       

                        _ = log.swan_core_log("wealth", "data w_profit_loss retrun data : " + JsonConvert.SerializeObject(temp_data));

                        return_wealth.Add("result", temp_data);
                    }
                    catch (Exception e) {
                        _ = log.swan_core_log("wealth", "data w_profit_loss error : " + e.ToString());
                    }



                }
                else {

                    // trail  for fss 20200427
                    var db_select = dbselect("database");
                    if (db_select == "mssql")
                    {
                        return_wealth.Add("result", Wealthapi.wealth_action(JsonConvert.SerializeObject(request_data)));
                    }
                    else if (db_select == "mysql")
                    {
                        return_wealth.Add("result", await Wealthapi.wealth_action_mysql(JsonConvert.SerializeObject(request_data)));
                    }


                }




               


                //return_wealth.Add("result", await Wealthapi.wealth_action(JsonConvert.SerializeObject(request_data)));

                return Content(JsonConvert.SerializeObject(return_wealth));
            }
            else
            {
                return Content("Api fail");

            }

        }


        private async Task<List<object>> tbl_multi_condition(string mode,string paramdata)
        {
            Logmodel log = new Logmodel();

            Mysqlwealth wealth = new Mysqlwealth();

            var wealth_data = JObject.Parse(paramdata);

            var temp_data = new List<object>();

            try
            {

                string Command = "CALL w_profit_loss(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20)";

            Dictionary<object, object> param = new Dictionary<object, object>();
            param.Add("1", mode);
            param.Add("2", wealth_data["v2"].ToString());
            param.Add("3", wealth_data["v3"].ToString());
            param.Add("4", wealth_data["v4"].ToString());
            param.Add("5", wealth_data["v5"].ToString());
            param.Add("6", wealth_data["v6"].ToString());
            param.Add("7", wealth_data["v7"].ToString());
            param.Add("8", wealth_data["v8"].ToString());
            param.Add("9", wealth_data["v9"].ToString());
            param.Add("10", wealth_data["v10"].ToString());

            param.Add("11", wealth_data["v11"].ToString());
            param.Add("12", wealth_data["v12"].ToString());
            param.Add("13", wealth_data["v13"].ToString());
            param.Add("14", wealth_data["v14"].ToString());
            param.Add("15", wealth_data["v15"].ToString());
            param.Add("16", wealth_data["v16"].ToString());
            param.Add("17", wealth_data["v17"].ToString());
            param.Add("18", wealth_data["v18"].ToString());
            param.Add("19", wealth_data["v19"].ToString());
            param.Add("20", wealth_data["v20"].ToString());

            _ = log.swan_core_log("debug_tran", "data variable : " + JsonConvert.SerializeObject(param));

            var data_1 = await wealth.data_with_col_api(Command, param);

            _ = log.swan_core_log("debug_tran", "data w_profit_loss : "+JsonConvert.SerializeObject(data_1));
                return data_1;

            }
            catch (Exception e)
            {
                _ = log.swan_core_log("debug_tran", ": Execption E " + e.ToString());

                return temp_data;


            }
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


        public async Task<ActionResult> wealth_2(report_passthru_full passdata)
        {
            Appconfig cfg = new Appconfig();
            Logmodel log = new Logmodel();

            string project_request = Appconfig.client_config("ite-000120190421", "wealth_project");
            connect_extension.connect_db = project_request;

            _ = log.info("V2 Pass True _data : " + passdata.v2);

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

            var url_api = cfg.initial_config("Domain");

            var alldata = new Dictionary<object, object>();
            alldata.Add("method", "wealth001");
            alldata.Add("webapi_access_token", "y9hUKvmeOpLNmNG9kMq/Hf3XJvJLqVYBC28q9fUwRVk=");
            alldata.Add("apiname", "wealth001");
            alldata.Add("version", "1");
            alldata.Add("data", request_data);

         

            int resulthttpstatus = 200;
            if (resulthttpstatus == 200)
            {
                //var temp_data = JObject.Parse(response.Content.ToString());
                //var data = JObject.Parse(temp_data["response"].ToString());
                var return_wealth = new Dictionary<object, object>();

                //// trail  for fss 20200427
               
                var db_select = dbselect("database");
                if (db_select == "mssql")
                {
                    return_wealth.Add("result", Wealthapi.wealth_full(JsonConvert.SerializeObject(request_data)));
                }
                else if (db_select == "mysql")
                {
                    return_wealth.Add("result", await Wealthapi.wealth_full_mysql(JsonConvert.SerializeObject(request_data)));
                }

                //return_wealth.Add("result", await Wealthapi.wealth_full(JsonConvert.SerializeObject(request_data)));

                return Content(JsonConvert.SerializeObject(return_wealth));
            }
            else
            {
                return Content("Api fail");

            }

        }


    }
}
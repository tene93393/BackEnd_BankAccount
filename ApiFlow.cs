using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using service.Models;


namespace service
{



    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ApiFlowController : ControllerBase
    {

        public Logmodel log = new Logmodel();
        private Dictionary<object, object> Apiflow_default_condition;
        private Dictionary<object, object> raw_data;
      

        [HttpGet]
        public async Task<IActionResult> swandata(string v1,string v2,string v3,string v4,string v5,string v6,string v7 ,string v8, string v9,string v10)
        {
            var temp_status = new Dictionary<object, object>();
            var fail_status = new Dictionary<object, object>();

            if (String.IsNullOrEmpty(v1))
            {
                fail_status.Add("status", "request no valid"); 
                return StatusCode(400, fail_status);
            }
            else if (String.IsNullOrEmpty(v2))
            {
                fail_status.Add("status", "request no valid");
                return StatusCode(400, fail_status);

            }
            else if (String.IsNullOrEmpty(v4))
            {
                fail_status.Add("status", "request no valid");
                return StatusCode(400, fail_status);

            }
            else if (String.IsNullOrEmpty(v5))
            {
                fail_status.Add("status", "request no valid");
                return StatusCode(400, fail_status);

            }
            else {

                temp_status.Add("1", v1); // check permission On resuest function name
                temp_status.Add("2", v2); // userid request transction 
                temp_status.Add("3", v3);  // data size | start   
                temp_status.Add("4", v4); // data size  | end , 
                temp_status.Add("5", v5); // param1
                temp_status.Add("6", v6); // param1
                temp_status.Add("7", v7); // param1
                temp_status.Add("8", v8); // param1
                temp_status.Add("9", v9); // param1
                temp_status.Add("10", v10); //param1

                try {

                    var meta_data = await UpdateASP.udm_hawkdata(JsonConvert.SerializeObject(temp_status));
                    if (meta_data.Count <= 0) {

                        fail_status.Add("status", "request no valid");
                        return StatusCode(400, fail_status);
                    }
                    else {

                        return StatusCode(200, meta_data);
                    }

                  

                }
                catch (Exception e) {

                    _ = log.Update_customerdata(" error =  >" + e.ToString());
                    fail_status.Add("status", "request no valid");
                    return StatusCode(400, fail_status);
                }

            }
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            Logmodel log = new Logmodel();
            Mysqlhawk hawkdb = new Mysqlhawk();
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

            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "=============================================================================> \r\n");
            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "RAW Body : " + body);


            object Datalake = "";
            var Datalake_status = "";
            var request_data = JObject.Parse(body);

            Check_customer chk = new Check_customer();
            Openaccount open = new Openaccount();
            try
            {
                switch (request_data["apiname"].ToString())
                {
                    case "exist_openaccount":

                        _ = log.swan_core_log("check_Exits_openaccount", "start : " + DateTime.Now.ToString());
                        Apiflow_default_condition = new Dictionary<object, object>();

                        var temp_apidata = JObject.Parse(body);
                        var response = new Dictionary<object, object>();


                        var example_case = await ASPS_exist_sample.logic_exist_customer(body);
                        
                        Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Apiflow_default_condition.Add("response", example_case);
                        Datalake_status = "true";

                        break;

                    case "reg_open_cop":
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "start : " +DateTime.Now.ToString());
                            Apiflow_default_condition = new Dictionary<object, object>();

                            var prepare_data_openflow_copp = JObject.Parse(body);
                           


                            var tmp_cop = await open.trigger_open_copp(body);

                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", " Return status : " + JsonConvert.SerializeObject(tmp_cop));

                            var temp_flow_cop = JObject.Parse(tmp_cop);
                            var flow_cop = new Dictionary<object, object>();

                            ///  build  remark  and  external data
                            if (prepare_data_openflow_copp["acc_status"].ToString() == "E")
                            {

                            // Register Hawkid to remark17

                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", " Hawkid : " + prepare_data_openflow_copp["hawkid"].ToString());

                            // add hawkid
                            var param_ins_hawkid = new Dictionary<object, object>();
                            param_ins_hawkid.Add("remark17", prepare_data_openflow_copp["hawkid"].ToString());
                            var param_condition_add_hawkid = new Dictionary<object, object>();
                            param_condition_add_hawkid.Add("flowid", temp_flow_cop["flowid"].ToString());
                            var upd_data_add_hawkid = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_hawkid), JsonConvert.SerializeObject(param_condition_add_hawkid));





                            var master_data_remark1 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_copp["hawkid"].ToString(), "","remark1"));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark1 : " + JsonConvert.SerializeObject(master_data_remark1));

                            var param_ins = new Dictionary<object, object>();
                            param_ins.Add("remark1", encode.base64_encode(master_data_remark1));
                            var param_condition = new Dictionary<object, object>();
                            param_condition.Add("flowid", temp_flow_cop["flowid"].ToString());
                            var upd_data = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins), JsonConvert.SerializeObject(param_condition));

                            var command_update_schema = "UPDATE tmp_open_part1 SET remark1 = CONVERT(FROM_BASE64(remark1)USING utf8) WHERE flowid = @flowid";
                            var udp_convert_schema = hawkdb.data_utility(command_update_schema, param_condition);

                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Update Remark1 : " + JsonConvert.SerializeObject(upd_data));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Update schema : " + JsonConvert.SerializeObject(udp_convert_schema));

                            //==================== Remark3 =========================================================
                            var master_data_remark3 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_copp["hawkid"].ToString(), "individual", "remark3"));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark3 : " + JsonConvert.SerializeObject(master_data_remark3));

                            var param_ins_remark3 = new Dictionary<object, object>();
                            param_ins_remark3.Add("remark1", encode.base64_encode(master_data_remark3));
                            var param_condition_remark3 = new Dictionary<object, object>();
                            param_condition_remark3.Add("flowid", temp_flow_cop["flowid"].ToString());
                            var upd_data_remark3 = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_remark3), JsonConvert.SerializeObject(param_condition_remark3));

                            //data_remark3(string hawkid, string custtype)

                            var ins_address = await HawknetModel.exist_data_information_state("exist_addres", await HawknetModel.check_exist_detail_flow(temp_flow_cop["flowid"].ToString(), "remark17"), temp_flow_cop["flowid"].ToString(), "");
                            var ins_bank = await HawknetModel.exist_data_information_state("exist_bank", await HawknetModel.check_exist_detail_flow(temp_flow_cop["flowid"].ToString(), "remark17"), temp_flow_cop["flowid"].ToString(), "");
                            var ins_person = await HawknetModel.exist_data_information_state("exist_person", await HawknetModel.check_exist_detail_flow(temp_flow_cop["flowid"].ToString(), "remark17"), temp_flow_cop["flowid"].ToString(), "");



                        }
                        else if (prepare_data_openflow_copp["acc_status"].ToString() == "N")
                            {




                            }


                        _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "flowid : " + temp_flow_cop["flowid"].ToString());
                            flow_cop.Add("flowid", temp_flow_cop["flowid"].ToString());

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_cop);
                            Datalake_status = "true";

                        break;


                    case "reg_open_individual":

                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "start : " + DateTime.Now.ToString());
                            Apiflow_default_condition = new Dictionary<object, object>();

                            var prepare_data_openflow_individual = JObject.Parse(body);
                         


                            var tmp_individual =  await open.trigger_open_individual(body);
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", " Return status : " + JsonConvert.SerializeObject(tmp_individual));

                            var temp_flow_individual = JObject.Parse(tmp_individual);
                            var flow_individual = new Dictionary<object,object>();


                            ///  build  remark  and  external data
                            ///  

                            if (prepare_data_openflow_individual["acc_status"].ToString() == "E")
                            {

                                // Register Hawkid to remark17

                                _ = log.swan_core_log("Request_newa_and_Exist_openaccount", " Hawkid : " + prepare_data_openflow_individual["hawkid"].ToString());

                                // add hawkid
                                var param_ins_hawkid = new Dictionary<object, object>();
                                param_ins_hawkid.Add("remark17", prepare_data_openflow_individual["hawkid"].ToString());
                                var param_condition_add_hawkid = new Dictionary<object, object>();
                                param_condition_add_hawkid.Add("flowid", temp_flow_individual["flowid"].ToString());
                                var upd_data_add_hawkid = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_hawkid), JsonConvert.SerializeObject(param_condition_add_hawkid));


                               var master_data_remark1 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_individual["hawkid"].ToString(), "", "remark1"));
                                _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark1 : " + JsonConvert.SerializeObject(master_data_remark1));
                              
                                var param_ins = new Dictionary<object, object>();
                                param_ins.Add("remark1", encode.base64_encode(master_data_remark1));
                                var param_condition = new Dictionary<object, object>();
                                param_condition.Add("flowid", temp_flow_individual["flowid"].ToString());
                                var upd_data = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins), JsonConvert.SerializeObject(param_condition));

                                var command_update_schema = "UPDATE tmp_open_part1 SET remark1 = CONVERT(FROM_BASE64(remark1)USING utf8) WHERE flowid = @flowid";
                                var udp_convert_schema = hawkdb.data_utility(command_update_schema, param_condition);    

                                _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Update Remark1 : " + JsonConvert.SerializeObject(upd_data));
                                _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Update schema : " + JsonConvert.SerializeObject(udp_convert_schema));

                            // ================== remark3=============================================

                            var master_data_remark3 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_individual["hawkid"].ToString(), "individual", "remark3"));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark3 : " + JsonConvert.SerializeObject(master_data_remark3));

                            var param_ins_remark3 = new Dictionary<object, object>();
                            param_ins_remark3.Add("remark3", encode.base64_encode(master_data_remark3));
                            var param_condition_remark3 = new Dictionary<object, object>();
                            param_condition_remark3.Add("flowid", temp_flow_individual["flowid"].ToString());
                            var upd_data_remark3 = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_remark3), JsonConvert.SerializeObject(param_condition_remark3));



                            // ================== remark3=============================================

                            var param_ins_remark5 = new Dictionary<object, object>();
                            param_ins_remark5.Add("remark5", "eyJTVzAzMl9mb3JtYXQ2LTAiOiAiIn0=");
                            var param_condition_remark5 = new Dictionary<object, object>();
                            param_condition_remark5.Add("flowid", temp_flow_individual["flowid"].ToString());
                            var upd_data_remark5 = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_remark5), JsonConvert.SerializeObject(param_condition_remark5));


                            var command_update_schema_remark5 = "UPDATE tmp_open_part1 SET remark5 = CONVERT(FROM_BASE64(remark5)USING utf8) WHERE flowid = @flowid";
                            var udp_convert_schema_remark5 = hawkdb.data_utility(command_update_schema_remark5, param_condition_remark5);


                            // ================== remark7=============================================

                            var master_data_remark7 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_individual["hawkid"].ToString(), "individual", "remark7"));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark7 : " + JsonConvert.SerializeObject(master_data_remark7));

                            var param_ins_remark7 = new Dictionary<object, object>();
                            param_ins_remark7.Add("remark7", encode.base64_encode(master_data_remark7));
                            var param_condition_remark7 = new Dictionary<object, object>();
                            param_condition_remark7.Add("flowid", temp_flow_individual["flowid"].ToString());
                            var upd_data_remark7 = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_remark7), JsonConvert.SerializeObject(param_condition_remark7));


                            // ================== remark8=============================================

                            var temp_remark8_data = new List<object>();
                            var master_data_remark8 = JsonConvert.SerializeObject(await HawknetCoreModel.customer_data_state(prepare_data_openflow_individual["hawkid"].ToString(), "individual", "remark8"));
                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "Master data remark8 : " + JsonConvert.SerializeObject(master_data_remark8));

                            //var loop_remark8_temp = JObject.Parse(master_data_remark8);

                            //foreach (var item_remark8 in loop_remark8_temp) {
                            //    var tmp_data = new Dictionary<object, object>();

                            //    tmp_data.Add("name", item_remark8.Key.ToString());
                            //    tmp_data.Add("value", item_remark8.Value.ToString());

                            //    temp_remark8_data.Add(tmp_data);

                            //}

                            var param_ins_remark8 = new Dictionary<object, object>();
                            param_ins_remark8.Add("remark8", encode.base64_encode(master_data_remark8));
                            var param_condition_remark8 = new Dictionary<object, object>();
                            param_condition_remark8.Add("flowid", temp_flow_individual["flowid"].ToString());
                            var upd_data_remark8 = hawkdb.data_update("tmp_open_part1", JsonConvert.SerializeObject(param_ins_remark8), JsonConvert.SerializeObject(param_condition_remark8));


                            var command_update_schema_remark8 = "UPDATE tmp_open_part1 SET remark8 = CONVERT(FROM_BASE64(remark8)USING utf8) WHERE flowid = @flowid";
                            var udp_convert_schema_remark8 = hawkdb.data_utility(command_update_schema_remark8, param_condition_remark8);

                            var ins_address = await HawknetModel.exist_data_information_state("exist_addres",await HawknetModel.check_exist_detail_flow(temp_flow_individual["flowid"].ToString(), "remark17"), temp_flow_individual["flowid"].ToString(), "");
                            var ins_bank = await HawknetModel.exist_data_information_state("exist_bank", await HawknetModel.check_exist_detail_flow(temp_flow_individual["flowid"].ToString(), "remark17"), temp_flow_individual["flowid"].ToString(), "");
                            var ins_person = await HawknetModel.exist_data_information_state("exist_person", await HawknetModel.check_exist_detail_flow(temp_flow_individual["flowid"].ToString(), "remark17"), temp_flow_individual["flowid"].ToString(), "");

                        }
                        else if (prepare_data_openflow_individual["acc_status"].ToString() == "N")
                            {




                            }


                            _ = log.swan_core_log("Request_newa_and_Exist_openaccount", "flowid : " + temp_flow_individual["flowid"].ToString());

                            flow_individual.Add("flowid", temp_flow_individual["flowid"].ToString());
                      
                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_individual);
                            Datalake_status = "true";

                      
                        break;

                    case "exits_account":
                        _= log.exits_customer("Request exits ========================= >");

                        try { 

                        Apiflow_default_condition = new Dictionary<object, object>();
                        var temp_exits = await chk.chk_exits(body);

                        Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Apiflow_default_condition.Add("response", temp_exits);
                        Datalake_status = "true";

                        _ = log.swan_core_log("Request_new_openaccount", "Result : " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {

                            _ = log.swan_core_log("Request_new_openaccount", "Error : " + e.ToString());
                        }
                        break;
                    //======================================================
                    case "kyc_compliance":
                        

                        _= log.kyc("Request kyc_compliance ========================= >");

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var kyc_compliance = new Dictionary<object, object>();

                            kyc_compliance.Add("userid", request_data["userid"].ToString());
                            kyc_compliance.Add("date", DateTime.Now.ToString());
                            kyc_compliance.Add("hawkid", request_data["hawkid"].ToString());
                            kyc_compliance.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            kyc_compliance.Add("acc_no", request_data["acc_no"].ToString());
                            kyc_compliance.Add("marketing_id", request_data["marketing_id"].ToString());
                            kyc_compliance.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            kyc_compliance.Add("marketing_team", request_data["marketing_team"].ToString());
                            kyc_compliance.Add("marketing_group", request_data["marketing_group"].ToString());
                            kyc_compliance.Add("formid", "9");
                            kyc_compliance.Add("title", request_data["title"].ToString());
                            kyc_compliance.Add("branch", request_data["branch"].ToString());
                            kyc_compliance.Add("formdata", request_data["formdata"].ToString());

                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(kyc_compliance));

                            var flow_update_kyc_compliance = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_kyc_compliance.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_kyc_compliance);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(kyc_compliance));
                            _ = log.Update_customerdata("Detail kyc_compliance :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow kyc_compliance : " + e.ToString());
                        }


                        break;

                    case "kyc_legal":
                         _= log.kyc("Request kyc_legal ========================= >");

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var kyc_legal = new Dictionary<object, object>();

                            kyc_legal.Add("userid", request_data["userid"].ToString());
                            kyc_legal.Add("date", DateTime.Now.ToString());
                            kyc_legal.Add("hawkid", request_data["hawkid"].ToString());
                            kyc_legal.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            kyc_legal.Add("acc_no", request_data["acc_no"].ToString());
                            kyc_legal.Add("marketing_id", request_data["marketing_id"].ToString());
                            kyc_legal.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            kyc_legal.Add("marketing_team", request_data["marketing_team"].ToString());
                            kyc_legal.Add("marketing_group", request_data["marketing_group"].ToString());
                            kyc_legal.Add("formid", "10");
                            kyc_legal.Add("title", request_data["title"].ToString());
                            kyc_legal.Add("branch", request_data["branch"].ToString());
                            kyc_legal.Add("formdata", request_data["formdata"].ToString());

                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(kyc_legal));

                            var flow_update_kyc_legal = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_kyc_legal.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_kyc_legal);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(kyc_legal));
                            _ = log.Update_customerdata("Detail kyc_legal :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow kyc_legal : " + e.ToString());
                        }
                        break;

                    //======================================================
                    case "update_customer":

                        Apiflow_default_condition = new Dictionary<object, object>();


                        try
                        {
                            var update_customer = new Dictionary<object, object>();

                            update_customer.Add("userid", request_data["userid"].ToString());
                            update_customer.Add("date", DateTime.Now.ToString());
                            update_customer.Add("hawkid", request_data["hawkid"].ToString());
                            update_customer.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_customer.Add("acc_no", request_data["acc_no"].ToString());
                            update_customer.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_customer.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_customer.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_customer.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_customer.Add("formid", "14");
                            update_customer.Add("title", request_data["title"].ToString());
                            update_customer.Add("branch", request_data["branch"].ToString());
                            update_customer.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_customer));

                            var flow_update_customer = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_customer.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_customer);
                            Datalake_status = "true";

                            _ = log.swan_core_log("Update_customer","Detail Raw :  " + JsonConvert.SerializeObject(update_customer));
                            _ = log.swan_core_log("Update_customer","Detail update_customer :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _= log.Update_customerdata(" Error Request Flow update_customer : "+e.ToString());
                        }

                        break;

                    case "update_address":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_address = new Dictionary<object, object>();

                            update_address.Add("userid", request_data["userid"].ToString());
                            update_address.Add("date", DateTime.Now.ToString());
                            update_address.Add("hawkid", request_data["hawkid"].ToString());
                            update_address.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_address.Add("acc_no", request_data["acc_no"].ToString());
                            update_address.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_address.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_address.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_address.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_address.Add("formid", "15");
                            update_address.Add("title", request_data["title"].ToString());
                            update_address.Add("branch", request_data["branch"].ToString());
                            update_address.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_address));

                            var flow_update_address = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_address.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_address);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_address));
                            _ = log.Update_customerdata("Detail update_address :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_address : " + e.ToString());
                        }

                        break;

                    case "update_relate":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_relate = new Dictionary<object, object>();

                            update_relate.Add("userid", request_data["userid"].ToString());
                            update_relate.Add("date", DateTime.Now.ToString());
                            update_relate.Add("hawkid", request_data["hawkid"].ToString());
                            update_relate.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_relate.Add("acc_no", request_data["acc_no"].ToString());
                            update_relate.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_relate.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_relate.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_relate.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_relate.Add("formid", "16");
                            update_relate.Add("title", request_data["title"].ToString());
                            update_relate.Add("branch", request_data["branch"].ToString());
                            update_relate.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_relate));

                            var flow_update_relate = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_relate.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_relate);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_relate));
                            _ = log.Update_customerdata("Detail update_relate :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_relate : " + e.ToString());
                        }

                        break;


                    case "update_bank":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_bank = new Dictionary<object, object>();

                            update_bank.Add("userid", request_data["userid"].ToString());
                            update_bank.Add("date", DateTime.Now.ToString());
                            update_bank.Add("hawkid", request_data["hawkid"].ToString());
                            update_bank.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_bank.Add("acc_no", request_data["acc_no"].ToString());
                            update_bank.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_bank.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_bank.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_bank.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_bank.Add("formid", "17");
                            update_bank.Add("title", request_data["title"].ToString());
                            update_bank.Add("branch", request_data["branch"].ToString());
                            update_bank.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_bank));

                            var flow_update_bank = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_bank.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_bank);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_bank));
                            _ = log.Update_customerdata("Detail update_bank :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_bank : " + e.ToString());
                        }


                       
                        break;

                    case "update_document":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_document = new Dictionary<object, object>();

                            update_document.Add("userid", request_data["userid"].ToString());
                            update_document.Add("date", DateTime.Now.ToString());
                            update_document.Add("hawkid", request_data["hawkid"].ToString());
                            update_document.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_document.Add("acc_no", request_data["acc_no"].ToString());
                            update_document.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_document.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_document.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_document.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_document.Add("formid", "18");
                            update_document.Add("title", request_data["title"].ToString());
                            update_document.Add("branch", request_data["branch"].ToString());
                            update_document.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_document));

                            var flow_update_document = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_document.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_document);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_document));
                            _ = log.Update_customerdata("Detail update_document :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_document : " + e.ToString());
                        }

                        break;


                    case "update_credit":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_credit = new Dictionary<object, object>();

                            update_credit.Add("userid", request_data["userid"].ToString());
                            update_credit.Add("date", DateTime.Now.ToString());
                            update_credit.Add("hawkid", request_data["hawkid"].ToString());
                            update_credit.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_credit.Add("acc_no", request_data["acc_no"].ToString());
                            update_credit.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_credit.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_credit.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_credit.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_credit.Add("formid", "19");
                            update_credit.Add("title", request_data["title"].ToString());
                            update_credit.Add("branch", request_data["branch"].ToString());
                            update_credit.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_credit));

                            var flow_update_credit = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_credit.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_credit);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_credit));
                            _ = log.Update_customerdata("Detail update_credit :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_credit : " + e.ToString());
                        }


                        break;

                    case "update_kyc":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_kyc = new Dictionary<object, object>();

                            update_kyc.Add("userid", request_data["userid"].ToString());
                            update_kyc.Add("date", DateTime.Now.ToString());
                            update_kyc.Add("hawkid", request_data["hawkid"].ToString());
                            update_kyc.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_kyc.Add("acc_no", request_data["acc_no"].ToString());
                            update_kyc.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_kyc.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_kyc.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_kyc.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_kyc.Add("formid", "20");
                            update_kyc.Add("title", request_data["title"].ToString());
                            update_kyc.Add("branch", request_data["branch"].ToString());
                            update_kyc.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_kyc));

                            var flow_update_kyc = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_kyc.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_kyc);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_kyc));
                            _ = log.Update_customerdata("Detail update_kyc :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_kyc : " + e.ToString());
                        }

                        break;

                    case "update_suite":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_suite = new Dictionary<object, object>();

                            update_suite.Add("userid", request_data["userid"].ToString());
                            update_suite.Add("date", DateTime.Now.ToString());
                            update_suite.Add("hawkid", request_data["hawkid"].ToString());
                            update_suite.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_suite.Add("acc_no", request_data["acc_no"].ToString());
                            update_suite.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_suite.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_suite.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_suite.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_suite.Add("formid", "21");
                            update_suite.Add("title", request_data["title"].ToString());
                            update_suite.Add("branch", request_data["branch"].ToString());
                            update_suite.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_suite));

                            var flow_update_suite = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_suite.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_suite);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_suite));
                            _ = log.Update_customerdata("Detail update_suite :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_suite : " + e.ToString());
                        }

                        break;

                    case "update_fatca":

                        Apiflow_default_condition = new Dictionary<object, object>();


                        try
                        {
                            var update_fatca = new Dictionary<object, object>();

                            update_fatca.Add("userid", request_data["userid"].ToString());
                            update_fatca.Add("date", DateTime.Now.ToString());
                            update_fatca.Add("hawkid", request_data["hawkid"].ToString());
                            update_fatca.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_fatca.Add("acc_no", request_data["acc_no"].ToString());
                            update_fatca.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_fatca.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_fatca.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_fatca.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_fatca.Add("formid", "22");
                            update_fatca.Add("title", request_data["title"].ToString());
                            update_fatca.Add("branch", request_data["branch"].ToString());
                            update_fatca.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_fatca));

                            var flow_update_fatca = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_fatca.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_fatca);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_fatca));
                            _ = log.Update_customerdata("Detail update_fatca :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_fatca : " + e.ToString());
                        }

                        break;

                    case "update_marketing":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_marketing = new Dictionary<object, object>();

                            update_marketing.Add("userid", request_data["userid"].ToString());
                            update_marketing.Add("date", DateTime.Now.ToString());
                            update_marketing.Add("hawkid", request_data["hawkid"].ToString());
                            update_marketing.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_marketing.Add("acc_no", request_data["acc_no"].ToString());
                            update_marketing.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_marketing.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_marketing.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_marketing.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_marketing.Add("formid", "23");
                            update_marketing.Add("title", request_data["title"].ToString());
                            update_marketing.Add("branch", request_data["branch"].ToString());
                            update_marketing.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_marketing));

                            var flow_update_marketing = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_marketing.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_marketing);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_marketing));
                            _ = log.Update_customerdata("Detail update_marketing :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_marketing : " + e.ToString());
                        }

                        break;

                    case "update_lockunlock":

                        Apiflow_default_condition = new Dictionary<object, object>();


                        try
                        {
                            var update_lockunlock = new Dictionary<object, object>();

                            update_lockunlock.Add("userid", request_data["userid"].ToString());
                            update_lockunlock.Add("date", DateTime.Now.ToString());
                            update_lockunlock.Add("hawkid", request_data["hawkid"].ToString());
                            update_lockunlock.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_lockunlock.Add("acc_no", request_data["acc_no"].ToString());
                            update_lockunlock.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_lockunlock.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_lockunlock.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_lockunlock.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_lockunlock.Add("formid", "24");
                            update_lockunlock.Add("title", request_data["title"].ToString());
                            update_lockunlock.Add("branch", request_data["branch"].ToString());
                            update_lockunlock.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_lockunlock));

                            var flow_update_lockunlock = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_lockunlock.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_lockunlock);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_lockunlock));
                            _ = log.Update_customerdata("Detail update_lockunlock :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_lockunlock : " + e.ToString());
                        }

                        break;

                    case "update_closeaccount":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_closeaccount = new Dictionary<object, object>();

                            update_closeaccount.Add("userid", request_data["userid"].ToString());
                            update_closeaccount.Add("date", DateTime.Now.ToString());
                            update_closeaccount.Add("hawkid", request_data["hawkid"].ToString());
                            update_closeaccount.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            update_closeaccount.Add("acc_no", request_data["acc_no"].ToString());
                            update_closeaccount.Add("marketing_id", request_data["marketing_id"].ToString());
                            update_closeaccount.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            update_closeaccount.Add("marketing_team", request_data["marketing_team"].ToString());
                            update_closeaccount.Add("marketing_group", request_data["marketing_group"].ToString());
                            update_closeaccount.Add("formid", "25");
                            update_closeaccount.Add("title", request_data["title"].ToString());
                            update_closeaccount.Add("branch", request_data["branch"].ToString());
                            update_closeaccount.Add("formdata", request_data["formdata"].ToString());


                            var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_closeaccount));

                            var flow_update_closeaccount = new Dictionary<object, object>();

                            var temp_flow_state = JObject.Parse(status_flow_trigger);

                            flow_update_closeaccount.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_closeaccount);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_closeaccount));
                            _ = log.Update_customerdata("Detail update_closeaccount :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_closeaccount : " + e.ToString());
                        }

                        break;

                    case "update_account":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var update_account = new Dictionary<object, object>();

                                update_account.Add("userid", request_data["userid"].ToString());
                                update_account.Add("date", DateTime.Now.ToString());
                                update_account.Add("hawkid", request_data["hawkid"].ToString());
                                update_account.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                                update_account.Add("acc_no", request_data["acc_no"].ToString());
                                update_account.Add("marketing_id", request_data["marketing_id"].ToString());
                                update_account.Add("marketing_branch", request_data["marketing_branch"].ToString());
                                update_account.Add("marketing_team", request_data["marketing_team"].ToString());
                                update_account.Add("marketing_group", request_data["marketing_group"].ToString());
                                update_account.Add("formid", "26");
                                update_account.Add("title", request_data["title"].ToString());
                                update_account.Add("branch", request_data["branch"].ToString());
                                update_account.Add("formdata", request_data["formdata"].ToString());


                                var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(update_account));

                                var flow_update_account = new Dictionary<object, object>();

                                var temp_flow_state = JObject.Parse(status_flow_trigger);

                                flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                                Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                                Apiflow_default_condition.Add("response", flow_update_account);
                                Datalake_status = "true";

                                _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(update_account));
                                _ = log.Update_customerdata("Detail update_account :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow update_account : " + e.ToString());
                        }

                        break;


                        //============================ Report Interface==========================
                       // "apiname":"update_address",
	                   // "userid":"ite-000120190421-13",
	                   // "hawkid":"",
	                   // "cust_id_ref":"",
	                   // "acc_no":"",
	                   // "marketing_id":"",
	                   // "marketing_branch":"",
	                   // "marketing_team":"",
	                   // "marketing_group":"",
	                   // "branch":"00",
                       //  "reportid":"00",
	                   // "title":"Update naja",
	                   // "formdata":""
                    case "report_batch":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var report_batch = new Dictionary<object, object>();

                            report_batch.Add("userid", request_data["userid"].ToString());
                            report_batch.Add("date", DateTime.Now.ToString());
                            report_batch.Add("hawkid", request_data["hawkid"].ToString());
                            report_batch.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            report_batch.Add("acc_no", request_data["acc_no"].ToString());
                            report_batch.Add("marketing_id", request_data["marketing_id"].ToString());
                            report_batch.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            report_batch.Add("marketing_team", request_data["marketing_team"].ToString());
                            report_batch.Add("marketing_group", request_data["marketing_group"].ToString());
                            report_batch.Add("reportid", request_data["reportid"].ToString());
                            report_batch.Add("title", request_data["title"].ToString());
                            report_batch.Add("branch", request_data["branch"].ToString());
                            report_batch.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                           // var temp_flow_state = JObject.Parse(status_flow_trigger);
                           // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(report_batch));
                            _ = log.Update_customerdata("Detail report_batch :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow report_batch : " + e.ToString());
                        }

                        break;
                    case "report_generator":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var report_generator = new Dictionary<object, object>();

                            report_generator.Add("userid", request_data["userid"].ToString());
                            report_generator.Add("date", DateTime.Now.ToString());
                            report_generator.Add("hawkid", request_data["hawkid"].ToString());
                            report_generator.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            report_generator.Add("acc_no", request_data["acc_no"].ToString());
                            report_generator.Add("marketing_id", request_data["marketing_id"].ToString());
                            report_generator.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            report_generator.Add("marketing_team", request_data["marketing_team"].ToString());
                            report_generator.Add("marketing_group", request_data["marketing_group"].ToString());
                            report_generator.Add("reportid", request_data["reportid"].ToString());
                            report_generator.Add("title", request_data["title"].ToString());
                            report_generator.Add("branch", request_data["branch"].ToString());
                            report_generator.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                            // var temp_flow_state = JObject.Parse(status_flow_trigger);
                            // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(report_generator));
                            _ = log.Update_customerdata("Detail report_generator :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow report_generator : " + e.ToString());
                        }

                        break;
                    case "report_preview":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var report_preview = new Dictionary<object, object>();

                            report_preview.Add("userid", request_data["userid"].ToString());
                            report_preview.Add("date", DateTime.Now.ToString());
                            report_preview.Add("hawkid", request_data["hawkid"].ToString());
                            report_preview.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            report_preview.Add("acc_no", request_data["acc_no"].ToString());
                            report_preview.Add("marketing_id", request_data["marketing_id"].ToString());
                            report_preview.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            report_preview.Add("marketing_team", request_data["marketing_team"].ToString());
                            report_preview.Add("marketing_group", request_data["marketing_group"].ToString());
                            report_preview.Add("reportid", request_data["reportid"].ToString());
                            report_preview.Add("title", request_data["title"].ToString());
                            report_preview.Add("branch", request_data["branch"].ToString());
                            report_preview.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                            // var temp_flow_state = JObject.Parse(status_flow_trigger);
                            // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(report_preview));
                            _ = log.Update_customerdata("Detail report_preview :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow report_preview : " + e.ToString());
                        }

                        break;
                    // "apiname":"update_address",
                    // "userid":"ite-000120190421-13",
                    // "hawkid":"",
                    // "cust_id_ref":"",
                    // "acc_no":"",
                    // "marketing_id":"",
                    // "marketing_branch":"",
                    // "marketing_team":"",
                    // "marketing_group":"",
                    // "branch":"00",
                    // "mailid":"00",
                    // "title":"Update naja",
                    // "formdata":""

                    case "mail_batch":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var report_batch = new Dictionary<object, object>();

                            report_batch.Add("userid", request_data["userid"].ToString());
                            report_batch.Add("date", DateTime.Now.ToString());
                            report_batch.Add("hawkid", request_data["hawkid"].ToString());
                            report_batch.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            report_batch.Add("acc_no", request_data["acc_no"].ToString());
                            report_batch.Add("marketing_id", request_data["marketing_id"].ToString());
                            report_batch.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            report_batch.Add("marketing_team", request_data["marketing_team"].ToString());
                            report_batch.Add("marketing_group", request_data["marketing_group"].ToString());
                            report_batch.Add("mailid", request_data["templateid"].ToString());
                            report_batch.Add("title", request_data["title"].ToString());
                            report_batch.Add("branch", request_data["branch"].ToString());
                            report_batch.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                            // var temp_flow_state = JObject.Parse(status_flow_trigger);
                            // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(report_batch));
                            _ = log.Update_customerdata("Detail report_batch :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow report_batch : " + e.ToString());
                        }

                        break;

                    case "mail_attach":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var mail_attach = new Dictionary<object, object>();

                            mail_attach.Add("userid", request_data["userid"].ToString());
                            mail_attach.Add("date", DateTime.Now.ToString());
                            mail_attach.Add("hawkid", request_data["hawkid"].ToString());
                            mail_attach.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            mail_attach.Add("acc_no", request_data["acc_no"].ToString());
                            mail_attach.Add("marketing_id", request_data["marketing_id"].ToString());
                            mail_attach.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            mail_attach.Add("marketing_team", request_data["marketing_team"].ToString());
                            mail_attach.Add("marketing_group", request_data["marketing_group"].ToString());
                            mail_attach.Add("mailid", request_data["templateid"].ToString());
                            mail_attach.Add("title", request_data["title"].ToString());
                            mail_attach.Add("branch", request_data["branch"].ToString());
                            mail_attach.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                            // var temp_flow_state = JObject.Parse(status_flow_trigger);
                            // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(mail_attach));
                            _ = log.Update_customerdata("Detail mail_attach :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow mail_attach : " + e.ToString());
                        }

                        break;

                    case "mail_approve":

                        Apiflow_default_condition = new Dictionary<object, object>();

                        try
                        {
                            var mail_approve = new Dictionary<object, object>();

                            mail_approve.Add("userid", request_data["userid"].ToString());
                            mail_approve.Add("date", DateTime.Now.ToString());
                            mail_approve.Add("hawkid", request_data["hawkid"].ToString());
                            mail_approve.Add("cust_id_ref", request_data["cust_id_ref"].ToString());
                            mail_approve.Add("acc_no", request_data["acc_no"].ToString());
                            mail_approve.Add("marketing_id", request_data["marketing_id"].ToString());
                            mail_approve.Add("marketing_branch", request_data["marketing_branch"].ToString());
                            mail_approve.Add("marketing_team", request_data["marketing_team"].ToString());
                            mail_approve.Add("marketing_group", request_data["marketing_group"].ToString());
                            mail_approve.Add("mailid", request_data["templateid"].ToString());
                            mail_approve.Add("title", request_data["title"].ToString());
                            mail_approve.Add("branch", request_data["branch"].ToString());
                            mail_approve.Add("formdata", request_data["formdata"].ToString());


                            //var status_flow_trigger = await Trigger_action.trigger_dm_flow(JsonConvert.SerializeObject(report_batch));

                            var flow_update_account = new Dictionary<object, object>();

                            // var temp_flow_state = JObject.Parse(status_flow_trigger);
                            // flow_update_account.Add("flowid", temp_flow_state["flowid"].ToString());//flowid

                            Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                            Apiflow_default_condition.Add("response", flow_update_account);
                            Datalake_status = "true";

                            _ = log.Update_customerdata("Detail Raw :  " + JsonConvert.SerializeObject(mail_approve));
                            _ = log.Update_customerdata("Detail mail_approve :  " + JsonConvert.SerializeObject(Apiflow_default_condition));

                        }
                        catch (Exception e)
                        {
                            _ = log.Update_customerdata(" Error Request Flow mail_approve : " + e.ToString());
                        }

                        break;
                    //======================================================



                    default:
                        Apiflow_default_condition = new Dictionary<object, object>();
                        Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                        Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                        Datalake_status = "fail";
                        break;
                }
            }
            catch (Exception e) {

                Apiflow_default_condition = new Dictionary<object, object>();
                Apiflow_default_condition.Add("Error", e.ToString());
                Apiflow_default_condition.Add("info", "Web Api Access Not allow");
                Apiflow_default_condition.Add("response time", DateTime.Now.ToString("dd-MM-yyy HH':'mm':'ss"));
                Datalake_status = " ";

            }

            var status_api = "";
            if (Datalake_status == "Create")
            {
                status_api = "201";
            }
            else if (Datalake_status == "true")
            {
                status_api = "200";
            }
            else if (Datalake_status == "fail")
            {
                status_api = "400";
            }
            else if (Datalake_status == " ")
            {
                status_api = "500";
            }
            return StatusCode(Int32.Parse(status_api), Apiflow_default_condition);

          
        }

       


        // PUT: api/App1/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
             return StatusCode(400, "method Not allow");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(400, "method Not allow");
        }
    }

  
}

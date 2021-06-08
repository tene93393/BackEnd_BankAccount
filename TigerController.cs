
using System.Collections.Generic;
using System.Diagnostics;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace service.Controllers
{
    public class TigerController : Controller
    {      
        private Appconfig config = new Appconfig();
      

        public static string documentdb = Appconfig.client_config("ite-000120190421", "h3_db");


  //`extension` varchar(255) DEFAULT NULL,
  //`key1` varchar(255) DEFAULT NULL,  -- conditon
  //`key2` varchar(255) DEFAULT NULL, -- flow
  // `key3` varchar(255) DEFAULT NULL, -- form
  //`key4` varchar(255) DEFAULT NULL, -- custcode - or staffcode
  //`key5` varchar(255) DEFAULT NULL, -- acc_no
  //`key6` varchar(255) DEFAULT NULL, -- hawkid
  //`key7` varchar(255) DEFAULT NULL, -- cardid
  //`key8` varchar(255) DEFAULT NULL, -- email
  //`key9` varchar(255) DEFAULT NULL, -- cardid
  //`key10` varchar(255) DEFAULT NULL, -- other
  //`file` varchar(500) DEFAULT NULL,
  //`filename` varchar(500) DEFAULT NULL,
  //`temp_file` varchar(500) DEFAULT NULL,
  //`page` varchar(3) DEFAULT NULL,
  //`channel` varchar(255) DEFAULT NULL,
  //`subchannel` varchar(255) DEFAULT NULL,
  //`document_group` varchar(100) DEFAULT NULL,
  //`document_code` varchar(100) DEFAULT NULL,
  //`document_name` varchar(100) DEFAULT NULL,
  //`document_type` varchar(100) DEFAULT NULL,
  //`size` varchar(10) DEFAULT NULL,
  //`status` varchar(2) DEFAULT NULL,
  //`content_identity` varchar(200) DEFAULT NULL,
  //`uploaduser` varchar(50) DEFAULT NULL,
  //`updateuser` varchar(50) DEFAULT NULL,
  //`uploadtime` datetime DEFAULT NULL,
  //`updatetime` datetime DEFAULT NULL,
  //`document_expire` datetime DEFAULT NULL,
  //`log_date` datetime DEFAULT NULL,
  //`detail_title` varchar(100) DEFAULT NULL, -- title_เอกสาร
  //`detail1` varchar(500) DEFAULT NULL,  -- รายละเอียด1
  //`detail2` varchar(500) DEFAULT NULL,  -- รายละเอียด2
  //`detail3` varchar(500) DEFAULT NULL,  -- รายละเอียด3



        [Proofpoint]
        public IActionResult Index()
        {
            ViewBag.comment = "Power By Netcore 2.2";
            return View("~/Views/web001/Tiger/Index.cshtml");
        }
        //[Proofpoint]
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file, string docid, string doccode, string document_group, string condition, string account_type, string flowid, string userid, string formid)
        {
            Logmodel log = new Logmodel();
            var status_upload = new Dictionary<object, object>();

            var extension = "";
            var filename = "";
            var path = "";
            var temp_path = "";
            _ = log.upload("============== Start Upload Workflow  ===================");


            string base64String = "";
            try
            {

           
                if (file == null || file.Length == 0)
                {
                    //return Content("file not selected");
                    _ = log.upload("============== file value Not null ===================");


                    status_upload.Add("status", "file not selected");
                    return Json(status_upload);
                }

                path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", file.GetFilename());
                temp_path = Path.Combine("/Upload", "temp", file.GetFilename());

                _ = log.upload("============== RAW Path File ===================" + path.ToString());


                filename = file.GetFilename();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                extension = Path.GetExtension(path).ToLowerInvariant();
              

                _ = log.upload("Extentsion Upload : "+extension);

                if (extension == ".pdf" | extension == ".PDF")
                {

                    base64String = "";
                }
                else if (extension == ".jpg" | extension == ".JPG" | extension == ".png" | extension == ".PNG")
                {

                    //using (Image image = Image.FromFile(path))
                    //{
                    //    using (MemoryStream m = new MemoryStream())
                    //    {
                    //        image.Save(m, image.RawFormat);
                    //        byte[] imageBytes = m.ToArray();
                    //        base64String = Convert.ToBase64String(imageBytes);
                    //    }
                    //}

                    byte[] imageArray = System.IO.File.ReadAllBytes(path);
                    base64String = Convert.ToBase64String(imageArray);

                }
                else {

                    status_upload.Add("status", "file not selected");
                    return Json(status_upload);
                   
                }

               // _ = log.upload("docid =====> " + docid);
               // _ = log.upload("doccode =====> " + doccode);
               // _ = log.upload("document_group =====> " + document_group);
               // _ = log.upload("condition =====> " + condition);
               // _ = log.upload("account_type =====> " + account_type);
               // _ = log.upload("flowid =====> " + flowid);
               // _ = log.upload("userid =====> " + userid);
               // _ = log.upload("formid =====> " + formid);

                try {

                    var document_info = await Edocument_model.document_group_info(document_group, doccode);


                    string[] edocument_obj = condition.Split('|');

                    foreach (var document_condition in edocument_obj)
                    {




                        var document_workflow = new Dictionary<object, object>();
                        document_workflow.Add("date", DateTime.Now.ToString("yyyyMMdd H:i:s"));
                        document_workflow.Add("method", "TSW001");
                        // document_workflow.Add("docid", docid);
                        document_workflow.Add("base64data", base64String);
                        document_workflow.Add("path", path);
                        document_workflow.Add("userid", userid);

                        document_workflow.Add("extension", extension);
                        document_workflow.Add("key1", document_condition.Trim()); //  --conditon
                        document_workflow.Add("key2", flowid); // -- flow
                        document_workflow.Add("key3", formid); // -- form
                        document_workflow.Add("key4", ""); // -- custcode - or staffcode
                        document_workflow.Add("key5", ""); // -- acc_no
                        document_workflow.Add("key6", ""); // -- hawkid
                        document_workflow.Add("key7", ""); //  -- cardid
                        document_workflow.Add("key8", ""); // -- email
                        document_workflow.Add("key9", document_condition.Substring(0, 4)); // -- accounttype
                        document_workflow.Add("key10", "openacc"); // -- other

                        document_workflow.Add("file", temp_path);
                        document_workflow.Add("filename", filename);
                        document_workflow.Add("temp_file", temp_path);
                        document_workflow.Add("page", "0");
                        document_workflow.Add("channel", "Flow");
                        document_workflow.Add("subchannel", "web");

                        document_workflow.Add("document_group", document_group);
                        document_workflow.Add("document_code", doccode);
                        document_workflow.Add("document_name", document_info["document_name"].ToString());
                        document_workflow.Add("document_type", document_info["document_type"].ToString());
                        document_workflow.Add("status", "Y");

                        document_workflow.Add("size", file.Length);
                        document_workflow.Add("content_identity", "");
                        document_workflow.Add("uploaduser", userid);
                        document_workflow.Add("updateuser", userid);
                        document_workflow.Add("uploadtime", DateTime.Now.ToString("yyyMMdd HH:mm:ss"));
                        document_workflow.Add("updatetime", DateTime.Now.ToString("yyyMMdd HH:mm:ss"));
                        document_workflow.Add("document_expire", "");
                        document_workflow.Add("log_date", DateTime.Now.ToString("yyyMMdd HH:mm:ss"));
                        document_workflow.Add("detail_title", "");
                        document_workflow.Add("detail1", "");
                        document_workflow.Add("detail2", "");
                        document_workflow.Add("detail3", "");

                        _ = log.swan_core_log("Edocument_flow", " Debug Edocuement Condition  : " + document_condition);
                        _ = log.swan_core_log("Edocument_flow", " Debug Parse Edocuement Condition  : " + document_condition.Substring(0, 4));

                      

                        var document_status_upload = await Edocument_model.tiger_document_system(document_workflow);

                        _ = log.swan_core_log("Edocument_flow", "Edocument Flow send to Savedb :  " + JsonConvert.SerializeObject(document_status_upload));


                        // status_upload.Add("date", DateTime.Now.ToString("yyyyMMdd H:i:s"));
                        //status_upload.Add("Extension", extension);


                        // check condition approve workflow

                        var edoc_chk = await Edocument.Edocument_flow_require_count(flowid);

                        var edoc_require = await document_require_info(document_condition.Trim(), condition, "`options`");


                        _ = log.swan_core_log("Edocument_flow", "Edocument Require ?  : " + JsonConvert.SerializeObject(edoc_require));

                        _ = log.swan_core_log("Edocument_flow", "Edocument Status : " + JsonConvert.SerializeObject(edoc_chk));

                        if (edoc_chk.ToString() == "0" & edoc_require == "Y")
                        {
                            var formid_action = await Swan_flow_data.flow_info(flowid, "formid");

                            _ = log.swan_core_log("Edocument_flow", "Edocument Approve formid : " + formid_action);

                            var status_approve = await Swan_flow_utility.Auto_approve("ite_000120190710-00","", flowid ,formid_action, "Edoc Approve");
                            _ = log.swan_core_log("Edocument_flow", "Require Edoc Approve Flow : " + JsonConvert.SerializeObject(status_approve));


                        }
                        else
                        {
                            _ = log.swan_core_log("Edocument_flow", "Require Edoc Waite for Upload : ");

                        }

                    }

                    return Json(status_upload);


                }
                catch (Exception e ) {

                    _ = log.upload("Error Upload Edocument : " + e.ToString());

                    status_upload.Add("status", "process Upload Fail");
                    return Json(status_upload);
                }

            }
            catch (Exception e) {

                _ = log.upload("Error Upload Edocument : "+e.ToString());
                status_upload.Add("status", "process Upload Fail");
                return Json(status_upload);
            }
           
        }


        //[Proofpoint]
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> Uploadtiger(IFormFile file, string data)
        {
            Logmodel log = new Logmodel();
            var status_upload = new Dictionary<object, object>();

            var extension = "";
            var filename = "";
            var path = "";
            var temp_path = "";
            _ = log.swan_core_log("tiger_upload","============== Start Upload Workflow  ===================");

            _ = log.swan_core_log("tiger_upload", "Data : " + data);


            string base64String = "";
            try
            {


                if (file == null || file.Length == 0)
                {
                    //return Content("file not selected");
                    _ = log.swan_core_log("tiger_upload", "============== file value Not null ===================");


                    status_upload.Add("status", "file not selected");
                    return Json(status_upload);
                }

                path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", file.GetFilename());
                temp_path = Path.Combine("/Upload", "temp", file.GetFilename());

                _ = log.swan_core_log("tiger_upload", "============== RAW Path File ===================" + path.ToString());


                filename = file.GetFilename();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                extension = Path.GetExtension(path).ToLowerInvariant();


                _ = log.swan_core_log("tiger_upload", "Extentsion Upload : " + extension);

                if (extension == ".pdf" | extension == ".PDF")
                {

                    base64String = "";
                }
                else if (extension == ".jpg" | extension == ".JPG" | extension == ".png" | extension == ".PNG")
                {

                    //using (Image image = Image.FromFile(path))
                    //{
                    //    using (MemoryStream m = new MemoryStream())
                    //    {
                    //        image.Save(m, image.RawFormat);
                    //        byte[] imageBytes = m.ToArray();
                    //        base64String = Convert.ToBase64String(imageBytes);
                    //    }
                    //}

                    byte[] imageArray = System.IO.File.ReadAllBytes(path);
                    base64String = Convert.ToBase64String(imageArray);

                }
                else
                {

                    status_upload.Add("status", "file not selected");
                    return Json(status_upload);

                }

                // _ = log.upload("docid =====> " + docid);
                // _ = log.upload("doccode =====> " + doccode);
                // _ = log.upload("document_group =====> " + document_group);
                // _ = log.upload("condition =====> " + condition);
                // _ = log.upload("account_type =====> " + account_type);
                // _ = log.upload("flowid =====> " + flowid);
                // _ = log.upload("userid =====> " + userid);
                // _ = log.upload("formid =====> " + formid);

                try
                {

                   

                   
                    status_upload.Add("Detail", data);
                    return Json(status_upload);


                }
                catch (Exception e)
                {

                    _ = log.upload("Error Upload Edocument : " + e.ToString());

                    status_upload.Add("status", "process Upload Fail");
                    return Json(status_upload);
                }

            }
            catch (Exception e)
            {

                _ = log.upload("Error Upload Edocument : " + e.ToString());
                status_upload.Add("status", "process Upload Fail");
                return Json(status_upload);
            }

        }



        public static async Task<string> document_require_info(string condition, string accounttype,string dbresult)
        {
            Logmodel log = new Logmodel();
            Mysqlhawk hawk = new Mysqlhawk();

            var fnc_data = "";

            try
            {
                var Command2 = "SELECT "+ dbresult + " AS data_db FROM t_edocument_require WHERE `condition` = @1 AND account_type = @2 AND `status`  = 'Y'";

                _ = log.swan_core_log("Edocument_flow", "Edocument Get Command : " + Command2);

                var param2 = new Dictionary<object, object>();
                param2.Add("1", condition);
                param2.Add("2", condition.Substring(0, 4));

                var data_flow = await hawk.data_with_col_api(Command2, param2);
                var temp_data = JArray.Parse(JsonConvert.SerializeObject(data_flow));

                _ = log.swan_core_log("Edocument_flow", "document_require_info : " + JsonConvert.SerializeObject(temp_data));

                fnc_data = temp_data[0]["data_db"].ToString();

                return fnc_data;

            }
            catch (Exception e)
            {

                _ = log.swan_core_log("Edocument_flow", "Edocument Get flowinfo : " + e.ToString());

                fnc_data = "404";

                return fnc_data;
            }
        }


        [Proofpoint]
        public async Task<IActionResult> Clear_temp_document(string docid,string userid,string formid)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            var temp_result = new List<object>();
            try
            {

                if (String.IsNullOrEmpty(docid))
                {
                    temp_result.Add("Fail:no docid");
                }
                else
                {
                    var hash_data = encode.base64_decode(docid);
                    var hash_aes = await encode.encode_aes("decrypt", hash_data);
                    string[] parts = hash_aes.Split('|');
                    var clear_status = await Edocument.Edocument_clear_temp_upload(parts[0]);
                    temp_result.Add("Compelete");
                }

            }
            catch (Exception e)
            {
                _ = log.edocument("Error Clear documentid" + e.ToString());
                temp_result.Add("Fail");

            }


            return Json(temp_result);
        }


        [Proofpoint]
        public async Task<IActionResult> tiger_require_flow(string flowid)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            var temp_result = new List<object>();
            try
            {
                 temp_result = await Edocument.Edocument_flow_require_document(flowid);

            }
            catch (Exception e)
            {
                _ = log.edocument("Error Clear documentid" + e.ToString());
                temp_result.Add("nodata");

            }


            return Json(temp_result);
        }


        [Proofpoint]
        public async Task<IActionResult> Clear_master_document(string docid, string userid, string formid)
        {
            Logmodel log = new Logmodel();
            Encryption_model encode = new Encryption_model();
            var temp_result = new List<object>();
            try
            {

                if (String.IsNullOrEmpty(docid))
                {
                    temp_result.Add("Fail:no docid");
                }
                else
                {
                    var hash_data = encode.base64_decode(docid);
                    var hash_aes = await encode.encode_aes("decrypt", hash_data);
                    string[] parts = hash_aes.Split('|');
                    var clear_status = await Edocument.Edocument_clear_master(parts[0]);
                    temp_result.Add("Compelete");
                }

            }
            catch (Exception e)
            {
                _ = log.edocument("Error Clear documentid" + e.ToString());
                temp_result.Add("Fail");

            }


            return Json(temp_result);
        }

        [Proofpoint]
        public async Task<IActionResult> Document_flow_management(string flowid)
        {
            Logmodel log = new Logmodel();
            var temp_result = new List<object>();
            try
            {
               
                if (String.IsNullOrEmpty(flowid))
                {

                }
                else {

                    var tiger_data = await Edocument.Edocument_flow_require_document_management(flowid);

                    temp_result.Add(tiger_data);

                }

            }
            catch (Exception e) {

                _= log.edocument("Error Get api Document Flow"+e.ToString());
                temp_result.Add("Fail");

            }


            return Json(temp_result);
        }
        [Proofpoint]
        public async Task<IActionResult> Document_Edocument_management(string param)
        {
            Logmodel log = new Logmodel();
            var temp_result = new List<object>();
            try
            {
                if (String.IsNullOrEmpty(param))
                {
                }
                else
                {
                    var tiger_data = await Edocument.Edocument_flow_require_document_management(param);
                    temp_result.Add(tiger_data);
                }

            }
            catch (Exception e)
            {
                _ = log.edocument("Error Get api Document Flow" + e.ToString());
                temp_result.Add("Fail");
            }


            return Json(temp_result);
        }

        [Proofpoint]
        public async Task<IActionResult> Document_private_management(string userid)
        {
            Logmodel log = new Logmodel();
            var temp_result = new List<object>();
            try
            {
                if (String.IsNullOrEmpty(userid))
                {
                }
                else
                {
                    var tiger_data = await Edocument.Edocument_flow_require_document_management(userid);
                    temp_result.Add(tiger_data);
                }

            }
            catch (Exception e)
            {
                _ = log.edocument("Error Get api Document Flow" + e.ToString());
                temp_result.Add("Fail");
            }


            return Json(temp_result);
        }


        [System.Web.Http.HttpPost]
        public async Task<IActionResult> document(IFormFile fileupload)
        {
            Logmodel log = new Logmodel();
            string base64String = "";
            _= log.upload("Start ======================Tiger Upload=====================");
            if (fileupload == null || fileupload.Length == 0)
               
               return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", fileupload.GetFilename());

            _ = log.upload("Path : "+ path);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fileupload.CopyToAsync(stream);
            }
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }

        
            Encryption_model encode = new Encryption_model();
          
            var hash = await encode.encode_aes("encrypt",base64String);

            Edocumentmng.insertDocument(documentdb, base64String, fileupload.GetFilename(), "Tiger",hash, fileupload.Length.ToString(), "Tiger", "SiteCustomer");

            string response = "Tiger Upload Compelete";

            _ = log.upload("End ======================Tiger Upload=====================");
            return Content(response);
        }


        [System.Web.Http.HttpPost]
        public async Task<IActionResult> Lineupload(IFormFile fileupload)
        {
            Logmodel log = new Logmodel();
            string base64String = "";
            _ = log.upload("Start ======================Tiger Upload=====================");
            if (fileupload == null || fileupload.Length == 0)

                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", fileupload.GetFilename());

            _ = log.upload("Path : " + path);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fileupload.CopyToAsync(stream);
            }
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }

          
            Encryption_model encode = new Encryption_model();
           
            var hash = await encode.encode_aes("encrypt", base64String);

            Edocumentmng.insertDocument(documentdb, base64String, fileupload.GetFilename(), "Lineuser", hash, fileupload.Length.ToString(), "LineUpload", "SiteCustomer");

            string response = "Tiger Upload Compelete";

            _ = log.upload("End ======================Tiger Upload=====================");
            return Content(response);
        }







        [Proofpoint]
        public IActionResult List_temp_upload()
        {
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Upload/temp/");
            List<string> filesList = filePaths.ToList();
            ViewBag.dateC  = DateTime.Now.ToString("yyyyMMddHHmmssss");
            //JsonConvert.SerializeObject(filesList);
            ViewBag.comment = JsonConvert.SerializeObject(filesList);
            return View("~/Views/web001/Tiger/List_temp_upload.cshtml");
        }


        [Proofpoint]
        public IActionResult Clear_temp_upload()
        {
            string result_function = "";
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Upload/temp/");

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                result_function = "Clear temp File Success";

            }
            catch (Exception e)
            {
                result_function = "Clear Temp Fiel Fail " + e.ToString();
            }
            ViewBag.comment = result_function;

            return View("~/Views/web001/Tiger/Clear_temp_upload.cshtml");
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        //Cancel This api
        //public  IActionResult adAuthenTiger()
        //{
        //    string response  =  "<response><status>success</status><userid>1</userid></response>";
        //    return Content(response);
        ///}

        public IActionResult rest_import_documents()
        {
            string response = "<xml><ImportBatchResult><Okays><Okay>"+ DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")+"</Okay></Okays><Errors></Errors></ImportBatchResult></xml>";
            return Content(response);
        }

        public IActionResult rest_begin_batch()
        {
            string response = "<xml><ImportBatchResult><Okays><Okay>" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + "</Okay></Okays><Errors></Errors></ImportBatchResult></xml>";
            return Content(response);
        }

        public IActionResult rest_end_batch()
        {
            string response = "<xml><ImportBatchResult><Okays><Okay>" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + "</Okay></Okays><Errors></Errors></ImportBatchResult></xml>";
            return Content(response);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

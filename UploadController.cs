
using System.Collections.Generic;
using System.Diagnostics;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Drawing;
using iTextSharp.text.pdf;

namespace service.Controllers
{
   
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [Proofpoint]
        public IActionResult Index()
        {
            return View("~/Views/web001/Upload/Index.cshtml");
        }

        [Proofpoint]
        [System.Web.Http.HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

             var path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp",file.GetFilename());
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> updfile(IFormFile file, string flowid, string doc_type, string doc_group, string userid)
        {
            var upload_status = "";
            var result_image = new Dictionary<object, object>();
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Content("File Empty");
                }
                var path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", file.GetFilename());
                var extension = Path.GetExtension(path).ToLowerInvariant();
                var curr_data = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss").ToString();
                var real_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "data", flowid + "_" + curr_data + "_" + "iswan" + extension);
                var temp_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "data", "tmp_" + flowid + "_" + curr_data + "_" + "iswan" + extension);
                var uploadstatus = true;

                var img_org_base64 = "";
                var img_tmp_base64 = "";
                var pdf_org_base64 = "";

                switch (extension)
                {
                    case ".png":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);


                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".PNG":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 150, 80);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".jpg":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".JPG":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".jpeg":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".JPEG":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".bmp":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".BMP":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        img_org_base64 = await image_base64(real_path);
                        img_tmp_base64 = await image_base64_temp(real_path, temp_path, 115, 96);
                       
                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".pdf":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        pdf_org_base64 = await pdf_base64(real_path);


                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    case ".PDF":
                        using (var stream = new FileStream(real_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        pdf_org_base64 = await pdf_base64(real_path);

                        upload_status = "Upload Success :" + Date.date_now();
                        _ = log.info(upload_status);

                        break;
                    default:

                        img_org_base64 = "";
                        img_tmp_base64 = "";

                        upload_status = "Extension Not Support";
                        uploadstatus = false;

                        _ = log.info(upload_status);

                        break;
                }

                if (uploadstatus)
                {


                    result_image.Add("status", "success");
                    // result_image.Add("org",img_org_base64);
                    result_image.Add("temp", img_tmp_base64);

                    return Content(JsonConvert.SerializeObject(result_image));
                }
                else
                {

                    result_image.Add("status", "fail");
                    // result_image.Add("org", "");
                    result_image.Add("temp", "");

                    _ = log.info("Upload Fail : ");

                    return Content(JsonConvert.SerializeObject(result_image));
                }


            }
            catch (Exception e)
            {


                _ = log.info("Error : " + e.ToString());
                result_image.Add("status", "error");
                //result_image.Add("org", "");
                result_image.Add("temp", "");


                return Content("Error");
            }
        }

        //string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        public ActionResult DownloadFile()
        {
            string FilePath = "";
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(FilePath);
                using (MemoryStream inputData = new MemoryStream(bytes))
                {
                    using (MemoryStream outputData = new MemoryStream())
                    {
                        string PDFFilepassword = "123456";
                        PdfReader reader = new PdfReader(inputData);
                        PdfReader.unethicalreading = true;
                        PdfEncryptor.Encrypt(reader, outputData, true, PDFFilepassword, PDFFilepassword, PdfWriter.ALLOW_SCREENREADERS);
                        bytes = outputData.ToArray();
                        //Response.AddHeader("content-length", bytes.Length.ToString());
                        //Response.BinaryWrite(bytes);
                        return File(bytes, "application/pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static async Task<string> pdf_base64(string path)
        {
          
            string base64String = "";
            try
            {
                byte[] pdfBytes = System.IO.File.ReadAllBytes(path);
                base64String = Convert.ToBase64String(pdfBytes);
            }
            catch (Exception e)
            {
                base64String = "";
            }
            return base64String;

        }
        public static async Task<string> image_base64(string path)
        {
            FileInfo f = new FileInfo(path);
            long filesize = f.Length;

            string base64String = "";
            try
            {
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception e)
            {
                base64String = "";
            }
            return base64String;

        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return result;
        }

        public async Task<string> image_base64_temp(string path, string temp_path, int maxWidth, int maxHeight)
        {
            Logmodel log = new Logmodel();
            try
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(path);
                Bitmap newImage = ResizeBitmap(bmp, maxWidth, maxHeight);
                newImage.Save(temp_path);
            }
            catch (Exception e)
            {
                log.info("Error create temp" + e.ToString());
            }

            FileInfo f = new FileInfo(temp_path);
            long filesize = f.Length;

            string base64String = "";

            try
            {
                using (Image image = Image.FromFile(temp_path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception e)
            {
                base64String = "";
            }
            return base64String;

        }



        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");
            var path = Path.Combine( Directory.GetCurrentDirectory() + "/Upload", "temp", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

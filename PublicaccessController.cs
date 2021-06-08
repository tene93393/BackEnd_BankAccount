using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Image = System.Drawing.Image;
using System.Net.Http;
using OfficeOpenXml;
using System.Data.SqlTypes;

namespace service.Controllers
{

    public class PublicaccessController : Controller
    {

        private Appconfig config = new Appconfig();
        Logmodel log = new Logmodel();
 
        public async Task<IActionResult> Index(string Job)
        {

            return View("~/Views/web001/Publicaccess/Index.cshtml");
        }

        [Proofpoint]
        public async Task<FileResult> temp(string assetid)
        {   
            Encryption_model encode = new Encryption_model();
            Logmodel log = new Logmodel();
            try
            {
                var value = HttpContext.Session.GetString("FirstSeen");
                var sess_token = HttpContext.Session.GetString("FirstSeen");

                var task_userid = await Client_info.info_logon(sess_token, "userid");

                _ = log.swan_core_log("edocument", "User request  : ===== > " + task_userid);
                _ = log.edocument("Reuest temp document : ===== > " + assetid);
                var path = "";

                string base64String = "";
                var filename = assetid;
                if (String.IsNullOrEmpty(filename))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                }
                else
                {
                    // =============================
                    //  For Get document 
       
                    _ = log.swan_core_log("edocument", "Reuest temp document form stroage  : ===== > " + assetid);

                    var hash_data = encode.base64_decode(assetid);
                    var hash_aes = await encode.encode_aes("decrypt", hash_data);

                    string[] parts = hash_aes.Split('|');

                    _ = log.swan_core_log("edocument", "Reuest temp document form stroage docid  : ===== > " + parts[0].ToString());
                    


                    // =============================
                    var document_data = await Edocument_model.document_temp(parts[0].ToString());
                    //path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", document_data["filename"].ToString());


                    string root = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", document_data["filename"].ToString());

                    _ = log.swan_core_log("edocument", "Document Path stroage  : ===== > " + root);



                    System.Drawing.Image bitmapa = (System.Drawing.Image)Bitmap.FromFile(root); // set image     
                    //Font font = new Font("Arial", 20, FontStyle.Italic, GraphicsUnit.Pixel);
                    Font font = new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel);
                    //Color color = Color.FromArgb(246, 246, 0, 0); // (255,255,255
                    Color color = Color.FromArgb(255, 255, 255, 255); // (255,255,255)
                    //Color color = Color.FromArgb(0, 246, 246,246); // (255,255,255)
                  
                    SolidBrush brush = new SolidBrush(color);
                    Graphics graphics = Graphics.FromImage(bitmapa);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;


                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.RotateAt(45, new System.Drawing.PointF(bitmapa.Width / 2, bitmapa.Height / 2));
                    graphics.Transform = matrix;


                    //graphics.DrawString("SWAN SBB", font, brush, atpoint, sf);

                    Point atpoint1 = new Point(bitmapa.Width / 4, bitmapa.Height / 4);
                    Point atpoint2 = new Point(bitmapa.Width / 3, bitmapa.Height / 3);
                    Point atpoint3 = new Point(bitmapa.Width / 2, bitmapa.Height / 2);

                    graphics.DrawString(task_userid, font, brush, atpoint1, sf);
                    graphics.DrawString(task_userid, font, brush, atpoint2, sf);
                    graphics.DrawString(task_userid, font, brush, atpoint3, sf);

                    _ = log.swan_core_log("edocument", "Watermark Text  : ===== > " + task_userid);

                    graphics.Dispose();
                    MemoryStream m = new MemoryStream();
                    bitmapa.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] convertedToBytes = m.ToArray();
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "ziped_"+document_data["filename"].ToString());

                    _ = log.swan_core_log("edocument", "Watermark Document Path  : ===== > " + path);

                    System.IO.File.WriteAllBytes(path, convertedToBytes);


                }

                _ = log.swan_core_log("edocument", "Convert Bitmap  : ===== > " + path);

                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

           

                _ = log.swan_core_log("edocument", "return default document stroage  : ===== > " + assetid );
              


                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
            }
            catch (Exception e)
            {
                _ = log.swan_core_log("edocument","Asset id : "+ assetid + " Error "+e.ToString());
                var default_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(default_path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

           
                _ = log.swan_core_log("edocument", "return default document Image  : ===== > " + assetid);


                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
               
            }

        }


        [Proofpoint]
        public async Task<FileResult> temp_list(string requestid)
        {
            Logmodel log = new Logmodel();
            try
            {
                _ = log.edocument("Reuest temp_list document : ===== > " + requestid);
                var path = "";

                string base64String = "";
                var filename = requestid;
                if (String.IsNullOrEmpty(filename))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                }
                else
                {
                    // =============================
                    //  For Get document 






                    // =============================
                    var document_data = await Edocument_model.document_temp("104");
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", document_data["filename"].ToString());
                }

                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }


                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
            }
            catch (Exception e)
            {

                var default_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(default_path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
                _ = log.edocument("Reuest temp_list Error document : ===== > " + e.ToString());
            }

        }

        [Proofpoint]
        public async Task<FileResult> master(string assetid)
        {
            Logmodel log = new Logmodel();
            try
            {
                _ = log.edocument("Reuest master document : ===== > " + assetid);
                var path = "";

                string base64String = "";
                var filename = assetid;
                if (String.IsNullOrEmpty(filename))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                }
                else
                {
                    // =============================
                    //  For Get document 






                    // =============================
                    var document_data = await Edocument_model.document_temp("104");
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", document_data["filename"].ToString());
                }

                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }


                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
            }
            catch (Exception e)
            {

                var default_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(default_path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
                _ = log.edocument("Reuest master Error document : ===== > " + e.ToString());
            }

        }

        [Proofpoint]
        public async Task<FileResult> master_list(string requestid)
        {
            Logmodel log = new Logmodel();
            try
            {
                _ = log.edocument("Reuest master_list document : ===== > " + requestid);
                var path = "";

                string base64String = "";
                var filename = requestid;
                if (String.IsNullOrEmpty(filename))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                }
                else
                {
                    // =============================
                    //  For Get document 






                    // =============================
                    var document_data = await Edocument_model.document_temp("104");
                    path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", document_data["filename"].ToString());
                }

                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }


                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
            }
            catch (Exception e)
            {

                var default_path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", "index.jpg");
                Bitmap bitmap;
                using (Stream bmpStream = System.IO.File.Open(default_path, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

                return new FileContentResult(ImageToByteArray(bitmap), "image/jpeg");
                _ = log.edocument("Reuest master_list Error document : ===== > " + e.ToString());
            }

        }




        [HttpGet]
        public async Task<IActionResult> Get(string request)
        {
            var filename = request;
            if (filename == null)
                return Content("filename not present");
            var path = Path.Combine(Directory.GetCurrentDirectory() + "/Upload", "temp", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(path), Path.GetFileName(path));
          
        }

        private static byte[] ImageToByteArray(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
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


        public async Task<IActionResult> Activate(string Jobid)
        {

            return View("~/Views/web001/Publicaccess/Index.cshtml");
        }



        public async  Task<IActionResult> Error_page(string statuscode) {
           
            return  View("~/Views/web001/Publicaccess/"+ statuscode + ".cshtml");
        }

        public void AddWatermark(string watermarkText, string image, string TColor)
        {

            System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile("load form file");
            Font font = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);

            RectangleF rectf = new RectangleF(70, 90, 90, 50);

            Color color = Color.FromArgb(255, 255, 0, 0);
            try
            {
                if (TColor.ToUpper() == "RED")
                {
                    color = Color.FromArgb(255, 255, 0, 0);
                }
                else if (TColor.ToUpper() == "WHITE")
                {
                    color = Color.FromArgb(255, 255, 255, 255);
                }
                else if (TColor.ToUpper() == "BLACK")
                {
                    color = Color.FromArgb(155, 0, 0, 0);
                }
                else if (TColor.ToUpper() == "GREEN")
                {
                    color = Color.FromArgb(255, 0, 255, 0);
                }
            }
            catch { }


            //     WHITE (255, 255, 255, 255)
            //     RED   (255, 255, 000, 000)
            //     GREEN (255, 000, 255, 000)
            //     BLUE  (255, 000, 000, 255) 
            //     BLACK (150, 000, 000, 000)
            //     PURPLE(255, 125, 000, 255)
            //     GREY  (255, 128, 128, 128) 
            //     YELLOW(255, 255, 255, 000) 
            //     ORANGE(255, 255, 125, 000)

            //   Point atpoint = new Point(bitmap.Width / 2, bitmap.Height / 2);
            Point atpoint = new Point(bitmap.Width / 2, bitmap.Height - 10);

            SolidBrush brush = new SolidBrush(color);
            Graphics graphics = Graphics.FromImage(bitmap);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            graphics.DrawString(watermarkText, font, brush, atpoint, sf);
            graphics.Dispose();
            MemoryStream m = new MemoryStream();
            bitmap.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);

            //   Response.WriteFile("images/DefaultLogo.png", true);
           // return

          //  m.WriteTo(Response.OutputStream);

            m.Dispose();
            base.Dispose();
        }








        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

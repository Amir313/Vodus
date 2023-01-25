using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Vodus.Web.Models;

namespace Vodus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GenerateJsonFile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateJsonFile(VodusFileInfo file)
        {
            List<ImageDetailModel> imageDetailObj= new List<ImageDetailModel>();

            if (!string.IsNullOrEmpty(file.fileUrl))
            {
                if (Path.GetExtension(file.fileUrl).Equals(".xlsx"))
                {
                    var fileName = Path.GetFileName(file.fileUrl);
                    var destFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ExcelFile", fileName);
                    if(System.IO.File.Exists(destFilePath))
                    {
                        System.IO.File.Delete(destFilePath);
                    }

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(file.fileUrl, destFilePath);
                    }

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = System.IO.File.Open(destFilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            foreach (DataTable table in result.Tables)
                            {
                                table.Rows.RemoveAt(0);

                                foreach (DataRow dr in table.Rows)
                                {
                                    
                                        imageDetailObj.Add(new ImageDetailModel
                                        {
                                            page = dr["Column0"].ToString(),
                                            promoTitle = dr["Column1"].ToString(),
                                            promoDescription = dr["Column2"].ToString(),
                                            TandC = dr["Column3"].ToString(),
                                            startDate = dr["Column4"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(dr["Column4"].ToString()),
                                            endDate = dr["Column5"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(dr["Column5"].ToString()),
                                            imageUrl = dr["Column6"].ToString()
                                        });
                                    
                                }
                            }

                        }
                    }

                    var destJsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\JsonFile", fileName.Replace("xlsx","json"));
                    if (System.IO.File.Exists(destJsonFilePath))
                    {
                        System.IO.File.Delete(destJsonFilePath);
                    }

                    Utils.WriteJsonFile(imageDetailObj, destJsonFilePath);
                    TempData["SuccessMsg"] = "File Generated Successfully.";
                    TempData["JsonFilePath"] = destJsonFilePath;
                }
                else
                {
                    TempData["ErrorMsg"] = "Only Excel file is Allowed.";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Url Cannot be Empty.";
            }
            
            return View();
        }

        public IActionResult DownloadFile()
        {
            var memory = Utils.DownloadFile("vodus-test.json", @"wwwroot\JsonFile");
            return File(memory.ToArray(), "text/plain", "vodus-test.json");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult LoadJsonFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\JsonFile", "vodus-test.json");
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMsg"] = "File Not Exists.";
            }
            else
            {
                List<ImageDetailModel> imageDetailObj = Utils.ReadJsonFile(filePath);
                ViewBag.ExcelData = imageDetailObj;
            }
            return View();
        }

        [HttpPost]
        public IActionResult LoadJsonFile(VodusSearchInfo SearchInfoObj)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\JsonFile", "vodus-test.json");
            List<ImageDetailModel> imageDetailObj = null;
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMsg"] = "File Not Exists.";
            }
            else if (SearchInfoObj.startDate > SearchInfoObj.endDate)
            {
                TempData["ErrorMsg"] = "End Date Must be Greater than Start Date.";
                ViewBag.ExcelData = null;
            }
            else
            {
                imageDetailObj = Utils.ReadJsonFile(filePath);
                var finalList = imageDetailObj.Where(m => m.startDate >= SearchInfoObj.startDate && m.endDate <= SearchInfoObj.endDate).ToList();
                ViewBag.ExcelData = finalList;
            }
            return View();
        }

        public IActionResult CustomGrid()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\JsonFile", "vodus-test.json");
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMsg"] = "File Not Exists.";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

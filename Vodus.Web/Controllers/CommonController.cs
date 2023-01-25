using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vodus.Web.Models;

namespace Vodus.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpPost("GetData")]
        public IActionResult GetData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\JsonFile", "vodus-test.json");
                var vodusData = (from tempuser in Utils.ReadJsonFile(filePath) select tempuser);

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection.Equals("desc"))
                    {
                        switch(sortColumn)
                        {
                            case "page":
                                vodusData = vodusData.OrderByDescending(s => s.page).ToList();
                                break;
                            case "promoTitle":
                                vodusData = vodusData.OrderByDescending(s => s.promoTitle).ToList();
                                break;
                            case "promoDescription":
                                vodusData = vodusData.OrderByDescending(s => s.promoDescription).ToList();
                                break;
                            case "startDate":
                                vodusData = vodusData.OrderByDescending(s => s.startDate).ToList();
                                break;
                            case "endDate":
                                vodusData = vodusData.OrderByDescending(s => s.endDate).ToList();
                                break;
                            case "imageUrl":
                                vodusData = vodusData.OrderByDescending(s => s.imageUrl).ToList();
                                break;
                            default:
                                vodusData = vodusData.OrderByDescending(s => s.endDate).ToList();
                                break;
                        }
                        
                    }
                    else if (sortColumnDirection.Equals("asc"))
                    {
                        switch (sortColumn)
                        {
                            case "page":
                                vodusData = vodusData.OrderBy(s => s.page).ToList();
                                break;
                            case "promoTitle":
                                vodusData = vodusData.OrderBy(s => s.promoTitle).ToList();
                                break;
                            case "promoDescription":
                                vodusData = vodusData.OrderBy(s => s.promoDescription).ToList();
                                break;
                            case "startDate":
                                vodusData = vodusData.OrderBy(s => s.startDate).ToList();
                                break;
                            case "endDate":
                                vodusData = vodusData.OrderBy(s => s.endDate).ToList();
                                break;
                            case "imageUrl":
                                vodusData = vodusData.OrderBy(s => s.imageUrl).ToList();
                                break;
                            default:
                                vodusData = vodusData.OrderBy(s => s.endDate).ToList();
                                break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    vodusData = vodusData.Where(m => m.page.Contains(searchValue)
                                                || m.promoTitle.Contains(searchValue)
                                                || m.promoDescription.Contains(searchValue)
                                                || m.startDate.ToString().Contains(searchValue)
                                                || m.endDate.ToString().Contains(searchValue)
                                                || m.imageUrl.Contains(searchValue));
                }
                recordsTotal = vodusData.Count();
                var data = vodusData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

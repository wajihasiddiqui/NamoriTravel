using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using NamoriTravel.Common;
using Newtonsoft.Json;
using ServiceLayer;
using System.Xml;
using ModelsDTO;
using Microsoft.AspNetCore.Http;
using NamoriTravel.Models;
using Microsoft.EntityFrameworkCore;

namespace NamoriTravel.Controllers
{
    [AllowAnonymous]
    [Route("NamoriTravels")]
    public class NamoriTravelsController : BaseController
    {
        private readonly IServiceManager _serviceManager;
        public NamoriTravelsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.TblTitle = "Namori Travel";
            return View();
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<JsonResult> GetCountries()
        {
            var getCountries = await _serviceManager.countryService.GetAllAsync(0);
            try
            {
                return Json(new
                {
                    Data = getCountries.Select(x => new { x.Code, x.Name })//  City = getCities.Result
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all GetLocations (API)", 0);
                return Json(new
                {
                    Data = ""//  City = getCities.Result
                });
            }
        }
        [HttpGet]
        [Route("GetCities")]
        public async Task<JsonResult> GetCities(string Code)
        {
            var getCities = await _serviceManager.cityService.GetAllByCountryCodeAsync(0, Code);
            try
            {
                return Json(new
                {
                    Data = getCities.Select(x => new { x.Code, x.Name })//  City = getCities.Result
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all GetLocations (API)", 0);
                return Json(new
                {
                    Data = ""//  City = getCities.Result
                });
            }
        }

        [HttpGet]
        [Route("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            var getCountries = await _serviceManager.countryService.GetAllAsync(0);
            try
            {
                return Json(new
                {
                    Country = getCountries.Select(x => new { x.Code, x.Name })//  City = getCities.Result
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all GetLocations (API)", 0);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("SearchHotel")]
        public async Task<IActionResult> SearchHotel(string DTO)
        {
            try
            {
                var redirectUrl = Url.Action("HotelDetails", "NamoriTravels", new { DTO = DTO });
                return Json(new { redirectUrl });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all GetLocations (API)", 0);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("HotelDetails")]
        public async Task<IActionResult> HotelDetails(string DTO)
        {
            try
            {
                SearchParam searchParam = new SearchParam();
                if (DTO.Contains("\\n"))
                {
                    var processeddata = DTO.Replace("\\n", "-")
                                     .Replace(" ", "")
                                     .Split(",");
                    searchParam = JsonConvert.DeserializeObject<SearchParam>(string.Join(",", processeddata));
                    searchParam.FromDate = Common.Common.FormatDate(searchParam.FromDate);
                    searchParam.ToDate = Common.Common.FormatDate(searchParam.ToDate);
                }
                else
                {
                    searchParam = JsonConvert.DeserializeObject<SearchParam>(DTO);

                }
                string responseXml = "";
                if (!string.IsNullOrEmpty(DTO))
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("fromDate", searchParam.FromDate);
                    dic.Add("toDate", searchParam.ToDate);
                    dic.Add("currency", searchParam.Currency);
                    dic.Add("cityCode", searchParam.City);
                    dic.Add("rateBasis", searchParam.Ratebasis);
                    dic.Add("childrenNo", searchParam.Children);
                    dic.Add("adultsCode", searchParam.Adults);
                    dic.Add("roomsNo", searchParam.Rooms);
                    dic.Add("resultsPerPage", searchParam.PageSize);
                    dic.Add("page", searchParam.Page);
                    string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "searchhotels", dic, 0);
                    responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, 0, AppSettings.DotWConnect_Url);
                }
                //string fileContent;
                //try
                //{
                //    string filePath = @"D:\Response.txt"; // Update with your actual path
                //    if (System.IO.File.Exists(filePath))
                //    {
                //        fileContent = System.IO.File.ReadAllText(filePath);
                //    }
                //    else
                //    {
                //        fileContent = "File not found.";
                //    }
                //}
                //catch (Exception ex)
                //{
                //    fileContent = $"An error occurred: {ex.Message}";
                //}
                //var Response = XmlResponseTo_Obj.XmlResponseToObj(fileContent);
                 var Response = XmlResponseTo_Obj.XmlResponseToObj(responseXml);
                HttpContext.Session.SetString("HotelResponseData", JsonConvert.SerializeObject(Response));
                var Amenitieslist = await _serviceManager.amenitiesService.GetAllAsync(0);
                var RateBasisList = await _serviceManager.rateBasisServices.GetAllAsync(0);
                Response.RateBasis = RateBasisList.ToList();
                Response.Amenitiesdto = Amenitieslist.ToList();
                Response.TotalPages = Response.Count;
                Response.PageSize = 10;
                Response.CurrentPage = Convert.ToInt32(searchParam.Page);
                Response.searchParam = searchParam;
                return View(Response);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all HotelDetails (API)", 0);
                return BadRequest(ex.Message);
            }
        }
            
        [HttpGet]
        [Route("GetRooms")]
        public async Task<IActionResult> GetRooms(string HotelId, int page = 1, int pageSize = 10)
        {

            string fileContent;
            try
            {
                HotelDetail hotelDetail = new HotelDetail();
                var HotelData = JsonConvert.DeserializeObject<HotelResponse>(HttpContext.Session.GetString("HotelResponseData"));
                var hotel = HotelData.Hotels.FirstOrDefault(x => x.HotelId == HotelId);
                if (hotel == null)
                {
                    // Handle the case where the hotel is not found.
                    throw new Exception($"Hotel with ID {HotelId} not found.");
                }
                else
                {
                    hotelDetail = new HotelDetail()
                    {
                        Ip = HotelData.Ip,
                        Hotel = hotel,
                        ElapsedTime = HotelData.ElapsedTime,
                        Command = HotelData.Command,
                        CurrencyShort = HotelData.CurrencyShort,
                        Date = HotelData.Date,
                        TID = HotelData.TID,
                        Version = HotelData.Version
                    };
                    hotelDetail.Hotel.Description = hotelDetail.Hotel.Description1 != null ? string.Join("<br />", hotelDetail.Hotel.Description1.Split("<br />").Take(3)) : "";
                    hotelDetail.Hotel.Description2 = string.Join("<br />", hotelDetail.Hotel.Description1);
                    if (hotelDetail.Hotel.Amenities is not null && hotelDetail.Hotel.Amenities.Count > 5)
                    {
                        hotelDetail.Hotel.Amenities2 = hotelDetail.Hotel.Amenities.ToList();
                    }

                    if (hotel.Rooms != null && hotel.Rooms.Any())
                    {
                        var totalRecords = hotel.Rooms.Count();
                        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                        var hotels = hotel.Rooms
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        hotelDetail.Hotel.Rooms = hotels;    
                        hotelDetail.PageSize = pageSize;     
                        hotelDetail.CurrentPage = page;      
                        hotelDetail.TotalPages = totalPages; 
                    }
                }

                return View(hotelDetail);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all HotelDetails (API)", 0);
                return BadRequest(ex.Message);

            }
        }

    }
    
}

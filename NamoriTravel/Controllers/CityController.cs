using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using NamoriTravel.Models;
using NamoriTravel.Common;
using Newtonsoft.Json;
using ServiceLayer;
using System.Xml;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
   
    [CustomAuthorize("City", "Visible")]
    public class CityController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public CityController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [CustomAuthorize("City", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "City";
          
            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
          [CustomAuthorize("City", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.cityService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = Result.DTO.Count(),
                    iTotalDisplayRecords = Result.Total,
                    aaData = Result.DTO
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all citys (API)", UserId.Value);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new List<object>()
                });
            }
        }


        [HttpGet]
        [CustomAuthorize("City", "Read")]
        public async Task<IActionResult> GetLiveCities(string countryCode = "0")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["countryCode"] = countryCode;
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getallcities", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var cityList = new List<CityDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNodeList cities = doc.SelectNodes("//city");

                foreach (XmlNode city in cities)
                {
                    // Extracting the values from the XML
                    int id = Convert.ToInt32(city.Attributes["runno"].Value);
                    string name = city.SelectSingleNode("name").InnerText;
                    string code = city.SelectSingleNode("code").InnerText;
                    string CountryName = city.SelectSingleNode("countryName").InnerText;
                    string CountryCode = city.SelectSingleNode("countryCode").InnerText;

                    cityList.Add(new CityDTO
                    {
                        Id = id,
                        Name = name,
                        Code = code,
                        CountryCode = CountryCode,
                        IsActive = true
                    });
                }
               
                return Json(new { Data = cityList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all citys (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
          [CustomAuthorize("City", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var city = await _serviceManager.cityService.GetByIdAsync(id, UserId.Value);
                if (city == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    city = new CityDTO();

                return Json(new { Data = city });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching city {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
          [CustomAuthorize("City", "Create")]
        public async Task<IActionResult> ApiAdd(CityDTO DTO, string DTOlist)
        {
            try
            {
                List<CityDTO> cities = new List<CityDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    cities = JsonConvert.DeserializeObject<List<CityDTO>>(DTOlist); 

                await _serviceManager.cityService.AddAsync(DTO, cities, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "cityController", "ApiAdd", $"API city {DTO.Name} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating city (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
          [CustomAuthorize("City", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, CityDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<CityDTO> cityDTOs = new List<CityDTO>();
                await _serviceManager.cityService.UpdateAsync(DTO, cityDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "cityController", "ApiUpdate", $"API city {DTO.Name} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating city {DTO.Name} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
          [CustomAuthorize("City", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var city = await _serviceManager.cityService.GetByIdAsync(id, UserId.Value);
                if (city == null)
                {
                    return NotFound();
                }
                List<CityDTO> cityDTOs = new List<CityDTO>();
                await _serviceManager.cityService.DeleteAsync(city, cityDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "cityController", "ApiDelete", $"API city {city.Name} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting city {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

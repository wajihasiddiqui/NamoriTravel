using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using NamoriTravel.Common;
using NamoriTravel.Models;
using Newtonsoft.Json;
using ServiceLayer;
using System.Xml;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{

    [CustomAuthorize("Country", "Visible")]
    public class CountryController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public CountryController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [CustomAuthorize("Country", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "Country";
            
            return View();
        }
        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Country", "Read")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var Result = await _serviceManager.countryService.GetAllAsync(UserId.Value);
                return Json(new
                {
                    Data = Result.Select(x => new { x.Code, x.Name })
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all countries (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        [CustomAuthorize("Country", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.countryService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords =  Result.DTO.Count() ,
                    iTotalDisplayRecords =  Result.Total,
                    aaData = Result.DTO
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all countrys (API)", UserId.Value);
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
        [CustomAuthorize("Country", "Read")]
        public async Task<IActionResult> GetLiveCountries()
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
              
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getallcountries", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var countryList = new List<CountryDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);

                // Get the list of countries
                XmlNodeList countriess = doc.SelectNodes("//country");

                foreach (XmlNode country in countriess)
                {
                    int id = Convert.ToInt32(country.Attributes["runno"].Value);
                    string name = country.SelectSingleNode("name").InnerText;
                    string code = country.SelectSingleNode("code").InnerText;
                    countryList.Add(new CountryDTO
                    {
                        Id = id,
                        Name = name,
                        Code = code,
                        Currency = "",
                        IsActive = true
                    });
                }
                return Json(new { Data = countryList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all countrys (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("Country", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var country = await _serviceManager.countryService.GetByIdAsync(id, UserId.Value);
                if (country == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    country = new CountryDTO();

                return Json(new { Data = country });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching country {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Country", "Create")]
        public async Task<IActionResult> ApiAdd(CountryDTO DTO , string DTOlist)
        {
            try
            {
                List<CountryDTO> countries = new List<CountryDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    countries = JsonConvert.DeserializeObject<List<CountryDTO>>(DTOlist); // Deserialize the JSON string

                await _serviceManager.countryService.AddAsync(DTO, countries, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "countryController", "ApiAdd", $"API country {DTO.Name} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating country (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Country", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, CountryDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<CountryDTO> countryDTOs = new List<CountryDTO>();
                await _serviceManager.countryService.UpdateAsync(DTO, countryDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "countryController", "ApiUpdate", $"API country {DTO.Name} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating country {DTO.Name} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Country", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var country = await _serviceManager.countryService.GetByIdAsync(id, UserId.Value);
                if (country == null)
                {
                    return NotFound();
                }
                List<CountryDTO> countryDTOs = new List<CountryDTO>();
                await _serviceManager.countryService.DeleteAsync(country, countryDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "countryController", "ApiDelete", $"API country {country.Name} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting country {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using NamoriTravel.Models;
using NamoriTravel.Common;
using Newtonsoft.Json;
using ServiceLayer;
using AutoMapper;
using System.Xml;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Amenities", "Visible")]
    public class AmenitiesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public AmenitiesController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [CustomAuthorize("Amenities", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "Amenities";

            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Amenities", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.amenitiesService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Amenitiess (API)", UserId.Value);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new List<AmenitiesDTO>()
                });
            }
        }


        [HttpGet]
        [CustomAuthorize("Amenities", "Read")]
        public async Task<IActionResult> GetLiveAmenities(string countryCode = "0")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getamenitieids", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var AmenitiesList = new List<AmenitiesDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNodeList options = doc.SelectNodes("//amenities/option");


                foreach (XmlNode option in options)
                {
                    int value = Convert.ToInt32(option.Attributes["value"].Value);
                    string description = option.InnerText;

                    AmenitiesList.Add(new AmenitiesDTO
                    {
                        Value = value,
                        Description = description
                    });
                }

                return Json(new { Data = AmenitiesList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Amenitiess (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("Amenities", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var Amenities = await _serviceManager.amenitiesService.GetByIdAsync(id, UserId.Value);
                if (Amenities == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    Amenities = new AmenitiesDTO();

                return Json(new { Data = Amenities });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching Amenities {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Amenities", "Create")]
        public async Task<IActionResult> ApiAdd(AmenitiesDTO DTO, string DTOlist)
        {
            try
            {
                List<AmenitiesDTO> cities = new List<AmenitiesDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    cities = JsonConvert.DeserializeObject<List<AmenitiesDTO>>(DTOlist);

                await _serviceManager.amenitiesService.AddAsync(DTO, cities, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "AmenitiesController", "ApiAdd", $"API Amenities {DTO.Description} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating Amenities (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Amenities", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, AmenitiesDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<AmenitiesDTO> AmenitiesDTOs = new List<AmenitiesDTO>();
                await _serviceManager.amenitiesService.UpdateAsync(DTO, AmenitiesDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "AmenitiesController", "ApiUpdate", $"API Amenities {DTO.Description} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating Amenities {DTO.Description} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Amenities", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var Amenities = await _serviceManager.amenitiesService.GetByIdAsync(id, UserId.Value);
                if (Amenities == null)
                {
                    return NotFound();
                }
                List<AmenitiesDTO> AmenitiesDTOs = new List<AmenitiesDTO>();
                await _serviceManager.amenitiesService.DeleteAsync(Amenities, AmenitiesDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "AmenitiesController", "ApiDelete", $"API Amenities {Amenities.Description} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting Amenities {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

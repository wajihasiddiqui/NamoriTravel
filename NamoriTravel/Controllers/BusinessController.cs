using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using NamoriTravel.Common;
using NamoriTravel.Models;
using Newtonsoft.Json;
using ServiceLayer;
using AutoMapper;
using System.Xml;
using ModelsDTO;

namespace NamoriTravel.Controllers
{

    [CustomAuthorize("Business", "Visible")]
    public class BusinessController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public BusinessController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [CustomAuthorize("Business", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "Business";

            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Business", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.businessServices.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Businesss (API)", UserId.Value);
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
        [CustomAuthorize("Business", "Read")]
        public async Task<IActionResult> GetLiveBusiness(string countryCode = "0")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getbusinessids", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var BusinessList = new List<BusinessDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNodeList options = doc.SelectNodes("//business/option");


                foreach (XmlNode option in options)
                {
                    int value = Convert.ToInt32(option.Attributes["value"].Value);
                    string description = option.InnerText;

                    BusinessList.Add(new BusinessDTO
                    {
                        Value = value,
                        Description = description
                    });
                }

                return Json(new { Data = BusinessList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Businesss (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("Business", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var Business = await _serviceManager.businessServices.GetByIdAsync(id, UserId.Value);
                if (Business == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    Business = new BusinessDTO();

                return Json(new { Data = Business });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching Business {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Business", "Create")]
        public async Task<IActionResult> ApiAdd(BusinessDTO DTO, string DTOlist)
        {
            try
            {
                List<BusinessDTO> cities = new List<BusinessDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    cities = JsonConvert.DeserializeObject<List<BusinessDTO>>(DTOlist);

                await _serviceManager.businessServices.AddAsync(DTO, cities, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "BusinessController", "ApiAdd", $"API Business {DTO.Description} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating Business (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Business", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, BusinessDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<BusinessDTO> BusinessDTOs = new List<BusinessDTO>();
                await _serviceManager.businessServices.UpdateAsync(DTO, BusinessDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "BusinessController", "ApiUpdate", $"API Business {DTO.Description} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating Business {DTO.Description} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Business", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var Business = await _serviceManager.businessServices.GetByIdAsync(id, UserId.Value);
                if (Business == null)
                {
                    return NotFound();
                }
                List<BusinessDTO> BusinessDTOs = new List<BusinessDTO>();
                await _serviceManager.businessServices.DeleteAsync(Business, BusinessDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "BusinessController", "ApiDelete", $"API Business {Business.Description} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting Business {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

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

    [CustomAuthorize("RateBasis", "Visible")]
    public class RateBasisController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public RateBasisController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [CustomAuthorize("RateBasis", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "RateBasis";

            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("RateBasis", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.rateBasisServices.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all RateBasiss (API)", UserId.Value);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new List<RateBasisDTO>()
                });
            }
        }


        [HttpGet]
        [CustomAuthorize("RateBasis", "Read")]
        public async Task<IActionResult> GetLiveRateBasis(string countryCode = "0")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getratebasisids", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var RateBasisList = new List<RateBasisDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNodeList options = doc.SelectNodes("//ratebasis/option");


                foreach (XmlNode option in options)
                {
                    int value = Convert.ToInt32(option.Attributes["value"].Value);
                    string description = option.InnerText;

                    RateBasisList.Add(new RateBasisDTO
                    {
                        Value = value,
                        Description = description
                    });
                }

                return Json(new { Data = RateBasisList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all RateBasiss (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("RateBasis", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var RateBasis = await _serviceManager.rateBasisServices.GetByIdAsync(id, UserId.Value);
                if (RateBasis == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    RateBasis = new RateBasisDTO();

                return Json(new { Data = RateBasis });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching RateBasis {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("RateBasis", "Create")]
        public async Task<IActionResult> ApiAdd(RateBasisDTO DTO, string DTOlist)
        {
            try
            {
                List<RateBasisDTO> cities = new List<RateBasisDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    cities = JsonConvert.DeserializeObject<List<RateBasisDTO>>(DTOlist);

                await _serviceManager.rateBasisServices.AddAsync(DTO, cities, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RateBasisController", "ApiAdd", $"API RateBasis {DTO.Description} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating RateBasis (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("RateBasis", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, RateBasisDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<RateBasisDTO> RateBasisDTOs = new List<RateBasisDTO>();
                await _serviceManager.rateBasisServices.UpdateAsync(DTO, RateBasisDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RateBasisController", "ApiUpdate", $"API RateBasis {DTO.Description} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating RateBasis {DTO.Description} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("RateBasis", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var RateBasis = await _serviceManager.rateBasisServices.GetByIdAsync(id, UserId.Value);
                if (RateBasis == null)
                {
                    return NotFound();
                }
                List<RateBasisDTO> RateBasisDTOs = new List<RateBasisDTO>();
                await _serviceManager.rateBasisServices.DeleteAsync(RateBasis, RateBasisDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RateBasisController", "ApiDelete", $"API RateBasis {RateBasis.Description} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting RateBasis {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

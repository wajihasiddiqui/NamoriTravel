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
    [CustomAuthorize("Currency", "Visible")]
    public class CurrencyController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public CurrencyController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [CustomAuthorize("Currency", "Read")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TblTitle = "Currency";

            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Currency", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.currencyServices.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Currencys (API)", UserId.Value);
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
        [CustomAuthorize("Currency", "Read")]
        public async Task<IActionResult> GetLiveCurrency(string countryCode = "0")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string requestXml = await _serviceManager.xmlRequestService.GenerateRequestXmlAsync("UserDetails", "getcurrenciesids", dic, UserId.Value);
                string responseXml = await _serviceManager.xmlRequestService.SendDotWConnectRequestAsync(requestXml, UserId.Value, AppSettings.DotWConnect_Url);
                var CurrencyList = new List<CurrencyDTO>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNodeList options = doc.SelectNodes("//currency/option");


                foreach (XmlNode option in options)
                {
                    int value = Convert.ToInt32(option.Attributes["value"].Value);
                    string shortcut = Convert.ToString(option.Attributes["shortcut"].Value);
                    string description = option.InnerText;

                    CurrencyList.Add(new CurrencyDTO
                    {
                        Value = value,
                        Shortcut = shortcut,
                        Description = description
                    });
                }

                return Json(new { Data = CurrencyList });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all Currencys (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("Currency", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var Currency = await _serviceManager.currencyServices.GetByIdAsync(id, UserId.Value);
                if (Currency == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    Currency = new CurrencyDTO();

                return Json(new { Data = Currency });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching Currency {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Currency", "Create")]
        public async Task<IActionResult> ApiAdd(CurrencyDTO DTO, string DTOlist)
        {
            try
            {
                List<CurrencyDTO> cities = new List<CurrencyDTO>();
                if (!string.IsNullOrEmpty(DTOlist))
                    cities = JsonConvert.DeserializeObject<List<CurrencyDTO>>(DTOlist);

                await _serviceManager.currencyServices.AddAsync(DTO, cities, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "CurrencyController", "ApiAdd", $"API Currency {DTO.Description} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating Currency (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Currency", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, CurrencyDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                List<CurrencyDTO> CurrencyDTOs = new List<CurrencyDTO>();
                await _serviceManager.currencyServices.UpdateAsync(DTO, CurrencyDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "CurrencyController", "ApiUpdate", $"API Currency {DTO.Description} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating Currency {DTO.Description} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Currency", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var Currency = await _serviceManager.currencyServices.GetByIdAsync(id, UserId.Value);
                if (Currency == null)
                {
                    return NotFound();
                }
                List<CurrencyDTO> CurrencyDTOs = new List<CurrencyDTO>();
                await _serviceManager.currencyServices.DeleteAsync(Currency, CurrencyDTOs, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "CurrencyController", "ApiDelete", $"API Currency {Currency.Description} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting Currency {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModelsDTO;
using NamoriTravel.Authorize;
using NamoriTravel.Models;
using ServiceLayer;

namespace NamoriTravel.Controllers
{
   
    [CustomAuthorize("XmlRequest", "Visible")]
    public class XmlRequestController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public XmlRequestController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//
        [HttpGet]
        [CustomAuthorize("XmlRequest", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "XmlRequest List";
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching XmlRequest", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("XmlRequest", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);
                if (XmlRequest == null)
                {
                    return NotFound();
                }
                return View(XmlRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for XmlRequest {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("XmlRequest", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("XmlRequest", "Create")]
        public async Task<IActionResult> Create(XmlRequestDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _serviceManager.xmlRequestService.AddAsync(model, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "Create", $"XmlRequest {model.RequestType} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating XmlRequest", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("XmlRequest", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);
                if (XmlRequest == null)
                {
                    return NotFound();
                }
                return View(XmlRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching XmlRequest {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("XmlRequest", "Update")]
        public async Task<IActionResult> Edit(int id, XmlRequestDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                await _serviceManager.xmlRequestService.UpdateAsync(model, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "Edit", $"XmlRequest {model.RequestType} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating XmlRequest {model.RequestType}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("XmlRequest", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);
                if (XmlRequest == null)
                {
                    return NotFound();
                }
                return View(XmlRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching XmlRequest {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("XmlRequest", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);
                if (XmlRequest == null)
                {
                    return NotFound();
                }

                await _serviceManager.xmlRequestService.DeleteAsync(XmlRequest, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "DeleteConfirmed", $"XmlRequest {XmlRequest.RequestType} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting XmlRequest {id}", UserId.Value);
                return View("Error");
            }
        }
        #endregion

        #region//----------------- WEB API's---------------//
        [HttpGet]
        [CustomAuthorize("XmlRequest", "Read")]
        public async Task<IActionResult> XmlRequests(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.xmlRequestService.GetAllAsync(UserId.Value);

                return Json(new
                {
                    Result
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all XmlRequest (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("XmlRequest", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.xmlRequestService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all XmlRequest (API)", UserId.Value);
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
        [CustomAuthorize("XmlRequest", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);

                if (XmlRequest == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    XmlRequest = new XmlRequestDTO();
                return Json(new { Data = XmlRequest });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching XmlRequest {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("XmlRequest", "Create")]
        public async Task<IActionResult> ApiAdd(XmlRequestDTO DTO)
        {
            try
            {
                await _serviceManager.xmlRequestService.AddAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "ApiAdd", $"API XmlRequest {DTO.RequestType} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating XmlRequest (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("XmlRequest", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, XmlRequestDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.xmlRequestService.UpdateAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "ApiUpdate", $"API XmlRequest {DTO.RequestType} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating XmlRequest {DTO.RequestType} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("XmlRequest", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var XmlRequest = await _serviceManager.xmlRequestService.GetByIdAsync(id, UserId.Value);
                if (XmlRequest == null)
                {
                    return NotFound();
                }

                await _serviceManager.xmlRequestService.DeleteAsync(XmlRequest, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "XmlRequestController", "ApiDelete", $"API XmlRequest {XmlRequest.RequestType} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting XmlRequest {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}

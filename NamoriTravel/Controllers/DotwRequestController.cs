using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModelsDTO;
using NamoriTravel.Authorize;
using NamoriTravel.Models;
using ServiceLayer;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("DotwRequest", "Visible")]
    public class DotwRequestController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public DotwRequestController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//
        [HttpGet]
        [CustomAuthorize("DotwRequest", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "DotwRequest List";
                //var DotwRequest = await _serviceManager.dotwRequestService.GetAllDotwRequestAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<DotwRequestsDTO>>(DotwRequest);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching DotwRequest", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("DotwRequest", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);
                if (DotwRequest == null)
                {
                    return NotFound();
                }
                return View(DotwRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for DotwRequest {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("DotwRequest", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("DotwRequest", "Create")]
        public async Task<IActionResult> Create(DotwRequestDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _serviceManager.dotwRequestService.AddAsync(model, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "Create", $"DotwRequest {model.Username} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating DotwRequest", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("DotwRequest", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);
                if (DotwRequest == null)
                {
                    return NotFound();
                }
                return View(DotwRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching DotwRequest {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("DotwRequest", "Update")]
        public async Task<IActionResult> Edit(int id, DotwRequestDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                await _serviceManager.dotwRequestService.UpdateAsync(model, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "Edit", $"DotwRequest {model.Username} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating DotwRequest {model.Username}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("DotwRequest", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);
                if (DotwRequest == null)
                {
                    return NotFound();
                }
                return View(DotwRequest);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching DotwRequest {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("DotwRequest", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);
                if (DotwRequest == null)
                {
                    return NotFound();
                }

                await _serviceManager.dotwRequestService.DeleteAsync(DotwRequest, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "DeleteConfirmed", $"DotwRequest {DotwRequest.Username} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting DotwRequest {id}", UserId.Value);
                return View("Error");
            }
        }
        #endregion

        #region//----------------- WEB API's---------------//
        [HttpGet]
        [CustomAuthorize("DotwRequest", "Read")]
        public async Task<IActionResult> DotwRequests(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.dotwRequestService.GetAllAsync(UserId.Value);

                return Json(new
                {
                    Result
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all DotwRequest (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [CustomAuthorize("DotwRequest", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.dotwRequestService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all DotwRequest (API)", UserId.Value);
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
        [CustomAuthorize("DotwRequest", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);

                if (DotwRequest == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    DotwRequest = new DotwRequestDTO();
                return Json(new { Data = DotwRequest });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching DotwRequest {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("DotwRequest", "Create")]
        public async Task<IActionResult> ApiAdd(DotwRequestDTO DTO)
        {
            try
            {
                await _serviceManager.dotwRequestService.AddAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "ApiAdd", $"API DotwRequest {DTO.Username} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating DotwRequest (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("DotwRequest", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, DotwRequestDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.dotwRequestService.UpdateAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "ApiUpdate", $"API DotwRequest {DTO.Username} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating DotwRequest {DTO.Username} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("DotwRequest", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var DotwRequest = await _serviceManager.dotwRequestService.GetByIdAsync(id, UserId.Value);
                if (DotwRequest == null)
                {
                    return NotFound();
                }

                await _serviceManager.dotwRequestService.DeleteAsync(DotwRequest, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DotwRequestController", "ApiDelete", $"API DotwRequest {DotwRequest.Username} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting DotwRequest {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using NamoriTravel.Models;
using ModelsDTO;
using AutoMapper;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Permissions", "Visible")]
    public class PermissionsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public PermissionsController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//

        [HttpGet]
        [CustomAuthorize("Permissions", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "Permissions List";
                //var permissions = await _serviceManager.permissionService.GetAllPermissionsAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<PermissionViewModel>>(permissions);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all permissions", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Permissions", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var permission = await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value);
                if (permission == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<PermissionViewModel>(permission));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for permission {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Permissions", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Permissions", "Create")]
        public async Task<IActionResult> Create(PermissionViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var permissionDto = _mapper.Map<PermissionDTO>(model);
                await _serviceManager.permissionService.AddPermissionAsync(permissionDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "Create", $"Permission {model.PermissionName} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating permission", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Permissions", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var permission = _mapper.Map<PermissionViewModel>(await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value));
                if (permission == null)
                {
                    return NotFound();
                }
                return View(permission);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching permission {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Permissions", "Update")]
        public async Task<IActionResult> Edit(int id, PermissionViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var permissionDto = _mapper.Map<PermissionDTO>(model);
                await _serviceManager.permissionService.UpdatePermissionAsync(permissionDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "Edit", $"Permission {model.PermissionName} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating permission {model.PermissionName}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Permissions", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var permission = await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value);
                if (permission == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<PermissionViewModel>(permission));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching permission {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Permissions", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var permission = await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value);
                if (permission == null)
                {
                    return NotFound();
                }

                await _serviceManager.permissionService.DeletePermissionAsync(permission, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "DeleteConfirmed", $"Permission {permission.PermissionName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting permission {id}", UserId.Value);
                return View("Error");
            }
        }

        #endregion

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Permissions", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var permissions = await _serviceManager.permissionService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = permissions.DTO.Count(),
                    iTotalDisplayRecords = permissions.Total,
                    aaData = permissions.DTO
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all permissions (API)", UserId.Value);
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
        [CustomAuthorize("Permissions", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var permission = await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value);
                if (permission == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                    permission = new PermissionDTO();
                return Json(new { Data = permission });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching permission {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Permissions", "Create")]
        public async Task<IActionResult> ApiAdd(PermissionDTO DTO)
        {
            try
            {
                await _serviceManager.permissionService.AddPermissionAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "ApiAdd", $"API permission {DTO.PermissionName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating permission (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Permissions", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, PermissionDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.permissionService.UpdatePermissionAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "ApiUpdate", $"API permission {DTO.PermissionName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating permission {DTO.PermissionName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Permissions", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var permission = await _serviceManager.permissionService.GetPermissionByIdAsync(id, UserId.Value);
                if (permission == null)
                {
                    return NotFound();
                }
                await _serviceManager.permissionService.DeletePermissionAsync(permission, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PermissionsController", "ApiDelete", $"API permission {permission.PermissionName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting permission {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

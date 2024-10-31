using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using NamoriTravel.Models;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Role", "Visible")]
    public class RoleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public RoleController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//

        [HttpGet]
        [CustomAuthorize("Role", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "Role List";
                //var roles = await _serviceManager.roleService.GetAllRolesAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all roles", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Role", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<RoleViewModel>(role));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for role {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Role", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Role", "Create")]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var roleDto = _mapper.Map<RoleDTO>(model);
                await _serviceManager.roleService.AddRoleAsync(roleDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "Create", $"Role {model.RoleName} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating role", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Role", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var role = _mapper.Map<RoleViewModel>(await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value));
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching role {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Role", "Update")]
        public async Task<IActionResult> Edit(int id, RoleViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var roleDto = _mapper.Map<RoleDTO>(model);
                await _serviceManager.roleService.UpdateRoleAsync(roleDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "Edit", $"Role {model.RoleName} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating role {model.RoleName}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Role", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<RoleViewModel>(role));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching role {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Role", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null)
                {
                    return NotFound();
                }

                await _serviceManager.roleService.DeleteRoleAsync(role, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "DeleteConfirmed", $"Role {role.RoleName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting role {id}", UserId.Value);
                return View("Error");
            }
        }

        #endregion

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Role", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.roleService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all roles (API)", UserId.Value);
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
        [CustomAuthorize("Role", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null && id !=0)
                {
                    return NotFound();
                }
                if (id == 0)
                    role = new RoleDTO();

                return Json(new { Data = role });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching role {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Role", "Create")]
        public async Task<IActionResult> ApiAdd(RoleDTO DTO)
        {
            try
            {
                await _serviceManager.roleService.AddRoleAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "ApiAdd", $"API role {DTO.RoleName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating role (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Role", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, RoleDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.roleService.UpdateRoleAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "ApiUpdate", $"API role {DTO.RoleName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating role {DTO.RoleName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Role", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null)
                {
                    return NotFound();
                }
                await _serviceManager.roleService.DeleteRoleAsync(role, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "RoleController", "ApiDelete", $"API role {role.RoleName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting role {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

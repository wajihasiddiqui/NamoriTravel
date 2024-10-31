using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using NamoriTravel.Models;
using ServiceLayer;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
   

    [CustomAuthorize("Room", "Visible")]
    public class RoomController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public RoomController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [CustomAuthorize("Room", "Read")]
        public IActionResult Index()
        {
            ViewBag.TblTitle = "Room";
            return View();
        }

        #region //----------------- WEB API's---------------//

        [HttpGet]
       [CustomAuthorize("Room", "Read")]
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
       [CustomAuthorize("Room", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var role = await _serviceManager.roleService.GetRoleByIdAsync(id, UserId.Value);
                if (role == null && id != 0)
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
       [CustomAuthorize("Room", "Create")]
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
       [CustomAuthorize("Room", "Update")]
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
       [CustomAuthorize("Room", "Delete")]
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

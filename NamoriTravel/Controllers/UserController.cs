using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using NamoriTravel.Models;
using ModelsDTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("User", "Visible")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public UserController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //-------------------MVC Actions-------------------//
        [HttpGet]
        [CustomAuthorize("User", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "User List";
                //var users = await _serviceManager.userService.GetAllUsersAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<UserViewModel>>(users);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching users", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("User", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for user {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("User", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("User", "Create")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var userDto = _mapper.Map<UserDTO>(model);
                await _serviceManager.userService.AddUserAsync(userDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "Create", $"User {model.Username} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating user", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("User", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching user {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("User", "Update")]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var userDto = _mapper.Map<UserDTO>(model);
                await _serviceManager.userService.UpdateUserAsync(userDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "Edit", $"User {model.Username} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating user {model.Username}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("User", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching user {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("User", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                if (user == null)
                {
                    return NotFound();
                }

                await _serviceManager.userService.DeleteUserAsync(user, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "DeleteConfirmed", $"User {user.UserName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting user {id}", UserId.Value);
                return View("Error");
            }
        }

        //----------------- WEB API's---------------//
        [HttpGet]
        [CustomAuthorize("User", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.userService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                var roleList = await _serviceManager.roleService.GetAllRolesAsync(UserId.Value);
                var departList = await _serviceManager.DepartmentService.GetAllDepartmentAsync(UserId.Value);
                var groupList = await _serviceManager.groupService.GetAllGroupAsync(UserId.Value);

                foreach (var item in Result.DTO)
                {
                    // Update GroupName
                    var group = groupList.FirstOrDefault(g => g.Id == item.GroupId);
                    item.uGroupName = group != null ? group.GroupName : string.Empty;

                    // Update DepartmentName
                    var department = departList.FirstOrDefault(d => d.Id == item.DepartmentId);
                    item.uDepartmentName = department != null ? department.DepartmentName : string.Empty;

                    // Update RoleName
                    var role = roleList.FirstOrDefault(r => r.Id == item.RoleId);
                    item.uRoleName = role != null ? role.RoleName : string.Empty;
                }

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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all users (API)", UserId.Value);
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
        [CustomAuthorize("User", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                var role = await _serviceManager.roleService.GetAllRolesAsync(UserId.Value);
                var deprt = await _serviceManager.DepartmentService.GetAllDepartmentAsync(UserId.Value);
                var group = await _serviceManager.groupService.GetAllGroupAsync(UserId.Value);
                if (user == null && id !=0)
                {
                    return NotFound();
                }
                if (id == 0)
                    user = new UserDTO();
                return Json(new { Data = user , Role=role.Select(x=> new {x.Id,x.RoleName}) , Department= deprt.Select(x => new {x.Id,x.DepartmentName }), Group = group.Select(x => new {x.Id,x.GroupName })});
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching user {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("User", "Create")]
        public async Task<IActionResult> ApiAdd(UserDTO DTO)
        {
            try
            {
                await _serviceManager.userService.AddUserAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "ApiAdd", $"API user {DTO.UserName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating user (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("User", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, UserDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.userService.UpdateUserAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "ApiUpdate", $"API user {DTO.UserName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating user {DTO.UserName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("User", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var user = await _serviceManager.userService.GetUserByIdAsync(id, UserId.Value);
                if (user == null)
                {
                    return NotFound();
                }

                await _serviceManager.userService.DeleteUserAsync(user, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "UserController", "ApiDelete", $"API user {user.UserName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting user {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

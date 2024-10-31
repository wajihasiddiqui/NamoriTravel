using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using NamoriTravel.Models;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Group", "Visible")]
    public class GroupController : BaseController
    {
        private readonly IMapper _mapper;
        IServiceManager _serviceManager;
        public GroupController(IServiceManager  serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//

        [HttpGet]
        [CustomAuthorize("Group", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "Group List";
                //var groups = await _serviceManager.groupService.GetAllGroupAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<GroupViewModel>>(groups);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all groups", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Group", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var group = await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value);
                if (group == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<GroupViewModel>(group));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for group {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Group", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Group", "Create")]
        public async Task<IActionResult> Create(GroupViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var groupDto = _mapper.Map<GroupDTO>(model);
                await _serviceManager.groupService.AddGroupAsync(groupDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "Create", $"Group {model.GroupName} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating group", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Group", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var group = _mapper.Map<GroupViewModel>(await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value));
                if (group == null)
                {
                    return NotFound();
                }
                return View(group);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching group {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Group", "Update")]
        public async Task<IActionResult> Edit(int id, GroupViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var groupDto = _mapper.Map<GroupDTO>(model);
                await _serviceManager.groupService.UpdateGroupAsync(groupDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "Edit", $"Group {model.GroupName} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating group {model.GroupName}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Group", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var group = await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value);
                if (group == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<GroupViewModel>(group));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching group {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Group", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var group = await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value);
                if (group == null)
                {
                    return NotFound();
                }

                await _serviceManager.groupService.DeleteGroupAsync(group, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "DeleteConfirmed", $"Group {group.GroupName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting group {id}", UserId.Value);
                return View("Error");
            }
        }

        #endregion

        #region //----------------- WEB API's---------------//

        [HttpGet]
        [CustomAuthorize("Group", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Result = await _serviceManager.groupService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
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
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all groups (API)", UserId.Value);
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
        [CustomAuthorize("Group", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var group = await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value);
                if (group == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0)
                {
                    group = new GroupDTO();
                }
                return Json(new { Data = group });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching group {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Group", "Create")]
        public async Task<IActionResult> ApiAdd(GroupDTO DTO)
        {
            try
            {
                await _serviceManager.groupService.AddGroupAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "ApiAdd", $"API group {DTO.GroupName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating group (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Group", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, GroupDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.groupService.UpdateGroupAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "ApiUpdate", $"API group {DTO.GroupName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating group {DTO.GroupName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Group", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var group = await _serviceManager.groupService.GetGroupByIdAsync(id, UserId.Value);
                if (group == null)
                {
                    return NotFound();
                }
                await _serviceManager.groupService.DeleteGroupAsync(group, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "GroupController", "ApiDelete", $"API group {group.GroupName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting group {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

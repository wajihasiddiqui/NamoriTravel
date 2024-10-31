using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using AutoMapper;
using NamoriTravel.Models;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("AccessManagement", "Visible")]
    public class AccessManagementController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public AccessManagementController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        [CustomAuthorize("AccessManagement", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new AccessManagementViewModel()
                {
                    Groups = _mapper.Map<List<GroupViewModel>>(await _serviceManager.groupService.GetAllGroupAsync(UserId.Value)),
                    Pages = _mapper.Map<List<PageViewModel>>(await _serviceManager.pageService.GetAllPagesAsync(UserId.Value)),
                    Permissions = _mapper.Map<List<PermissionViewModel>>(await _serviceManager.permissionService.GetAllPermissionsAsync(UserId.Value)),
                };

                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "Access Management Controller", "Index", "Accessed Index page");
                return View(model);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error accessing Index page", UserId.Value);
                return StatusCode(500, "An error occurred while accessing the Index page.");
            }
        }

        public async Task<IActionResult> GetAccessData(int GroupID, string type)
        {
            var model = new AccessManagementViewModel();
            try
            {
                switch (type)
                {
                    case "Pages":
                        var allPermissions = _mapper.Map<List<PermissionViewModel>>(await _serviceManager.permissionService.GetAllPermissionsAsync(UserId.Value));
                        var allPages = _mapper.Map<List<PageViewModel>>(await _serviceManager.pageService.GetAllPagesAsync(UserId.Value));
                        var userPagesWithPermissions = _mapper.Map<List<PagePermissionsObjViewModel>>(await _serviceManager.pageService.GetPagePermissionsByGroupId(GroupID, UserId.Value));

                        model.Pages = allPages;
                        model.Permissions = allPermissions;
                        ViewBag.UserPagesWithPermissions = userPagesWithPermissions;

                        await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "Access Management Controller", "GetAccessData", $"Fetched access data for GroupID {GroupID}");
                        return PartialView("_PageAccessTreeView", model);
                    default:
                        return PartialView("_EmptyTreeView");
                }
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching access data for GroupID {GroupID}", UserId.Value);
                return StatusCode(500, "An error occurred while fetching access data.");
            }
        }

        [HttpGet]
        [CustomAuthorize("AccessManagement", "Read")]
        public async Task<IActionResult> GetPagePermissions(int groupId)
        {
            try
            {
                var pagePermissions = await _serviceManager.pageService.GetPagePermissionsByGroupId(groupId, UserId.Value);
                var allPages = await _serviceManager.pageService.GetAllPagesAsync(UserId.Value);
                var result = allPages.Select(page => new
                {
                    PageId = page.Id,
                    PageName = page.PageName,
                    Permissions = pagePermissions
                        .Where(pp => pp.PageId == page.Id)
                        .Select(pp => new
                        {
                            PermissionId = pp.PermissionId,
                            PermissionName = pp.PermissionName
                        })
                        .ToList()
                }).ToList();

                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "Access Management Controller", "GetPagePermissions", $"Fetched page permissions for group {groupId}");
                return Json(result);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching page permissions for group {groupId}", UserId.Value);
                return StatusCode(500, "An error occurred while fetching page permissions.");
            }
        }

        [HttpPost]
        [CustomAuthorize("AccessManagement", "Update")]
        public async Task<IActionResult> UpdatePagePermissions([FromBody] UpdatePagePermissionsRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _serviceManager.loggingService.LogErrorAsync(null, "Invalid data. Request is null", UserId.Value);
                    return BadRequest("Invalid data.");
                }

                int groupId = request.GroupId;
                List<PagePermissionDTO> permissions = request.Permissions;
                await _serviceManager.pageService.UpdatePagePermissions(groupId, permissions, UserId.Value);

                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "Access Management Controller", "UpdatePagePermissions", $"Updated permissions for group {groupId}");
                return Ok(new { message = "Permissions updated successfully" });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, ex.Message, UserId.Value);
                return StatusCode(500, "An error occurred while updating permissions.");
            }
        }
    }
}

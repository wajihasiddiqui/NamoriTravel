using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using NamoriTravel.Models;
using AutoMapper;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Department", "Visible")]
    public class DepartmentController : BaseController
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        public DepartmentController(IServiceManager serviceManager ,IMapper mapper)
        {
            //_serviceManager.DepartmentService = Service ?? throw new ArgumentNullException(nameof(Service));
            _mapper = mapper;
            _serviceManager = serviceManager;
            // _serviceManager.loggingService = loggingService;
        }

        #region//-------------------MVC Actions-------------------//
        [HttpGet]
        [CustomAuthorize("Department", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "Department List";
                //var Department = await _serviceManager.DepartmentService.GetAllDepartmentAsync(UserId.Value);
                //var Result = _mapper.Map<IEnumerable<DepartmentViewModel>>(Department);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all departments", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Department", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var Department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value);
                if (Department == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<DepartmentViewModel>(Department));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for department {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Department", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Department", "Create")]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            var DepartmentDto = _mapper.Map<DepartmentDTO>(model);
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceManager.DepartmentService.AddDepartmentAsync(DepartmentDto, UserId.Value);
                    await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "Create", $"Department {model.DepartmentName} created.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating department", UserId.Value);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [CustomAuthorize("Department", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var Department = _mapper.Map<DepartmentViewModel>(await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value));
                if (Department == null)
                {
                    return NotFound();
                }
                return View(Department);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching department {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Department", "Update")]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var DepartmentDto = _mapper.Map<DepartmentDTO>(model);

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceManager.DepartmentService.UpdateDepartmentAsync(DepartmentDto, UserId.Value);
                    await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "Edit", $"Department {model.DepartmentName} updated.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating department {model.DepartmentName}", UserId.Value);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value);
                if (Department == null)
                {
                    return NotFound();
                }
                else
                {
                    await _serviceManager.DepartmentService.DeleteDepartmentAsync(Department, UserId.Value);
                    await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "Delete", $"Department {Department.DepartmentName} deleted.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching department {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Department", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value);
                if (Department == null)
                {
                    return NotFound();
                }

                await _serviceManager.DepartmentService.DeleteDepartmentAsync(Department, UserId.Value);
                await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "DeleteConfirmed", $"Department {Department.DepartmentName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting department {id}", UserId.Value);
                return View("Error");
            }
        }
        #endregion

        #region //------------------WEB API--------------//
        [HttpGet]
        [CustomAuthorize("Department", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var Departments = await _serviceManager.DepartmentService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns, param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = Departments.DTO.Count(),
                    iTotalDisplayRecords = Departments.Total,
                    aaData = Departments.DTO
                });
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all departments (API)", UserId.Value);
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
        [CustomAuthorize("Department", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var Department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value);
                if (Department == null && id !=0)
                {
                    return NotFound();
                }
                if (id == 0)
                    Department = new DepartmentDTO();
                return Json(new {Data = Department });
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching department {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Department", "Create")]
        public async Task<IActionResult> ApiAdd(DepartmentDTO DTO)
        {
            try
            {
                await _serviceManager.DepartmentService.AddDepartmentAsync(DTO, UserId.Value);
                await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "ApiAdd", $"API department {DTO.DepartmentName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, "Error creating department (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Department", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, DepartmentDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }

                await _serviceManager.DepartmentService.UpdateDepartmentAsync(DTO, UserId.Value);
                await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "ApiUpdate", $"API department {DTO.DepartmentName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating department {DTO.DepartmentName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Department", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var Department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, UserId.Value);
                if (Department == null)
                {
                    return NotFound();
                }
                await _serviceManager.DepartmentService.DeleteDepartmentAsync(Department, UserId.Value);
                await  _serviceManager.loggingService.LogAuditAsync(UserId.Value, "DepartmentController", "ApiDelete", $"API department {Department.DepartmentName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await  _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting department {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}

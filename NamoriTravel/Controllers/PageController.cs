using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;
using ServiceLayer;
using AutoMapper;
using NamoriTravel.Models;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [Route("Page/[action]")]
    [CustomAuthorize("Page", "Visible")]
    public class PageController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        public PageController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region //-------------------MVC Actions-------------------//

        [HttpGet]
        [CustomAuthorize("Page", "Read")]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TblTitle = "Page List";
                //var pages = await _serviceManager.pageService.GetAllPagesAsync(UserId.Value);
                //var result = _mapper.Map<IEnumerable<PageViewModel>>(pages);
                return View();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all pages", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Page", "Read")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var page = await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value);
                if (page == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<PageViewModel>(page));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching details for page {id}", UserId.Value);
                return View("Error");
            }
        }

        [HttpGet]
        [CustomAuthorize("Page", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Page", "Create")]
        public async Task<IActionResult> Create(PageViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.IsActive = true;
                model.IsDeleted = false;
                var pageDto = _mapper.Map<PageDTO>(model);
                await _serviceManager.pageService.AddPageAsync(pageDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "Create", $"Page {model.PageName} created.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating page", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Page", "Update")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var page = _mapper.Map<PageViewModel>(await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value));
                if (page == null)
                {
                    return NotFound();
                }
                return View(page);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching page {id} for editing", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Page", "Update")]
        public async Task<IActionResult> Edit(int id, PageViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var pageDto = _mapper.Map<PageDTO>(model);
                await _serviceManager.pageService.UpdatePageAsync(pageDto, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "Edit", $"Page {model.PageName} updated.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating page {model.PageName}", UserId.Value);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize("Page", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var page = await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value);
                if (page == null)
                {
                    return NotFound();
                }
                return View(_mapper.Map<PageViewModel>(page));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching page {id} for deletion", UserId.Value);
                return View("Error");
            }
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Page", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var page = await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value);
                if (page == null)
                {
                    return NotFound();
                }

                await _serviceManager.pageService.DeletePageAsync(page, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "DeleteConfirmed", $"Page {page.PageName} deleted.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting page {id}", UserId.Value);
                return View("Error");
            }
        }

        #endregion

        #region //----------------- WEB API's---------------//
       
        string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
        
        [HttpGet]
        [CustomAuthorize("Page", "Read")]
        public async Task<IActionResult> ApiGetAll(DatatableParam param)
        {
            try
            {
                var pages = await _serviceManager.pageService.GetAllFilteredAsync(UserId.Value, param.sSearch, param.sColumns,param.iSortingCols, param.sSortDir_0, param.iDisplayStart, param.iDisplayLength);
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = pages.pageDTOs.Count(),
                    iTotalDisplayRecords = pages.Total,
                    aaData = pages.pageDTOs
                });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error fetching all pages (API)", UserId.Value);
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
        [CustomAuthorize("Page", "Read")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            try
            {
                var page = await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value);
                var AllPages = await _serviceManager.pageService.GetAllPagesAsync(UserId.Value);
                

                if (page == null && AllPages == null && id != 0)
                {
                    return NotFound();
                }
                if (id == 0 && page == null)
                {
                    page = new PageDTO();
                    page.ParentPageId = 0;
                    page.CreatedBy = UserId.Value;
                    page.CreatedDate = DateTime.UtcNow;
                    page.IsActive = true;
                    page.IsDeleted = false;
                }
                return Json(new { Data = page, allData = AllPages.Select(x => new { x.Id, x.PageName }) });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error fetching page {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Page", "Create")]
        public async Task<IActionResult> ApiAdd(PageDTO DTO)
        {
            try
            {
                await _serviceManager.pageService.AddPageAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "ApiAdd", $"API page {DTO.PageName} created.");
                return CreatedAtAction(nameof(ApiGetById), new { id = DTO.Id }, DTO);
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error creating page (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Page", "Update")]
        public async Task<IActionResult> ApiUpdate(int id, PageDTO DTO)
        {
            try
            {
                if (id != DTO.Id)
                {
                    return BadRequest();
                }
                await _serviceManager.pageService.UpdatePageAsync(DTO, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "ApiUpdate", $"API page {DTO.PageName} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error updating page {DTO.PageName} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize("Page", "Delete")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            try
            {
                var page = await _serviceManager.pageService.GetPageByIdAsync(id, UserId.Value);
                if (page == null)
                {
                    return NotFound();
                }
                await _serviceManager.pageService.DeletePageAsync(page, UserId.Value);
                await _serviceManager.loggingService.LogAuditAsync(UserId.Value, "PageController", "ApiDelete", $"API page {page.PageName} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, $"Error deleting page {id} (API)", UserId.Value);
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}

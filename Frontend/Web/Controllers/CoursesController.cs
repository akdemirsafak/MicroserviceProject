using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedLibrary.Services;
using Web.Models.Catalogs;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }
        public async Task<IActionResult> Index()
        {
            var myCourses = await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId);
            return View(myCourses);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(courseCreateInput);
            }
            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            await _catalogService.CreateCourseAsync(courseCreateInput);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseId(id);
            var categories = await _catalogService.GetAllCategoriesAsync();
            if (course is null)
            {
                RedirectToAction(nameof(Index));
            }
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                CategoryId = course.CategoryId,
                Picture = course.Picture,
                UserId = course.UserId,
                Feature = course.Feature
            };
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId); //! 3. parametre ile bu kursa ait kategoriyi seçtik.
            return View(courseUpdateInput);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                var course = await _catalogService.GetByCourseId(input.Id);
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);
                return View(input);
            }
            await _catalogService.UpdateCourseAsync(input);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateInput categoryCreateInput)
        {
            var categories = await _catalogService.CreateCategoryAsync(categoryCreateInput);
            return RedirectToAction(nameof(Index));
        }

    }
}

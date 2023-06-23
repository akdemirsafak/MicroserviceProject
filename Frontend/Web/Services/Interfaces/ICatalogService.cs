using Web.Models.Catalogs;

namespace Web.Services.Interfaces;

public interface ICatalogService
{
    Task<List<CourseViewModel>> GetAllCoursesAsync();
    Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId);
    Task<CourseViewModel> GetByCourseId(string courseId);
    Task<bool> CreateCourseAsync(CourseCreateInput input);
    Task<bool> UpdateCourseAsync(CourseUpdateInput input);
    Task<bool> DeleteCourseAsync(string courseId);

    Task<List<CategoryViewModel>> GetAllCategoriesAsync();
}

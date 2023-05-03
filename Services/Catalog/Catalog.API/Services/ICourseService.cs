using Catalog.API.Dtos;
using SharedLibrary.Dtos;

namespace Catalog.API.Services;

public interface ICourseService
{
    Task<SharedLibrary.Dtos.Response<List<CourseDto>>> GetAllAsync();
    Task<SharedLibrary.Dtos.Response<CourseDto>> GetByIdAsync(string id);
    Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);
    Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);
    Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto);
    Task<Response<NoContent>> DeleteAsync(string courseId);
}

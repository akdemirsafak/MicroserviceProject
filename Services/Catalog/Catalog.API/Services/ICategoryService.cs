using Catalog.API.Dtos;
using Catalog.API.Models;
using SharedLibrary.Dtos;

namespace Catalog.API.Services;

public interface ICategoryService
{
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(string id);
    Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
}

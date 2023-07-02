using SharedLibrary.Dtos;
using System.Net.Http.Json;
using Web.Models;
using Web.Models.Catalogs;
using Web.Services.Interfaces;

namespace Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly IPhotoStockService _photoStockService;

    public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService)
    {
        _httpClient = httpClient;
        _photoStockService = photoStockService;
    }


    public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
    {
        var response= await _httpClient.GetAsync("categories"); //Controlleradı
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseSuccess=await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<List<CourseViewModel>> GetAllCoursesAsync()
    {
        var response= await _httpClient.GetAsync("courses"); //Controlleradı
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseSuccess=await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
        return responseSuccess.Data;
    
    }

    public async Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId)
    {//GetAllByUserId/{userId}"
        var response= await _httpClient.GetAsync($"courses/GetAllByUserId/{userId}"); //Controlleradı
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseSuccess=await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<CourseViewModel> GetByCourseId(string courseId)
    {
        var response= await _httpClient.GetAsync($"courses/{courseId}"); //Controlleradı
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseSuccess=await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
        return responseSuccess.Data;
    }

    public async Task<bool> CreateCourseAsync(CourseCreateInput input)
    {
        var resultPhotoService= await _photoStockService.UploadPhoto(input.PhotoFormFile);
        if (resultPhotoService is not null)
        {
            input.Picture = resultPhotoService.Url;
        }
        var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>($"courses",input);
        return response.IsSuccessStatusCode;
    }

 
    public async Task<bool> UpdateCourseAsync(CourseUpdateInput input)
    {
        var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>($"courses",input);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        var response = await _httpClient.DeleteAsync($"courses/{courseId}");
        return response.IsSuccessStatusCode;
    }
}

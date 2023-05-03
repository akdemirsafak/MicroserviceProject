using Catalog.API.Dtos;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : CustomBaseController
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService) => _courseService = courseService;

    //---!!!----//
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _courseService.GetAllAsync();
        return CreateActionResult(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) 
    {
        var response= await _courseService.GetByIdAsync(id);
        return CreateActionResult(response);
    }
    
    //[Route("/api/[controller]/GetAllByUserId/{id}")]
    [HttpGet("GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _courseService.GetAllByUserIdAsync(userId);
        return CreateActionResult(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
    {
        return CreateActionResult(await _courseService.CreateAsync(courseCreateDto));
    }
    [HttpPut]
    public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
    {
        return CreateActionResult(await _courseService.UpdateAsync(courseUpdateDto));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return CreateActionResult(await _courseService.DeleteAsync(id));
    }
}
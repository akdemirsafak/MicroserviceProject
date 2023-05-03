using Catalog.API.Dtos;
using Catalog.API.Models;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : CustomBaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return CreateActionResult(await _categoryService.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) 
    {
        return CreateActionResult(await _categoryService.GetByIdAsync(id));
    }
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
    {
        return CreateActionResult(await _categoryService.CreateAsync(categoryCreateDto));
    }
}

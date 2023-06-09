﻿using AutoMapper;
using Catalog.API.Dtos;
using Catalog.API.Models;
using Catalog.API.Settings;
using MongoDB.Driver;
using SharedLibrary.Dtos;

namespace Catalog.API.Services;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;
    
    public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryCollection.Find(category=>true).ToListAsync();
        var categoryListDto = _mapper.Map<List<CategoryDto>>(categories);
        return Response<List<CategoryDto>>.Success(categoryListDto,200);
    }
    public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
    {
        var category=_mapper.Map<Category>(categoryCreateDto);
        await _categoryCollection.InsertOneAsync(category);
        
        return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 201); //Projede 200 dönüldü. 
    }
    public async Task<Response<CategoryDto>> GetByIdAsync(string id)
    {
        var category= await _categoryCollection.Find<Category>(x=>x.Id==id).FirstOrDefaultAsync();
        if (category is null)
        {
            return Response<CategoryDto>.Fail("Category NotFound", 400);
        }
        return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);

    }


}

﻿using AutoMapper;
using Catalog.API.Dtos;
using Catalog.API.Models;
using Catalog.API.Settings;
using MongoDB.Driver;
using SharedLibrary.Dtos;

namespace Catalog.API.Services;

public class CourseService:ICourseService
{
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper,IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
        _categoryCollection=database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<SharedLibrary.Dtos.Response<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();
        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }
        List<CourseDto> courseListDto = _mapper.Map<List<CourseDto>>(courses);
        return Response<List<CourseDto>>.Success(courseListDto, 200);
    }
    
    public async Task<SharedLibrary.Dtos.Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find(course =>course.Id==id).FirstAsync();
        
        if (course==null)
        {
            
            return Response<CourseDto>.Fail("Course Not Found.", 404);
        }
        course.Category = await _categoryCollection.Find(x=>x.Id==course.CategoryId).FirstAsync();
        return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }
    public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courseCollection.Find(user =>user.UserId==userId ).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync(); //! Patlar
            }
        }
        else
        {
            courses = new List<Course>();
        }
        List<CourseDto> courseListDto = _mapper.Map<List<CourseDto>>(courses);
        return Response<List<CourseDto>>.Success(courseListDto, 200);
    }
    public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);
        newCourse.CreatedAt = DateTime.Now;
        await _courseCollection.InsertOneAsync(newCourse);
        return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse),201);
    }
    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var updateCourse=_mapper.Map<Course>(courseUpdateDto);
        var result =await _courseCollection.FindOneAndReplaceAsync(x=>x.Id== courseUpdateDto.Id,updateCourse);
        if (result is null)
        {
            return Response<NoContent>.Fail("Course Not Found", 404);
        }
        
        return Response<NoContent>.Success(204);
    }
    public async Task<Response<NoContent>> DeleteAsync(string courseId)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == courseId);
        if (result.DeletedCount>0)
        {
            return Response<NoContent>.Success(204);
            
        }
        return Response<NoContent>.Fail("Course Not Found", 404);


    }

}
    
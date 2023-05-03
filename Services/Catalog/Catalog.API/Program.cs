using Catalog.API.Mapping;
using Catalog.API.Services;
using Catalog.API.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Options Pattern Implementation
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings")); //Datalari okuyarak DatabaseSettings class'ini doldurdu
builder.Services.AddSingleton<IDatabaseSettings>(serviceProvider=> {
    return serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value; 
});//IDatabaseSettings bir ctor'da gecildiginde bize DatabaseSettings'de tanimli degerler gelecek.

builder.Services.AddScoped<ICategoryService,CategoryService>(); //Dependency injection
builder.Services.AddScoped<ICourseService, CourseService>(); //Dependency injection
builder.Services.AddAutoMapper(typeof(GeneralMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

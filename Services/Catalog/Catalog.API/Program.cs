using Catalog.API.Mapping;
using Catalog.API.Services;
using Catalog.API.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configs =>
{
    configs.Authority = builder.Configuration["IdentityServerURL"]; //Token dağıtmakla görevli api.Kritik! Bu kısmı appSettings.json da belirttik
    //Private key ile imzalanmış bir token geldiğinde public key ile doğrulaması yapılacak 
    configs.Audience = "resource_catalog"; //IdentityServer'da belirttiğimiz isim.!
    configs.RequireHttpsMetadata = false; //Http kullandığımız için.
}); //Scheme name. Birden fazla token türü bekleniyor olabilir.Bu ayrımı yapmak için Scheme name kullanılır.





builder.Services.AddMassTransit(setting =>
{
    //Default port :5672
    setting.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});

// Add services to the container.

builder.Services.AddControllers(opt =>{
    opt.Filters.Add(new AuthorizeFilter()); //Bu Api'daki tüm controller'ları authorize etmiş olduk.
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

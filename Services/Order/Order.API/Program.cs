using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Order.Application.Queries.GetOrdersByUserId;
using Order.Infrastructure;
using SharedLibrary.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //sub alanının nameidentifier olarak görünmesinin önüne geçiyoruz.
//Burada kesinlikle bir sub alanı bekliyoruz bu sebeple yukarıdaki policy'i yazıyoruz.
//new AuthorizationPolicyBuilder().RequireClaim("scope", "discount_read"); requireAuthorizePolicy e bunu atasaydık sadece read permission gerekecekti.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configs =>
{
    configs.Authority = builder.Configuration["IdentityServerURL"];
    configs.Audience = "resource_discount";
    configs.RequireHttpsMetadata = false;
});


builder.Services.AddDbContext<OrderDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    opt => {
        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(OrderDbContext))!.GetName().Name); //DbContext'in bulunduğu class'ı dahil edelim.DbContext'den türetilen OrderDbContext'i dahil eder.
    });
});

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(GetOrdersByUserIdQueryHandler).Assembly);

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers(opt => { 
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //User gerektiren  (Resource Owner Token) Identity'lerde authorizefilter kullanıyoruz.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

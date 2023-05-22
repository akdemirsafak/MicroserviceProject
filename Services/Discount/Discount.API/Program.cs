using Discount.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SharedLibrary.Services;
using System.IdentityModel.Tokens.Jwt;

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService,SharedIdentityService>();


builder.Services.AddControllers(opt =>{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //User gerektiren  (Resource Owner Token) Identity'lerde authorizefilter kullanıyoruz.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDiscountService,DiscountService>();

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

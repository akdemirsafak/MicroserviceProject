using FakePayment.API;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configs =>
{
    configs.Authority = builder.Configuration["IdentityServerURL"];
    configs.Audience = "resource_fakepayment";
    configs.RequireHttpsMetadata = false;
});


//builder.Services.AddMassTransitHostedService(); 8. versiyon ile birlikte kullanılmasına gerek kalmadı.
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //User gerektiren  (Resource Owner Token) Identity'lerde authorizefilter kullanıyoruz.
});
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();


builder.Services.AddMassTransit(setting =>
{
    //Default port :5672
    setting.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(rabbitMqSettings.HostName, "/", host =>
        {
            host.Username(rabbitMqSettings.UserName);
            host.Password(rabbitMqSettings.Password);
        });
    });
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

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderDbContext>(opt=>{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    opt=>{
        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(OrderDbContext))!.GetName().Name); //DbContext'in bulunduğu class'ı dahil edelim.DbContext'den türetilen OrderDbContext'i dahil eder.
    });
});

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

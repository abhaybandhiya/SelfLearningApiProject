using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Repositories;
using SelfLearningApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

//Ye line Dependency Injection system ko batati hai ki jab IProductRepository maanga jaye, to ProductRepository provide karo.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Ye line batati hai ki jab IProductService maanga jaye, to ProductService provide karo.
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

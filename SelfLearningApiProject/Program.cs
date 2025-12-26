using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Mapping;
using SelfLearningApiProject.Middleware;
using SelfLearningApiProject.Repositories;
using SelfLearningApiProject.Repositories.Generic;
using SelfLearningApiProject.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

// WebApplication builder create karte hain, jo ki application ko configure karega
var builder = WebApplication.CreateBuilder(args);

// Serilog configuration ke liye. Yeh logging framework application ke logs ko manage karega aur unhe specified sinks (jaise console, file, etc.) me bhejega
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// JWT Authentication configuration ke liye. Yeh section JWT tokens ke sath authentication setup karta hai jisse secure endpoints banaye ja sakein
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     // Token validation parameters set karte hain jisse incoming JWT tokens ko validate kiya ja sake
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
         ValidAudience = builder.Configuration["JwtSettings:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(
         Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),

         NameClaimType = ClaimTypes.Name
     };
 }); // JWT Bearer authentication add karta hai jisse API requests authenticate ho sakein

    builder.Services.AddAuthorization(
       options =>
       {
           // 1) Role-based policy via policy name
           options.AddPolicy("AdminOnly", policy =>
               policy.RequireRole("Admin"));

           // 2) Claim required (jti) — token uniqueness present
           options.AddPolicy("HasJti", policy =>
               policy.RequireClaim(JwtRegisteredClaimNames.Jti));

           // 3) Department based access (custom claim)
           options.AddPolicy("SalesDept", policy =>
               policy.RequireClaim("department", "Sales"));

           // (bonus) Multi-condition example:
           // options.AddPolicy("MgmtOrAdmin", policy =>
           //     policy.RequireAssertion(ctx =>
           //         ctx.User.IsInRole("Admin") ||
           //         ctx.User.HasClaim("department", "Management")));
       }
    );

//Ye line Dependency Injection system ko batati hai ki jab IProductRepository maanga jaye, to ProductRepository provide karo.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Ye line batati hai ki jab IProductService maanga jaye, to ProductService provide karo.
builder.Services.AddScoped<IProductService, ProductService>();

// Ye line batati hai ki jab IUserRepository maanga jaye, to UserRepository provide karo.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Ye line batati hai ki jab IAuthService maanga jaye, to AuthService provide karo.
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ye line AutoMapper ko configure karti hai, jisse ki mapping profiles use ho sakein.
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFileService, FileService>(); // FileService ko Dependency Injection me add karte hain

builder.Services.AddMemoryCache(); // In-Memory Caching ke liye service add karte hain jisse frequently accessed data ko cache kiya ja sake taaki performance improve ho sake aur database load kam ho
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddHttpContextAccessor(); // HTTP context ko access karne ke liye service add karte hain jisse hum request-specific information jaise headers, user info, etc. ko access kar sakein
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>(); // CurrentUserService ko Dependency Injection me add karte 

// ye API versioning ko enable karta hai jisse hum apni API ke multiple versions ko manage kar sakein
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // default version set karte hain 1.0 agar client ne version specify nahi kiya 
    options.AssumeDefaultVersionWhenUnspecified = true; // agar version na diya ho
    options.ReportApiVersions = true; // response headers me version info
});

builder.Services.AddApiVersioning()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV"; // v1, v2
        options.SubstituteApiVersionInUrl = true;
    });


// Swagger configuration API documentation ke liye Swagger UI provide karta hai jisse hum API endpoints ko test kar sakte hain
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SelfLearningApiProject API",
        Version = "v1"
    });

    // JWT Authorize button ke liye
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<IJwtTokenService, JwtService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>(); // custom exception handling middleware ko pipeline me add karta hai
app.UseMiddleware<LoggingMiddleware>(); // custom logging middleware ko pipeline me add karta hai
app.UseMiddleware<RateLimitingMiddleware>(); // custom rate limiting middleware ko pipeline me add karta hai

app.UseAuthentication();  // ye JWT tokens ko validate karega
app.UseAuthorization(); // ye authenticated users ke access ko control karega

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SelfLearningApiProject v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

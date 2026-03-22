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

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

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
 });

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

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFileService, FileService>(); // FileService ko Dependency Injection me add karte hain

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>(); 

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); 
    options.AssumeDefaultVersionWhenUnspecified = true; 
    options.ReportApiVersions = true; 
});

//builder.Services.AddApiVersioning()
//    .AddApiExplorer(options =>
//    {
//        options.GroupNameFormat = "'v'VVV"; // v1, v2
//        options.SubstituteApiVersionInUrl = true;
//    });



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SelfLearningApiProject API",
        Version = "v1"
    });

    // JWT Authorize for button
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowReact");
app.UseMiddleware<ExceptionMiddleware>(); 
app.UseMiddleware<LoggingMiddleware>(); 
app.UseMiddleware<RateLimitingMiddleware>(); 
app.UseAuthentication();
app.UseAuthorization(); 

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

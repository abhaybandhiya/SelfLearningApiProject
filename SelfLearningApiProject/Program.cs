using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Mapping;
using SelfLearningApiProject.Repositories;
using SelfLearningApiProject.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

// WebApplication builder create karte hain, jo ki application ko configure karega
var builder = WebApplication.CreateBuilder(args);


// JWT Authentication configuration ke liye. Yeh section JWT tokens ke sath authentication setup karta hai jisse secure endpoints banaye ja sakein
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
         ValidAudience = builder.Configuration["JwtSettings:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
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

// Swagger configuration API documentation ke liye Swagger UI provide karta hai jisse hum API endpoints ko test kar sakte hain
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SelfLearningApiProject API",
        Version = "v1"
    });

    // 🔑 JWT Authorize button ke liye
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

var app = builder.Build();

app.UseAuthentication();  // ye JWT tokens ko validate karega
app.UseAuthorization();   // ye dekhega ki user ke paas permission hai ya nahi

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

app.UseAuthorization();

app.MapControllers();

app.Run();

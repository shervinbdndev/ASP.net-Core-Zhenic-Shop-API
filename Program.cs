using System.Text;
using ECommerceShopApi.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceShopApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options => {

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {

    options.TokenValidationParameters = new TokenValidationParameters {

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))

    };

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {

    c.SwaggerDoc("v1", new OpenApiInfo {

        Version = "v1",
        Title = "برای فروشگاه ژنیک Api",
        Description = "برای اتصال و توسعه نرمافزار دسکتاپ و موبایل پیاده سازی شده است"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {

        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization Header"

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {

        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {

                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"

                }
            },
            new string[] {}
        }

    });

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {

        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zhenic ECommerceShop API v1");
        c.RoutePrefix = string.Empty;

    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

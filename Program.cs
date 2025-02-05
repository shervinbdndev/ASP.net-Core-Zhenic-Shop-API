using System.Text;
using ECommerceShopApi.Utils;
using ECommerceShopApi.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ECommerceShopApi.Repositories.Role;
using ECommerceShopApi.Repositories.Account;
using ECommerceShopApi.Repositories.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ECommerceShopApi.Repositories.ProductNameSpace;
using ECommerceShopApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<UserLastLogin>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"] ?? throw new ArgumentException("key");
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

builder.Services.AddAuthorization(options => {

    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CategoryManagement", policy => policy.RequireRole("Admin"));
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

async Task SeedRolesAsync(IServiceProvider serviceProvider) {

    var rolesRepository = serviceProvider.GetRequiredService<IRolesRepository>();
    var roles = new[] {"Admin", "Customer"};

    foreach (var role in roles) {

        var roleExists = await rolesRepository.RoleExistsAsync(role);

        if (!roleExists) {

            await rolesRepository.CreateRoleAsync(role);
        }
    }
}

using (var scope = app.Services.CreateScope()) {

    var services = scope.ServiceProvider;

    await SeedRolesAsync(services);
}

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
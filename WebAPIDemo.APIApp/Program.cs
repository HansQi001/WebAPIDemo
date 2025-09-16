using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using WebAPIDemo.APIApp.Configurations;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Application.Services;
using WebAPIDemo.Application.Users.Services;
using WebAPIDemo.Infrastructure.Data;
using WebAPIDemo.Infrastructure.Repositories;
using WebAPIDemo.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var mvc = builder.Services.AddControllers();

// API versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader()//,
                                            //new HeaderApiVersionReader("X-Api-Version"),
                                            //new QueryStringApiVersionReader("api-version")
        );

    })
    .AddMvc(mvcOptions =>
    {
        // deprecate v1 for ALL controllers,
        // enumerate discovered controllers and apply the convention.
        var feature = new Microsoft.AspNetCore.Mvc.Controllers.ControllerFeature();
        // Populate discovered controllers using the same PartManager
        mvc.PartManager.PopulateFeature(feature);

        foreach (var controller in feature.Controllers)
        {
            mvcOptions.Conventions
                .Controller(controller.AsType())
                .HasDeprecatedApiVersion(1.0);
        }

    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = builder.Configuration["Jwt:SecurityName"],
        Type = SecuritySchemeType.ApiKey,
        Scheme = builder.Configuration["Jwt:SecurityScheme"],
        BearerFormat = builder.Configuration["Jwt:BearerFormat"],
        In = ParameterLocation.Header,
        Description = builder.Configuration["Jwt:BearerDescription"]
    });

    // Apply the scheme globally to all operations
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
            Array.Empty<string>()
        }
    });
});
// swagger page for each version
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DemoDb"));

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();

if (!builder.Environment.IsEnvironment("Testing"))
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}

var app = builder.Build();

app.UseSwagger();

//if (app.Environment.IsDevelopment())
//{
app.UseSwaggerUI(options =>
{
    // Serve Swagger UI at application's root
    options.RoutePrefix = string.Empty;
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json"
            , description.GroupName.ToUpperInvariant());
    }
});
//}

//if (app.Environment.IsProduction())
//{
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { } // need this for testing purpose
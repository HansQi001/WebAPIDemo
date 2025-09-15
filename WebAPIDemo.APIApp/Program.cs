using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPIDemo.APIApp.Configurations;
using WebAPIDemo.Domain.Interfaces;
using WebAPIDemo.Infrastructure.Data;
using WebAPIDemo.Infrastructure.Repositories;

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
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"),
            new QueryStringApiVersionReader("api-version")
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
builder.Services.AddSwaggerGen();
// swagger page for each version
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DemoDb"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();


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

app.MapControllers();

app.Run();

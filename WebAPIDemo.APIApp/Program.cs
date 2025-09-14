using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPIDemo.APIApp.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


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

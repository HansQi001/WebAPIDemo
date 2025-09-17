using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPIDemo.APIApp.Configurations
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _config;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration config)
        {
            _provider = provider;
            _config = config;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new()
                {
                    Title = _config["Swagger:Title"],
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated ? _config["Swagger:Description"] : null
                });
            }
        }
    }
}

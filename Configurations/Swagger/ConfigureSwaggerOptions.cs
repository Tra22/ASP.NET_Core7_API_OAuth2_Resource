using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ASP.NET_CORE7_API_OAUTH2_RESOURCE.Configurations.Swagger {
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>{
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                options.AddSecurityDefinition(
                        description.GroupName,
                        CreateSecurityScheme());
                options.AddSecurityRequirement(CreateSecurityRequirement(description.GroupName));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Example Web API",
                Version = description.ApiVersion.ToString(),
                Description = "Description for the example Web API",
                Contact = new OpenApiContact { Name = "Author name", Email = "author-main@org.com" },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
        private OpenApiSecurityScheme CreateSecurityScheme()
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer token\"",
            };
            return securityScheme;
        }

        private OpenApiSecurityRequirement CreateSecurityRequirement(string groupName)
        {
            var securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme, 
                                            Id = groupName}
                                        }, new string[] {}
                        }
                 };

            return securityRequirement;
        }
    }
}
using Asp.Versioning;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Configurations.Securities;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Configurations.Swagger;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Constants.Securities;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Policies.Handlers;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Policies.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

const string SwaggerRoutePrefix = "api-docs";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearerConfiguration(builder.Configuration["OAuth2_Jwt:Issuer"]??string.Empty, builder.Configuration["OAuth2_Jwt:Audience"]??string.Empty);

builder.Services.AddAuthorization(options => {
    var scopes = new[] {
        Scope.MANAGER_READ,
        Scope.MANAGER_WRITE,
        Scope.MANAGER_UPDATE,
        Scope.MANAGER_DELETE
    };

  Array.ForEach(scopes, scope =>
    options.AddPolicy(scope,
      policy => policy.Requirements.Add(
        new ScopeRequirement(builder.Configuration["OAuth2_Jwt:Issuer"]??string.Empty, scope)
      )
    )
  );
});

builder.Services
    .AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                })
    .AddApiExplorer(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

// Register our authorization handler.
builder.Services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // Add a custom operation filter which sets default values
    options.OperationFilter<SwaggerDefaultValues>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { options.RouteTemplate = $"{SwaggerRoutePrefix}/{{documentName}}/docs.json"; });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = SwaggerRoutePrefix;
        foreach (var description in app.DescribeApiVersions())
            options.SwaggerEndpoint($"/{SwaggerRoutePrefix}/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

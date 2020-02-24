using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyApi.Configurations
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach(var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Parky Api {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString()
                    });
            }
            //To include the xml comments in the controllers to the api doc.
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Rather than use the absolute path to the xml, we dynamically get the path.
            var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            options.IncludeXmlComments(cmlCommentFullPath);
        }
    }
}

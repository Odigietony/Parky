using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyApi.Data;
using ParkyApi.Repository.IRepository;
using AutoMapper;
using ParkyApi.ParkyMapper;
using ParkyApi.Repository.Implementations;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ParkyApi.Configurations;

namespace ParkyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>
               (options => options.UseSqlServer(Configuration.GetConnectionString("ParkyDbConnection")));
            services.AddControllers();
            services.AddAutoMapper(typeof(ParkyMappings));
            //This service adds versioning to the api
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");


            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddSwaggerGen();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            //services.AddSwaggerGen(options => {
            //    options.SwaggerDoc("ParkyOpenApiSpec",
            //        new Microsoft.OpenApi.Models.OpenApiInfo()
            //        {
            //            Title = "Parky API",
            //            Version = "1",
            //            Description = "Tony's firt API project for National Parks",
            //            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //            {
            //                Email = "odigietony.jr@gmail.com",
            //                Name = "Odigie Anthony Jr."
            //            },
            //            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //            {
            //                Name ="MIT License",
            //                Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //            } 
            //        });


            //    //options.SwaggerDoc("ParkyOpenApiSpecTrails",
            //    //   new Microsoft.OpenApi.Models.OpenApiInfo()
            //    //   {
            //    //       Title = "Parky API (Trails)",
            //    //       Version = "1",
            //    //       Description = "Tony's firt API project for Trails",
            //    //       Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //    //       {
            //    //           Email = "odigietony.jr@gmail.com",
            //    //           Name = "Odigie Anthony Jr."
            //    //       },
            //    //       License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //    //       {
            //    //           Name = "MIT License",
            //    //           Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //    //       }
            //    //   });
            //    // To include the xml comments in the controllers to the api doc.
            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Rather than use the absolute path to the xml, we dynamically get the path.
            //    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(cmlCommentFullPath);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            //For api documentation point swagger ui to the swagger default url
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });
            //app.UseSwaggerUI(options => {
            //    options.SwaggerEndpoint("/swagger/ParkyOpenApiSpec/swagger.json", "Parky API");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenApiSpecTrails/swagger.json", "Parky API (Trails)");
            //    options.RoutePrefix = ""; // this is to ensure that the API doc is loaded as the default lauching page (remove the default lauchingurl from lauchsettings.json)
            //});
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

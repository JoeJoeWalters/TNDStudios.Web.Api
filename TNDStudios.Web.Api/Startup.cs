using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using TNDStudios.Web.Api.Documentation;
using TNDStudios.Web.ApiManager.Security;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace TNDStudios.Web.Api
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
            // Set up the authentication service with the appropriate authenticator implementation
            FileStream accessControl = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "users.json"));
            IUserAuthenticator userAuthenticator = new UserAuthenticator();
            userAuthenticator.RefreshAccessList(accessControl);

            // Add various system services (rather than the custom ones)
            services
                .AddCors()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Set up the Api versioning
            services
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddApiVersioning(o =>
                {
                    o.ApiVersionReader = new HeaderApiVersionReader("api-version");
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.ReportApiVersions = true;
                });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                //options.IncludeXmlComments(XmlCommentsFilePath);
            });

            // Custom service setup for the API Manager
            services
                .AddLogging()
                .AddCustomAuthentication(userAuthenticator);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Salesforce Outbound API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Salesforce integration with Swagger, Swashbuckle, Input Formatting and API versioning.",
                Contact = new Contact() { Name = "Joe Walters", Email = "info@thenakeddeveloper.com" },
                TermsOfService = "Owned By TNDStudios",
                License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " <b color=\"red\">This API version has been deprecated</b>.";
            }

            return info;
        }
    }
}

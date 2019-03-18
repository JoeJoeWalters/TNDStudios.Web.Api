using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TNDStudios.Web.ApiManager;
using TNDStudios.Web.ApiManager.Data.Soap;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace TNDStudios.Web.Api
{
    public class Startup
    {
        /// <summary>
        /// Connection Strings from appsettings.json
        /// </summary>
        public static String CosmosDB = String.Empty;

        /// <summary>
        /// Salesforce Org Ids allowed to use the services
        /// </summary>
        public static List<string> AllowedOrgIds = new List<string>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Set the global connection strings
            CosmosDB = Configuration.GetConnectionString("CosmosDB") ?? String.Empty;

            // Get the allowed salesforce org id's
            AllowedOrgIds = Configuration.GetSection("AllowedOrgIds")
                                .AsEnumerable()
                                .Where(orgId => orgId.Value != null)
                                .Select(orgId => orgId.Value)
                                .ToList<String>();
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
            // Set up CORS to allow cross domain writing to the logging service
            services
                .AddLogging()
                .AddCors(options =>
                    {
                        options.AddPolicy("CORSPolicy",
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:60972",
                                                "https://localhost:44341");
                        });
                    })
                .AddMvc(options =>
                    {
                        // Add Custom Soap Envelope validation
                        options.InputFormatters.Add(new SoapFormatter());
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Custom service setup for the API Manager
            services
                .AddCustomAuthentication(userAuthenticator)
                .AddCustomVersioning();
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

            // Enable custom versioning
            app.UseCustomVersioning(provider);
        }
    }
}

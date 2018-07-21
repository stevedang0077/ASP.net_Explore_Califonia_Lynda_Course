using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExploreCalifonia
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            // Config to turn on and off the dev mode:
            configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(env.ContentRootPath + "/config.json")
                                    .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                                    .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>();
            // we can create the new instance like this, just an example.
            // But this is the same thing that ASP.net Core did already, so no need. 
            // services.AddTransient<FeatureToggles>(x => new FeatureToggles());

            // Now we can reference the config in the Startup method above
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                // Now we can use the FeatureToggles Class
                EnableDeveloperExceptions =
                configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middleware in the pipeline ;)
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            FeatureToggles feature
            )
        //^^ NOTE: injected the new class into this method, then use it.
        // This is similar to Angular services, where we inject the services into the constructor.
        {
            // This will listen and get fire on any error event that happened during the req,res
            // More user friendly, because they don't care about the stack trace, debugging the code
            // like the developers ^^
            app.UseExceptionHandler("/error.html");
            // NOTE^: we need to create an error.html file.

            // ================= Moved this into a Startup method above =====================
            // Config to turn on and off the dev mode:
            //var configuration = new ConfigurationBuilder()
            //                        .AddEnvironmentVariables()
            //                        .AddJsonFile(env.ContentRootPath + "/config.json")
            //                        .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
            //                        .Build();
            // ^^ add the JsonFile to read the config from a json file,
            // make sure to give it a path tho.
            // ================= Moved this into a Startup method above =====================


            // Everything in here will be executed by order, if a logic is match, then it will get executed,
            // and ignore others below it.

            // This will return null in the condition because we have not created the variable 
            // EnableDeveloperException yet. If we run this, the browser will skip the dev mode
            // and run the error.html file.
            //if (configuration["EnableDeveloperException"] == "True"){
            //    app.UseDeveloperExceptionPage();
            //}

            // Get the boolean value from the variable in Properties,
            // return false in condition if the var isn't there
            //if (configuration.GetValue<bool>("EnableDeveloperException"))
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            // Commented out to test the new service
            //if (configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions"))
            if (feature.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }

            // This is just an example for an invalid url localhost:4200/invalid
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("Error!!!!!");
                await next();
            });

            app.UseFileServer();
        }
    }
}

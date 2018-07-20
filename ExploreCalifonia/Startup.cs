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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middleware in the pipeline ;)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This will listen and get fire on any error event that happened during the req,res
            // More user friendly, because they don't care about the stack trace, debugging the code
            // like the developers ^^
            app.UseExceptionHandler("/error.html");
            // NOTE^: we need to create an error.html file.

            // Config to turn on and off the dev mode:
            var configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(env.ContentRootPath + "/config.json")
                                    .Build();
            // ^^ add the JsonFile to read the config from a json file,
            // make sure to give it a path tho.

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

            if (configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions"))
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

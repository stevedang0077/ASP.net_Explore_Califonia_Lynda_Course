## Ch1. The basics:
### Reponse to HTTP requests:
File Name: Startup.cs
```
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middleware in the pipeline ;)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Everything in here will be executed by order, if a logic is match, then it will get executed,
            // and ignore others below it.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.Use takes in two params: 1st: request, 2nd: next middle ware
            // makesure to add await next(); so the compiler know when to go to the next method/logic/function
            app.Use(async (context, next) =>
            {
                // how to make it run this method only on some url/path?
                // if the url matched, the content will get rendered, otherwise no Gucci ^^
                if (context.Request.Path.StartsWithSegments("/hello"))
                {
                    await context.Response.WriteAsync("hello path matched!! Hello from Starbucks! ");
                }
                await next();
            });

            // Don't use this app.Run (run like hell away from this :P, use app.Use instead).
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("I'm in Starbucks! The bagel smells soooo good! <3");
            });
        }
```
### Nuget Leverage External Dependencies:
```
Right click on Solution/Manage NuGet Packages for Solution.
StaticFiles - MS
OR:
app.UseFileServer();//<== this function/method is in StaticFiles NuGet Package
right click on this/Quick Actions and Refactorings/Add Package.
```

### Static Files:
```
Path to files: Ex_Files_ASPNET_CoreMVC\Exercise Files\Ch01\artifacts\site
Copy & Paste these files into wwwroot folder.
```
Startup.cs File:
```
    Delete the middlewares:
    // app.Use takes in two params: 1st: request, 2nd: next middle ware
            // makesure to add await next(); so the compiler know when to go to the next method/logic/function
            app.Use(async (context, next) =>
            {
                // how to make it run this method only on some url/path?
                // if the url matched, the content will get rendered, otherwise no Gucci ^^
                if (context.Request.Path.StartsWithSegments("/hello"))
                {
                    await context.Response.WriteAsync("hello path matched!! Hello from Starbucks! ");
                }
                await next();
            });

            // Don't use this app.Run (run like hell away from this :P, use app.Use instead).
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("I'm in Starbucks! The bagel smells soooo good! <3");
            });
```
Use: app.UseFileServer();
```
This middleware will register the static files in the wwwroot folder.
```
### Error Handling and Diagnostics:
Startup.cs File:
```
Add this code below to check if the link url has something like this:
https://localhost:44335/invalid then the error message will be rendered:

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("invalid"))
        throw new Exception("Error!!!!!");
    await next();
});
```
There are 4 tabs Stack/Query/Cookies/Headers that are very useful to check the code.
These came from the code below that MS ASP.net included for developers:
```
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
```
For users:
```
// This will listen and get fire on any error event that happened during the req,res
// More user friendly, because they don't care about the stack trace, debugging the code
// like the developers ^^
app.UseExceptionHandler("/error.html");
// NOTE^: we need to create an error.html file.
```
In order to see the error in production mode, disable dev environment:
```
Right click:
Project's name/Properties/Debug/ASPNETCORE_ENVIRONMENT
Set the value to other thing ie: "Production" instead for "Development",
then the application will no longer be in dev mode.

Navigate back to the web and refresh to see the custom error page:
localhost:4200/invalid - Ta da!

To get back to dev mode, just change its value back to "Development" from "Production".
```
### Custom Configuration Library - Dependency (Built in from MS):
Create an instance of the ConfigurationBuilder object:
NOTE^: this one is from config library, bring it in to use it:
```
using Microsoft.Extensions.Configuration;

// Config to turn on and off the dev mode:
var configuration = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .Build();

// Everything in here will be executed by order, if a logic is match, then it will get executed,
// and ignore others below it.

// This will return null in the condition because we have not created the variable 
// EnableDeveloperException yet. If we run this, the browser will skip the dev mode
// and run the error.html file.
if (configuration["EnableDeveloperException"] == "True")
{
    app.UseDeveloperExceptionPage();
}
```
If we add the EnableDeveloperException to True in the Project/Properties then the 
custom error.html page won't be there and the stack trace for developers will show up.

Another way to do this if statement is:
```
// Get the boolean value from the variable in Properties,
// return false in condition if the var isn't there
if (configuration.GetValue<bool>("EnableDeveloperException"))
{
    app.UseDeveloperExceptionPage();
}
```


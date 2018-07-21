## Ch1. The basics:
Short cut and tricks:
```
Create a constructor:
    ctor -> tab twice
    public Startup()
    { // more code here ... }

Quick actions / refactorings:
    Ctrl + .


```

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
### Populate Config Setting - from json file:
To read the json config file, add the AddJsonFile() method and the path to the config.json
Also, we need to create the config.json or it'll throw an exception. Like below:
```
// Config to turn on and off the dev mode:
var configuration = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile(env.ContentRootPath + "/config.json")
                        .Build();
// ^^ add the JsonFile to read the config from a json file,
// make sure to give it a path tho.
```
To create an json file, right click:
```
Project's name/Add/New Item(Ctrl Shjft A)
Choose data tab/json
Give it a name. Finish.

{
  "EnableDeveloperExceptions": true,
}
```
The config can also read the crazy json file:
```
{
  "FeatureToggles": {
    "EnableDeveloperExceptions": true
  }
}
```
To access the value in this object, it uses ":" instead for a "."
```
if (configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions"))
{
    app.UseDeveloperExceptionPage();
}
```
To add an optional config file for all the development process, simply add one more method,
the 2nd param is optional, which why it's set to true.
No file like this? no problem because this is just an optional file. (No exception at all)
```
var configuration = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile(env.ContentRootPath + "/config.json")
                        .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                        .Build();
Create the json.development.json file, then remove the EnableDeveloperExceptions in the config.json file.
{
  "FeatureToggles": {
  }
}
Only leave this option in the new file.
{
  "FeatureToggles": {
    "EnableDeveloperExceptions": true
  }
}
```
The Dev Mode still working!! 

### Increase Maintainability with Dependency Injection:
This code below can be converted into a module that do one thing well,
so the next step is to convert this into a module, and then inject it into our project.
Reason?: Because the config is depending on this configuration object, for code efficient
why not put this into a separated module? ;)
```
// Config to turn on and off the dev mode:
var configuration = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile(env.ContentRootPath + "/config.json")
                        .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
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
```
Project'name/Add/Class -> Name: FeatureToggles.cs
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifonia
{
    public class FeatureToggles
    {
        // Added this line, then add another parameter in the Startup.cs File.
        public bool EnableDeveloperExceptions { get; set; }
    }
}
```
Add another param for the public void Configure(IApplicationBuilder app, IHostingEnvironment env):
```
public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            FeatureToggles feature
        )
//^^ NOTE: injected the new class into this method, then use it.
// This is similar to Angular services, where we inject the services into the constructor.
{ // More code here ... }
```
Not ready yet to run, we're only 1/2 way done.
ASP.net Core will throw an exception if we run the code now. just FYI :D
Next step:
1.  Startup.cs/ConfigureServices method:

```
// This method gets called by the runtime. Use this method to add services to the container.
// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
public void ConfigureServices(IServiceCollection services)
{
    // Add services here ... ^^
}
```
Services: Out of all helpful methods, the AddScoped, AddSingleton, and AddTransient tell ASP.net to create instances of types when they are requested. 
```
Diff. between these are how long the lifespand.
1. Transient:
    Shortest lifespan: An instance will be created every time one is requested.
        ie: if there are two requested the same config, ASP.net will create two instances, no sharing.
2. Scoped:
    A single instance will be created for each "scope". In ASP.net Core MVC, the "scope" is almost always the current web request.
        ie: Allow to share states between diff. components through out the same request w/o worring about a diff request from the user gaining access to that request.
3. Singleton:
    A single instance will be created for the entire application.
        ie: share data cross requests. Use for common data to share.

=> In this project, we will use the Transient.
```
Add services trasient to the ConfigureServies method:
```
// This method gets called by the runtime. Use this method to add services to the container.
// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<FeatureToggles>();
}
```
Run the application and Woa-la!! It's running, but we're not done.
```
ctor - Tab twice to create a constructor.
// Config to turn on and off the dev mode:
configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/config.json")
                .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                .Build();

Ctrl + . on configuration to get a quick fix.
private readonly IConfigurationRoot configuration;

Add public Startup(IHostingEnvironment env)
```
The Startup method will look like this:
```
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
```
Then we can reference the config in the ConfigureServices method, the featureToggles class:
```
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
        // ^ THIS IS A DEPENDENCY INJECTED!!
        EnableDeveloperExceptions =
        configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")

    });
}
```
Put a break point here to see the FeatureToggles in action: value changed to true.
```
if (feature.EnableDeveloperExceptions)
{
    app.UseDeveloperExceptionPage();
}
```
o Low-level APIs
o Simple Logic 
o Static Files

### DONE WITH CHAPTER 1! TIME FOR FOOD.

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
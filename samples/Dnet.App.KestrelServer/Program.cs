var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5001, x => x.Protocols = HttpProtocols.Http1AndHttp2AndHttp3);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.MapWhen(ctx => ctx.Request.Host.Port == 5001, first =>
//{
//    first.Use((ctx, nxt) =>
//    {
//        ctx.Request.Path = "/Wasm" + ctx.Request.Path;
//        return nxt();
//    });

//    first.UseBlazorFrameworkFiles("/Wasm");

//    first.UseStaticFiles();

//    first.UseStaticFiles("/Wasm");

//    first.UseRouting();

//    first.UseEndpoints(endpoints =>
//    {
//        endpoints.MapControllers();
//        endpoints.MapFallbackToFile("/Wasm/{*path:nonfile}", "Wasm/index.html");
//    });
//});

app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();

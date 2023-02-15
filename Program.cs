using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using pricheson.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//CreateHostBuilder(args).Build().Run();

// IHostBuilder CreateHostBuilder(string[] args)
//{

//var certificate = new X509Certificate2(
//@"C:\Users\тимур\source\repos\pricheson\pricheson\Views\Shared\pricheson.pfx",
//"teamrakov16"
//);


//#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
//return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
//{
//    builder.UseKestrel(options =>
//    {
//        options.Listen(System.Net.IPAddress.Loopback, 44321, listenOptions =>
//        {
//            var connectionOptions = new HttpsConnectionAdapterOptions();
//            connectionOptions.ServerCertificate = certificate;

//            listenOptions.UseHttps(connectionOptions);
//        });
//    }).UseStartup();
//});

//#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
//}


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Dnet.App.ServerSide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                     .ConfigureKestrel(serverOptions =>
                     {
                         var pfxFilePath = "securitysuite.pfx";
                         var pfxPassword = "coms@123";

                         serverOptions.Listen(IPAddress.Any, 5000);
                         serverOptions.Listen(IPAddress.Any, 5001, listenOptions =>
                         {
                             // Enable support for HTTP1 and HTTP2 (required if you want to host gRPC endpoints)
                             listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                             // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
                             listenOptions.UseHttps(pfxFilePath, pfxPassword);
                         });
                     });
                });
    }
}

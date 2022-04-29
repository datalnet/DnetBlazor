using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Dnet.Blazor.Infrastructure.Services;
using Dnet.App.Shared.Infrastructure.Services;

namespace Dnet.App.ClientSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("WebHostURL", client => client.BaseAddress = new Uri(builder.Configuration["WebHostURL"]));

            builder.Services.AddDnetBlazor();

            builder.Services.AddScoped(typeof(IApplicationServiceService), typeof(ApplicationServiceService));

            await builder.Build().RunAsync();
        }
    }
}

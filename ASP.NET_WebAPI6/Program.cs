using DemoRestSimonas.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ASP.NET_WebAPI6.Entities;
using DagraAPI;
using Microsoft.AspNetCore;

namespace ASP.NET_WebAPI6
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //var host = CreateHostBuilder(args).Build();

            //using var scope = host.Services.CreateScope();
            //var dbSeeder = (DatabaseSeeder)scope.ServiceProvider.GetService(typeof(DatabaseSeeder));
            //await dbSeeder.SeedAsync();

            //await host.RunAsync();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });




        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}

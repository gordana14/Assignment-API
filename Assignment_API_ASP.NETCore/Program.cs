using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Assignment_API_ASP.NETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args)
              .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 var env = hostingContext.HostingEnvironment;

                 config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       // .AddConfiguration(, optinal: true , r)
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true) // optional extra provider
                       .SetBasePath(Directory.GetCurrentDirectory());
                 if (env.IsDevelopment()) // different providers in dev
                 {
                     var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                     if (appAssembly != null)
                     {
                         config.AddUserSecrets(appAssembly, optional: true);
                     }
                 }

                 config.AddEnvironmentVariables(); // overwrites previous values

                 if (args != null)
                 {
                     config.AddCommandLine(args);
                 }
             })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build().Run();



        }

        private static object BuildWebHost(string[] args)
        {
            throw new NotImplementedException();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
    }
}

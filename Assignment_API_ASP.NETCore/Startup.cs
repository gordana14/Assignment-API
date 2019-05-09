using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore;
using Assignment_API_ASP.NETCore.Controllers;
using Assignment_API_ASP.NETCore.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Assignment_API_ASP.NETCore
{
    public class Startup
    {



    // This method gets called by the runtime. Use this method to add services to the container.
       public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Set up dependency injection for controller's logger
            // Register using generic target type and factory function
            services.AddTransient<StreamContext>((s) =>
            {
                return new StreamContext();
            });
           


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
          
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "My assignment - API.NET Core"

                });
                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Assignment_API_ASP.NETCore.xml";
               // c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

            });
            services.Configure<IISOptions>(options => {
                options.AutomaticAuthentication = false;
                //options.ForwardClientCertificate = false;
               });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           // ConnectionString = Configuration["ConnectionStrings:StreamDatabase"];
            
            if (env.IsDevelopment())
            {
          
                app.UseDeveloperExceptionPage();
            }
            else
            {
         
                app.UseDeveloperExceptionPage();
                 app.UseDatabaseErrorPage();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API"); });

            //app.UseMvcWithDefaultRoute();
             app.UseMvc();

        }
    }
}

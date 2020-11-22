using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Products.Queries;
using Products.CommandValidators;
using Products.Commands;

namespace Products
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            })
            .AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Products API",
                    Description = "A Simple dotnetcore Products API",
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // If you want to set up a controller for, say, property injection
            // you can override the controller registration after populating services.
            // var loggerFactory = new LoggerFactory();
            // builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();
            // builder.RegisterGeneric(typeof(Logger<>))
            //     .As(typeof(ILogger<>))
            //     .SingleInstance();

            builder.RegisterType<ProductOptionsQuery>()
                .As<IProductOptionsQuery>()
                .SingleInstance();
            builder.RegisterType<ProductsQuery>()
                .As<IProductsQuery>()
                .SingleInstance();

            builder.RegisterType<ProductsCommandValidator>()
                .As<IProductsCommandValidator>()
                .SingleInstance();

            builder.RegisterType<ProductOptionsCommandValidator>()
                .As<IProductOptionsCommandValidator>()
                .SingleInstance();

            builder.RegisterType<ProductsCommand>()
                .As<IProductsCommand>()
                .SingleInstance();

            builder.RegisterType<ProductOptionsCommand>()
                .As<IProductOptionsCommand>()
                .SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
            });

            app.UseHttpsRedirection();

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseMvc();
        }
    }
}
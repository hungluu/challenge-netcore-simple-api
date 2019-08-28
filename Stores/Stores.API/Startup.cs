using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Stores.Infrastructure.Context;
using Stores.Infrastructure.Repositories;
using Stores.Business.Services;
using AutoMapper;
using Stores.Domain.Models;
using Stores.Business.ViewModels;

namespace Stores.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var factory = new StorePackageContextFactory();

                return factory.CreateDbContext(new string[0]);
            });

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IStorePackageService, StorePackageService>();

            //Config Automapper map
            Mapper.Initialize(config =>
            {
                config.CreateMap<StoreProductViewModel, StoreProduct>().ReverseMap();
                config.CreateMap<StorePackageViewModel, StorePackage>().ReverseMap();
            });


            ConfigureSwaggerService(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            RunMigration(app);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HmsOrient Booking API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<StorePackageContext>().Database.Migrate();
            }
        }

        /// <summary>
        /// Config Swagger Service
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "HmsOrient Booking API",
                    Description = "HmsOrient Booking API (ASP.NET Core)",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "OrientSoftware Admin", Email = "admin@orientsoftware.net", Url = "" },
                });

                c.DescribeAllParametersInCamelCase();
            });
        }
    }
}

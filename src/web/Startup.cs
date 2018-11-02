using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using vlaaienslag.Models;
using vlaaienslag.Application.Interfaces;
using vlaaienslag.Application.Services;
using vlaaienslag.Infrastructure.Data;

namespace vlaaienslag
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
            ConfigureCommonServices(services);

            System.Console.WriteLine(Configuration.GetConnectionString("DBConnection"));
            System.Console.WriteLine(Configuration.GetConnectionString("defaultConnection"));
            services.AddDbContext<DataStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
            services.BuildServiceProvider().GetService<DataStoreContext>().Database.Migrate();
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            services.AddDbContext<DataStoreContext>(options => options.UseInMemoryDatabase("myDatabase"));
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderService, OrderService>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}

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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

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
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<DataStoreContext>();
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            services.AddDbContext<DataStoreContext>(options => options.UseInMemoryDatabase("myDatabase"));
            
            //services.AddIdentity<IdentityUser, IdentityRole>()
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DataStoreContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddAuthorization( options =>
                {
                    options.AddPolicy("RequiresAdministration", policy => policy.RequireRole("oc"));
                });
            
            services.AddMvc()
                .AddRazorPagesOptions( options => 
                {
                    options.Conventions.AuthorizeFolder("/Products", "RequiresAdministration");
                });

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderService, OrderService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            CreateRoles(services).Wait();
//            CreateSuperUser(services).Wait();

            if (env.IsDevelopment())
            {
                CreateDefaultUsers(services).Wait();
            }

        }

        private async Task CreateRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            
            string[] requiredRoles = {"user" , "oc", "administrator"};

            foreach (var requiredRole in requiredRoles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(requiredRole);
                if (!roleExists)
                    {
                        var newRole = new IdentityRole(requiredRole);
                        await roleManager.CreateAsync(newRole);
                    }
            }
        }

        private async Task CreateDefaultUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            //add verkoper 1
            var baseUser = new IdentityUser("verkoper@bc.com");
            baseUser.Email = "verkoper@bc.com";

            var baseUserResult = await userManager.CreateAsync(baseUser, "Verkoper#123");
            if (baseUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(baseUser, "user");
            }

            //add oc-1 = oc
            var managementUser = new IdentityUser("ouder@anton.com");
            managementUser.Email = "ouder@anton.com";

            var managementUserResult = await userManager.CreateAsync(managementUser, "Ouder#123");
            if (managementUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(managementUser, "oc");
            }

            //add administrator
            var administrationUser = new IdentityUser("admin@company.com");
            managementUser.Email = "admin@company.com";

            var administrationUserResult = await userManager.CreateAsync(administrationUser, "Admin#123");
            if (administrationUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(administrationUser, "administrator");
            }

        }
    }
}

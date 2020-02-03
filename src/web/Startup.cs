using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Strover.Models;
using Strover.Application;
using Strover.Application.Interfaces;
using Strover.Application.Services;
using Strover.Infrastructure.Data;
using Strover.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Strover
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

            services.AddDbContext<DataStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddIdentity<Strover.Infrastructure.Data.SalesPerson, IdentityRole>()
                .AddEntityFrameworkStores<DataStoreContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //don't impose strange rules on passwords
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                //instead impose long passwords
                options.Password.RequiredLength = 8;
            });
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            services.AddDbContext<DataStoreContext>(options => options.UseInMemoryDatabase("myDatabase"));

            services.AddIdentity<Strover.Infrastructure.Data.SalesPerson, IdentityRole>()
                .AddEntityFrameworkStores<DataStoreContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
               {
                   options.AddPolicy("RequiresAdministration", policy => policy.RequireRole(ApplicationRole.Administrator));
               });

            services.AddLocalization(localizationOptions => localizationOptions.ResourcesPath = "Resources");

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
               {
                   options.Conventions.AuthorizePage("/Index");
                   options.Conventions.AuthorizeFolder("/Orders");
                   options.Conventions.AuthorizeFolder("/Payments");
                   options.Conventions.AuthorizePage("/Payments/Index", "RequiresAdministration");
                   options.Conventions.AuthorizePage("/Payments/Paid", "RequiresAdministration");
                   options.Conventions.AuthorizeFolder("/Products", "RequiresAdministration");
               })
               .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();


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

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("nl")
                    };

                options.DefaultRequestCulture = new RequestCulture("nl");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddTransient<IEmailSender, SendGridMailSender>();

            services.Configure<ShopOptions>(Configuration.GetSection("ShopConfiguration"));

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
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
            app.UseRequestLocalization(options =>
           {
               var supportedCultures = new List<CultureInfo>
                   {
                        new CultureInfo("en-US"),
                        new CultureInfo("nl")
                   };

               options.DefaultRequestCulture = new RequestCulture("nl");
               options.SupportedCultures = supportedCultures;
               options.SupportedUICultures = supportedCultures;
           });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            CreateRoles(services).Wait();

            if (env.IsDevelopment())
            {
                CreateDefaultUsers(services).Wait();
            }

        }

        private async Task CreateRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] requiredRoles = { "user", "oc", "administrator" };

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
            var userManager = services.GetRequiredService<UserManager<SalesPerson>>();

            //add verkoper 1
            var baseUser = new SalesPerson("verkoper@bc.com");
            baseUser.Email = "verkoper@bc.com";

            var baseUserResult = await userManager.CreateAsync(baseUser, "Verkoper#123");
            if (baseUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(baseUser, ApplicationRole.Seller);
            }

            //add oc-1 = oc
            var managementUser = new SalesPerson("ouder@anton.com");
            managementUser.Email = "ouder@anton.com";

            var managementUserResult = await userManager.CreateAsync(managementUser, "Ouder#123");
            if (managementUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(managementUser, ApplicationRole.Administrator);
            }

            //add administrator
            var administrationUser = new SalesPerson("admin@company.com");
            managementUser.Email = "admin@company.com";

            var administrationUserResult = await userManager.CreateAsync(administrationUser, "Admin#123");
            if (administrationUserResult.Succeeded)
            {
                var result = await userManager.AddToRoleAsync(administrationUser, "administrator");
            }

        }
    }
}

using Demo_03.BLL.Interfaces;
using Demo_03.BLL.Repositores;
using Demo_03.DAL.Contexts;
using Demo_03.DAL.Models;
using Demo_03_.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;


namespace Demo_03_.PL
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
            services.AddControllersWithViews();

            services.AddAutoMapper(typeof(Startup)); // ????? AutoMapper
                                                     // ???? ???????

            // To Allow Dependancy Injection & // Another way for Connection string
            services.AddDbContext<MvcProject_01_DbContext>(options =>
            {
                // Make Connection string More Dynamic based on Environment
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            });

            // To Allow Dependancy Injection

            services.AddScoped<IDepartementRepository, DepartementRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // To Allow Dependancy Injection

            services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            /*   services.AddAutoMapper(typeof(Startup))*/
            ;  // Add this in Startup.cs under ConfigureServices

            services.AddAutoMapper(M => M.AddProfile(new DepartemenetProfile()));

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true;   // @ # $
                Options.Password.RequireDigit = true;             // 0 1 2
                Options.Password.RequireLowercase = true;         // a b c
                Options.Password.RequireUppercase = true;         // A B C

                // P@ssw0rd  
                // Pa$$w0rd
            })
                    .AddEntityFrameworkStores<MvcProject_01_DbContext>()
                    .AddDefaultTokenProviders(); // Add Token


            ///services.AddScoped<UserManager<ApplicationUser>>();
            ///services.AddScoped<SignInManager<ApplicationUser>>();
            ///services.AddScoped<RoleManager<ApplicationUser>>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            // CookieAuthenticationDefaults => Encyrpt for Token
            {
                Options.LoginPath = "Account/Login";
                Options.AccessDeniedPath = "Home/Error";
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                  //pattern: "{controller=Home}/{action=Index}/{id?}");
                 // pattern: "{controller=Account}/{action=Register}/{id?}");
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}

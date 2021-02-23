using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PROG20_ASPNET1_Inlämningsuppgift2.Models;
using PROG20_ASPNET1_Inlämningsuppgift2.ModelsIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG20_ASPNET1_Inlämningsuppgift2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddDistributedMemoryCache();
            services.AddSession();

            //Connectionsträngen bör egentligen ligga I en config fil
            //data
            var conn = @"Server=localhost;Database=Tomasos;Trusted_Connection=True; ConnectRetryCount=0";
            services.AddDbContext<TomasosContext>(options => options.UseSqlServer(conn));

            //identity
            var conn2 = @"Server=localhost;Database=TomasosIdentity;Integrated Security=true;";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn2));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "Default",
                template: "{controller=Home}/{action=Index}"
                    );

            });
        }
    }
}

using CovidDataWarehouse.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataWarehouse
{
    public class Startup
    {
        private string DEFAULT_DATABASE_CONNECTION_STRING_NAME = "Server=tcp:covid-server-comp4522.database.windows.net,1433;Initial Catalog=covid-database;Persist Security Info=False;User ID=SuperCoolManager;Password=VeryMuchASecretPassword!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private string DEFAULT_DATA_WAREHOUSE_CONNECTION_STRING_NAME = "Server=tcp:covid-server-comp4522.database.windows.net,1433;Initial Catalog=covid-data-warehouse;Persist Security Info=False;User ID=SuperCoolManager;Password=VeryMuchASecretPassword!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<CovidDatabaseContext>(options => options.UseSqlServer(DEFAULT_DATABASE_CONNECTION_STRING_NAME));
            services.AddDbContext<CovidDataWarehouseContext>(options => options.UseSqlServer(DEFAULT_DATA_WAREHOUSE_CONNECTION_STRING_NAME));
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

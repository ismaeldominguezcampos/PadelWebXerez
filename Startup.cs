using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelWebXerez
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
            //Iniciar el contexto de la base de datos   
            IniDbContext(services);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void IniDbContext(IServiceCollection services)
        {
            // Obtener la cadena de conexión
            string Hostname = Configuration["ConnectionStrings:Hostname"];
            string Database = Configuration["ConnectionStrings:Database"];
            string User = Configuration["ConnectionStrings:User"];
            string Password = Configuration["ConnectionStrings:Password"];
            string gestorBd = Configuration["ConnectionStrings:GestorBd"];

            ConnectionStrings connectionStrings = new ConnectionStrings(gestorBd);
            connectionStrings.Hostname = Hostname;
            connectionStrings.Database = Database;
            connectionStrings.User = User;

            connectionStrings.Password = Password;

            //Esta opción es usada de momento para poder usar los servicios de migración 
            services.AddDbContext<PadelWebXerezContext>(optionsBuilder => optionsBuilder
                    .UseSqlServer(connectionStrings.GetConnectionString(),
                    options => options
                        .EnableRetryOnFailure())
                        .EnableSensitiveDataLogging(true));
        }
    }
}

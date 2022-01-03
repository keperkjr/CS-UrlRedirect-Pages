using CS_UrlRedirect_Pages.Data;
using CS_UrlRedirect_Pages.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS_UrlRedirect_Pages
{
    public class Startup
    {
        private string _contentRootPath = "";

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRedirectService, RedirectService>();

            string connection = UpdateConnectionPath(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<DatabaseDBContext>(options => options.UseSqlServer(connection));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddRazorPages();
        }

        private string UpdateConnectionPath(string connection)
        {
            if (connection.Contains("%CONTENTROOTPATH%"))
            {
                connection = connection.Replace("%CONTENTROOTPATH%", _contentRootPath);
            }
            return connection;
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

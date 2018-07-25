using System.IO;
using ArnimalService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace ArnimalService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                   {
                       builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                .       AllowCredentials();
                    });
            });
            services.AddDbContext<AnimalContext>(opt => opt.UseInMemoryDatabase("AnimalList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "static", "images");
            if (Directory.Exists(imgFolder))
            {
                Directory.Delete(imgFolder, recursive: true);
            }

            Directory.CreateDirectory(imgFolder);

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "static")),
                RequestPath = "/api/animal"
            });
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}

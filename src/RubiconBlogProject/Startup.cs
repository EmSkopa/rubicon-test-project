using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rubicon.Contexts;
using Rubicon.SeedingData;
using Rubicon.Services.BlogService;
using Rubicon.Services.TagService;

namespace RubiconBlogProject
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
            services.AddControllers();
            
            var connectionString = Environment.GetEnvironmentVariable("RubiconDBConnection") ??
                                    Configuration.GetConnectionString("RubiconDBConnection");
            services.AddDbContext<RubiconDBContext>(opt => opt.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

            services.AddScoped<IBlogService, BlogService>();

            services.AddScoped<ITagService, TagService>();

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedingData.SeedInitiallyDataInDb(app);
        }
    }
}

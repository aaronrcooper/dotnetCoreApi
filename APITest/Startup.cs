using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using APITest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Swashbuckle.AspNetCore.Swagger;

namespace APITest
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
            string connectionString = Configuration.GetConnectionString("TestDb");
            services.AddDbContext<TodoContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Swagger generation
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Test API", Version = "v1"});
                });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

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
                app.UseHsts();
            }

            // Swagger setup
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

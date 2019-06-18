using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authorization;
using APITest.Business.Authorization.Rules;
using APITest.Business.Authorization.Handlers;
using APITest.Domain;
using AutoMapper;

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

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opts =>
                    {
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    });

            //Swagger generation
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Test API", Version = "v1" });
                    c.CustomSchemaIds(operation => operation.FullName);
                });

            ConfigureOAuth(services);
            ConfigureAuth(services);
            ConfigureAutoMapper(services);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        public void ConfigureAutoMapper(IServiceCollection services)
        {
            // Configure automapper using the assemblies present in the application
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TodoContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            context.Database.Migrate();

            // Swagger setup
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                c.RoutePrefix = "swagger";
                c.DisplayOperationId();
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public void ConfigureOAuth(IServiceCollection services)
        {
            var issuer = Configuration.GetSection("jwt").GetSection("issuer").Value;
            var audience = Configuration.GetSection("jwt").GetSection("audience").Value;
            var secret = Configuration.GetSection("jwt").GetSection("secret").Value;

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(auth =>
            {
                // Save the token that is submitted
                auth.SaveToken = true;

                auth.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = issuer,
                    ValidAudience =  audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });
        }

        public void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                // Add a policy that requires the user attempting to perform the operation to be the current user
                opts.AddPolicy("IsCurrentUser", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.Requirements.Add(new CurrentUserRequirement());
                });

                opts.AddPolicy("IsLoggedIn", policy => policy.RequireClaim(JwtRegisteredClaimNames.Jti));
            });

            // Add the handler for the current user policy as a singleton
            services.AddSingleton<IAuthorizationHandler, CurrentUserHandler>();
        }
    }
}

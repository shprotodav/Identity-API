using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using TestTask.Identity.DAL.Entities;
using TestTask.Identity.DB.Schema;
using TestTask.Identity.Services.Filters;

namespace TestTask.Identity.Services
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public string Environment { get; }
        public IContainer Container { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            Environment = env.EnvironmentName;
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetSection("AppSettings:ConnectionString").Value;

            IdentityBuilder builder = services.AddIdentityCore<User>();

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddDefaultTokenProviders();

            Func<string, SqlConnection> getConnection = x =>
            {
                switch (x)
                {
                    case "dev": return new SqlConnection(connectionString);
                    case "test": return new SqlConnection(connectionString);
                    case "prod": return new SqlConnection(connectionString);
                    default: return new SqlConnection(connectionString);
                }
            };

            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(getConnection(Environment));
            });

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("common", configurePolicy =>
                configurePolicy.AllowAnyHeader()
                               .AllowAnyMethod()
                               .WithOrigins("http://localhost:4200")
                               .AllowCredentials());
            });

            DependencyResolver.Resolve(services, Configuration);

            services.AddMvc(option => option.EnableEndpointRouting = false);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Identity API",
                    Description = "Controller \"Account\" contains requests to authorization of users",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Vladyslav Slipchenko", Email = "shprotodav@gmail.com" }
                });
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments("TestTask.Identity.Services.xml");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseCors("common");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(name: "default", pattern: "api/{controller}/{action}");
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API V1");
            });
        }
    }
}

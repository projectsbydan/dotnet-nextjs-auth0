using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using dotnet_api_auth0.Configurations;
using System.Linq;

namespace dotnet_api_auth0
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Auth0Configuration>(Configuration.GetSection("AUTH0"));
            services.AddControllers();

            services.AddAuthentication(options =>
                {
                    // default JWT Settings
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    // the Auth0 Domain
                    options.Authority = "https://dev-anba2oqm.eu.auth0.com/";
                    // the configured Audience (for the the API not the Web App)
                    options.Audience = "http://localhost:5000/";
                });

            // add cors so that we cann call localhost:5000 from localhost:3000
            services.AddCors(options =>
            {
                options.AddPolicy("cors", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseCors("cors");
            app.UseAuthentication(); // use the Bearer Authentfication
            app.UseAuthorization(); // use the Authorization from Auth0
            app.UseEndpoints(config =>
            {
                config.MapControllers();
            });
        }
    }
}

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

namespace WebApi
{
    public class Startup
    {

      


        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string myAllowSpecificOrigin = "_myAllowSpecificOrigin";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Inject App settings to access appsettingJson Setting to access in controller.... ApplicationSettings class Inject 
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));



            services.AddCors(options =>
            {
                options.AddPolicy(myAllowSpecificOrigin,
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });


            services.AddControllers();

            services.AddDbContext<AuthenticationContext>(option =>
            option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //if we add some more Identity in Microsoft identity we need to make a class and inherit from it

            //for register new identity 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AuthenticationContext>();


            //To remove identity Validation 

            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 4;

            });

            //JWT ATHENTICATION 

            //secret key inside the appsetting.json
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience=false,
                    ClockSkew = TimeSpan.Zero
                };
            });

             


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          //LoggerFactory
          


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSerilogRequestLogging();//add this line

            app.UseRouting();

            app.UseCors(myAllowSpecificOrigin);
            app.UseAuthentication();



            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

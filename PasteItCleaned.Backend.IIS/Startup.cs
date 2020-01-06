using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Data;
using PasteItCleaned.Common.Helpers;
using PasteItCleaned.Common.Localization;

using System.Globalization;

namespace PasteItCleaned.IIS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ConfigHelper.SetConfigurationInstance(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Adds Amazon Cognito as Identity Provider
            //services.AddCognitoIdentity();
            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = "327610pt1kae96vu93g07g7apm"; // UserPoolClientId
                options.Authority = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_mb8JXKFWO"; // https://cognito-idp.<Region>.amazonaws.com/<UserPoolId>
            });*/

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.Authority = string.Format("https://cognito-idp.{0}.amazonaws.com/{1}", ConfigHelper.GetAppSetting("Amazon.Cognito.Region"), ConfigHelper.GetAppSetting("Amazon.Cognito.UserPoolId")); // https://cognito-idp.<Region>.amazonaws.com/<UserPoolId>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d1mscqs9mgjhgjhkghjkgmkch5m202pq8cm0er83kha3raa4unphivusgh9v84lfvd")),
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            services.AddLocalization();

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("fr")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<PasteItCleanedDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("PasteItCleaned.Backend.Data")));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStringLocalizerFactory factory)
        {
            T.SetStringLocalizerFactory(factory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLocalization();
            app.UseCors("Default");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

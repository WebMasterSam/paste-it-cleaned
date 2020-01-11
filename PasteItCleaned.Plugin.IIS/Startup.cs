using System.Globalization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using Swashbuckle.AspNetCore.Swagger;

using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Backend.Data;
using PasteItCleaned.Backend.Services;
using PasteItCleaned.Backend.Core.Middleware.Logging;
using PasteItCleaned.Plugin.Localization;

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
            services.AddTransient<IApiKeyService, ApiKeyService>();
            services.AddTransient<IDomainService, DomainService>();
            services.AddTransient<IErrorService, ErrorService>();
            services.AddTransient<IHitDailyService, HitDailyService>();
            services.AddTransient<IHitService, HitService>();

            services.AddDbContext<PasteItCleanedDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("PasteItCleaned.Backend.Data")));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "PasteItCleaned Plugin", Version = "v1" });
            });

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

            app.UseRequestResponseLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PasteItCleaned Plugin V1");
            });

            app.UseRequestLocalization();
            app.UseCors("Default");
            app.UseMvc();
        }
    }
}

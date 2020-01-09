using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Backend.Data;
using PasteItCleaned.Backend.Services;
using PasteItCleaned.Core.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using PasteItCleaned.Backend.Core.Middleware.Logging;

namespace PasteItCleaned.Aws.Lambda
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ConfigHelper.SetConfigurationInstance(configuration);
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
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

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseMvc();
            app.UseCors("Default");
        }
    }
}

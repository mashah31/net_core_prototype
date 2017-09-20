using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;

using ABC.NetCore.Models;
using ABC.NetCore.Filters;
using ABC.NetCore.Infrastructure;

using AutoMapper;

using ABC.NetCore.ProblemSolving.Models;
using ABC.NetCore.ProblemSolving.Infrastructures;
using ABC.NetCore.ProblemSolving.Services;

namespace ABC.NetCore.ProblemSolving
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly int? _httpsPort;
        private string[] _angularRoutes = new[] { "/home", "/about" };

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // Get https port (only in development)
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();

                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings.iisExpress:sslPort");
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.Configure<PagingOptions>(Configuration.GetSection("DefaultPagingOptions"));

            // Prepare policy for CORS Access
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

            services
                .AddDbContext<ProblemSolvingDBContext>(opt => {
                    // opt.UseInMemoryDatabase("ProblemSolvingDB"); // For quick development use in memory database .. 
                    opt.UseSqlServer(Configuration.GetConnectionString("ProblemSolvingDB"));
                })
                .AddDbContext<CentralDataDBContext>(opt => {
                    opt.UseSqlServer(Configuration.GetConnectionString("CentralSAPDataDB"));
                });

            // Add Framework Services.
            services.AddScoped<IComplaintCodeService, ComplaintCodeService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPlantService, PlantService>();
            services.AddScoped<IProblemSolvingTypeService, ProblemSolvingTypeService>();
            services.AddScoped<ISAPService, SAPService>();

            // Enable automapper for all the POJO
            services.AddAutoMapper();

            // Add response caching (server side caching)
            services.AddResponseCaching(opt =>
            {
                opt.MaximumBodySize = 1024;
            });

            // Like to use lowercase for our API's so enable it
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddMvc(opt =>
            {
                // Catch and format all of the un-handled exceptions
                opt.Filters.Add(typeof(JsonExceptionFilter));

                // Intercept all outgoing request and fromat link object
                opt.Filters.Add(typeof(LinkRewriteFilter));

                // Require HTTPS for all controllers
                // opt.SslPort = _httpsPort;
                // opt.Filters.Add(typeof(RequireHttpsAttribute));

                // Remove default json type and use ION format for response
                var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(jsonFormatter);
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));

                // Serverside cache profile
                opt.CacheProfiles.Add("ServerResponseCacheProfile", new CacheProfile
                {
                    Duration = 30,  // Duration of cached result
                    VaryByQueryKeys = new string[] {
                        "orderby", "search", "limit", "offset"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Set up of logging mechanism
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Use developer exception page for error
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ProblemSolvingDBContext context = serviceScope.ServiceProvider.GetService<ProblemSolvingDBContext>();
                context.Database.EnsureCreated();

                bool.TryParse(Configuration.GetSection("PrepareInitialDataDump").Value, out var prepareInitialDataDump);
                if (prepareInitialDataDump)
                {
                    SeedTestData(context);
                }
            }

            // Set up angular project routes
            // Make sure to add angular routes if you add more of them
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.HasValue && null != _angularRoutes.FirstOrDefault((ar) => context.Request.Path.Value.StartsWith(ar, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Request.Path = new PathString("/");
                }
                await next();
            });

            // force all traffic to use https instead of http
            // app.UseHsts(opt =>
            // {
            //     opt.MaxAge(days: 720);  //Set max age for remembering the HSTS settings (set it to year or so in production)
            //     opt.IncludeSubdomains();    //Include all domains to SSL traffic
            //     opt.Preload();  //Assums that client uses hsts if site is listed under common sites using hsts
            // });

            // Enable access across all origins
            app.UseCors("AllowAllOrigins");

            // Enable access static HTML pages
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Enable server side caching
            // Will cache response of API which are marked with [ResponseCache(CacheProfileName = "ServerResponseCacheProfile")]
            app.UseResponseCaching();

            // Add MVC to pipeline
            app.UseMvc();
        }

        // For i
        private static void SeedTestData(ProblemSolvingDBContext context)
        {
            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "Ford Livonia",
                Location = "Fort Mill, SC"
            });

            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "Ford Charlotte",
                Location = "Charlotte, NC"
            });

            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "GM Detriot",
                Location = "Fort Mill, SC"
            });

            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "GM Houston",
                Location = "Houston, TX"
            });

            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "GM Dallas",
                Location = "Dallas, TX"
            });

            context.Customers.Add(new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = "Honda Santa Clara",
                Location = "Santa Clara, CA"
            });

            context.SaveChanges();
        }
    }
}

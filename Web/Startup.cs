using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using API.Middleware;
using BLL.Configurastion;
using BLL.Helpers;
using BLL.Services;
using BLL.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using DAL;
using DAL.Commands;
using DAL.Entities;
using DAL.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers()
                .AddFluentValidation().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IValidator<ApplicationUser>, UserValidator>();

            services.AddTransient<IUserQuery,UserQuery>();

            services.AddTransient<IUserCommand, UserCommand>();


            services.AddTransient<ICourseQuery, CourseQuery>();

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



             services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("ConnStr"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,

                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,

                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddTransient<IMailService, MailService>();

            services.AddTransient<ICourseService, CourseService>();
            
            services.AddTransient<IAccountService,AccountService>();

            services.AddTransient<IAutheticateService, AutheticateService>();

            services.AddTransient<ICourseCommand, CourseCommand>();

            services.AddDbContext<ApplicationDbContext>(options =>
                                                         options.UseSqlServer(Configuration
                                                                 .GetConnectionString("ConnStr")).EnableSensitiveDataLogging());;


            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddHttpClient();

            services.Configure<FacebookAuthSettings>(Configuration.GetSection("FacebookAuthSettings"));

            services.AddSingleton<FacebookAuthSettings>();
                
            services.AddTransient<IFacebookAuthService, FacebookAuthService>();
            
            services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddTransient<IJwtUtils, JwtUtils>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          
            
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            
            
            app.UseHangfireDashboard();
            
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}

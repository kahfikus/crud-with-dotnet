using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam20203.Entities;
using Exam20203.Enums;
using Exam20203.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Exam20203
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

            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "Data-Protection");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
            });

            services.AddAntiforgery(Q =>
            {
                Q.Cookie.IsEssential = true;
                Q.Cookie.HttpOnly = true;
                Q.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                Q.Cookie.SameSite = SameSiteMode.Lax;
                Q.SuppressXFrameOptionsHeader = true;
            });
            services.AddRazorPages();

            services.AddDbContextPool<CartDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostGreConnString"), strategy =>
                {
                    strategy.EnableRetryOnFailure();
                }
                );
            });

            services.AddTransient<CartService>();

            services.AddAuthentication(CartAuthenticationSchemes.Cookie)
                    .AddCookie(CartAuthenticationSchemes.Cookie, options =>
                    {
                        options.LoginPath = "/Auth/Login";
                        options.LogoutPath = "/Auth/Logout";
                        options.AccessDeniedPath = "/Auth/AccessDenied";

                        options.Cookie.IsEssential = true;
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    });
       

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Cart API" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranApp API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}

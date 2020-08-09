using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApplicationCore.Data;
using ApplicationCore.Data.Entities;
using services.contracts;
using services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
//using web.middleware;
using Domain;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CleanArch
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
            services.AddProgressiveWebApp();

            // configure the application cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.MaxAge = TimeSpan.FromDays(30);
                options.Cookie.Name = "MyApp";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //                options.IdleTimeout = TimeSpan.FromSeconds(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                     x => x.UseNetTopologySuite()));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("SendGrid"));

            services.AddAuthentication();
            //.AddGoogle(options =>
            //{
            //    IConfigurationSection googleAuthNSection =
            //        Configuration.GetSection("Authentication:Google");

            //    options.ClientId = Configuration["GoogleAuth:ClientId"];
            //    options.ClientSecret = Configuration["GoogleAuth:ClientSecret"];

            //    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            //    options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
            //    options.SaveTokens = true;

            //    options.Events.OnCreatingTicket = ctx =>
            //    {
            //        List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

            //        tokens.Add(new AuthenticationToken()
            //        {
            //            Name = "TicketCreated",
            //            Value = DateTime.UtcNow.ToString()
            //        });

            //        ctx.Properties.StoreTokens(tokens);

            //        return Task.CompletedTask;
            //    };
            //});
            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["FacebookAuth:AppId"];
            //    facebookOptions.AppSecret = Configuration["FacebookAuth:ClientSecret"]; ;
            //});
            //.AddTwitter(twitterOptions =>
            //{
            //    twitterOptions.ConsumerKey = Configuration["TwitterAuth:AppId"];
            //    twitterOptions.ConsumerSecret = Configuration["TwitterAuth:AppSecret"];
            //})
            //.AddLinkedIn(options =>
            //{
            //    options.ClientId = Configuration["LinkedInAuth:ClientId"];
            //    options.ClientSecret = Configuration["LinkedInAuth:AppSecret"];

            //    options.Events = new OAuthEvents()
            //    {
            //        OnRemoteFailure = loginFailureHandler =>
            //        {
            //            var authProperties = options.StateDataFormat.Unprotect(loginFailureHandler.Request.Query["state"]);
            //            loginFailureHandler.Response.Redirect("/Account/login");
            //            loginFailureHandler.HandleResponse();
            //            return Task.FromResult(0);
            //        }
            //    };
            //});

            // For Dependency Injection
            // Add new Domain Services here
            services.AddTransient<IAddressStateService, AddressStateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            {
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 7;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

using System;
using System.Reflection;
using Apex.Data;
using Apex.Data.Entities.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Websites
{
    public static class StartupIdentity
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services, string connectionString)
        {
            string migrationsAssembly = Assembly.GetAssembly(typeof(ObjectDbContext)).GetName().Name;
            services.AddDbContextPool<ObjectDbContext>(opts =>
                opts.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ObjectDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                // Password settings.
                var passwordOpts = opts.Password;
                passwordOpts.RequireDigit = true;
                passwordOpts.RequiredLength = 8;
                passwordOpts.RequireNonAlphanumeric = false;
                passwordOpts.RequireUppercase = true;
                passwordOpts.RequireLowercase = false;
                passwordOpts.RequiredUniqueChars = 6;

                // Lockout settings.
                var lockoutOpts = opts.Lockout;
                lockoutOpts.AllowedForNewUsers = true;
                lockoutOpts.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                lockoutOpts.MaxFailedAccessAttempts = 5;

                // User settings.
                //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opts.User.RequireUniqueEmail = true;

                // SignIn settings.
                var signinOpts = opts.SignIn;
                signinOpts.RequireConfirmedEmail = true;
                signinOpts.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(opts =>
            {
                // Cookie settings.
                var cookieOpts = opts.Cookie;
                cookieOpts.Expiration = TimeSpan.FromMinutes(20);
                cookieOpts.HttpOnly = true;
                //cookieOpts.Name = "";
                opts.LoginPath = "/Account/Login";
                opts.LogoutPath = "/Account/Logout";
                opts.AccessDeniedPath = "/Account/AccessDenied";
                opts.SlidingExpiration = true;
                opts.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            return services;
        }
    }
}
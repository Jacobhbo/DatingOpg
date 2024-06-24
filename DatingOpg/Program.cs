using DatingOpg.Components;
using DatingOpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using DatingOpg.Services;

namespace DatingOpg
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddDbContext<DtingContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddQuickGridEntityFrameworkAdapter(); ;

            builder.Services.AddScoped<AuthHelperService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<ProfileService>();
            builder.Services.AddScoped<DtingContext>();
            builder.Services.AddHttpContextAccessor(); // Added this line to register IHttpContextAccessor
            builder.Services.AddScoped<LikeService>(); // Added this line to register LikeService
            builder.Services.AddScoped<ChatService>(); // Added this line to register ChatService

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_token";
                    options.LoginPath = "/Accounts/Login";
                    options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                    options.AccessDeniedPath = "/Accounts/AccessDenied";
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
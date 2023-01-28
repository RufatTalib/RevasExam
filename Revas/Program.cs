using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revas.Areas.Manage.Services;
using Revas.DAL;
using Revas.Models;
using Revas.Services;

namespace Revas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<RevasContext>(
                ctx =>
                {
                    ctx.UseSqlServer(
                        builder.Configuration.GetConnectionString("default")
                        );
                }
                );

            builder.Services.AddIdentity<AppUser, IdentityRole>(
                identity =>
                {
                    identity.Password.RequireNonAlphanumeric = false;
                    identity.Password.RequireUppercase = true;
                    identity.Password.RequireLowercase = true;
                    identity.Password.RequireDigit = true;
                    identity.Password.RequiredUniqueChars = 0;
                    identity.Password.RequiredLength = 8;
                }
                ).AddEntityFrameworkStores<RevasContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<AdminLayoutService>();
            builder.Services.AddScoped<HomeLayoutService>();


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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "manage",
                pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}"
                );

            app.MapRazorPages();

            app.Run();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Jeremiah_SupermarketOnline.Data;
using Jeremiah_SupermarketOnline.Models;

namespace Jeremiah_SupermarketOnline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Jeremiah_SupermarketOnlineContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Jeremiah_SupermarketOnlineContext") ?? throw new InvalidOperationException("Connection string 'Jeremiah_SupermarketOnlineContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //session
            builder.Services.AddDistributedMemoryCache(); 
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
using LingoVerse.Models;
using LingoVerse.Repositories.Implementation;
using LingoVerse.Repositories.Interface;
using LingoVerse.Services.Implementation;
using LingoVerse.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace LingoVerse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            //Add signlR
            builder.Services.AddSignalR();
            // Add services to the container.
            builder.Services.AddRazorPages();
            //Add database
            builder.Services.AddDbContext<LinguaVerseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

            // add Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();


            // Add dependency injection
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

            builder.Services.AddScoped<IOfficialPostService, OfficialPostService>();
            builder.Services.AddScoped<IOfficialPostsRepository, OfficialPostsRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUserPremiumSubscriptionsService, UserPremiumSubscriptionsService>();
            builder.Services.AddScoped<IUserPremiumSubscriptionsRepository, UserPremiumSubscriptionsRepository>();

            builder.Services.AddScoped<IEmailService, EmailService>();

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

            app.UseAuthorization();

            app.MapRazorPages();

            app.UseSession();

            app.MapFallbackToPage("/User/Home");

            app.Run();
        }
    }
}

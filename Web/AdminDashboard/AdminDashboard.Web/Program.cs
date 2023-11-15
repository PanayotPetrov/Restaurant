namespace AdminDashboard.Web
{
    using System.Reflection;

    using AdminDashboard.Data;
    using AdminDashboard.Data.Models;
    using AdminDashboard.Data.Repositories;
    using AdminDashboard.Data.Seeding;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Restaurant.Data;
    using Restaurant.Data.Common;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Messaging;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels;
    using Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var restaurantConnectionString = configuration.GetConnectionString("RestaurantConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(restaurantConnectionString));

            var adminDashboardConnectionString = configuration.GetConnectionString("AdminDashboardConnection");
            services.AddDbContext<AdminDashboardDbContext>(options => options.UseSqlServer(adminDashboardConnectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<AdminDashboardUser>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<AdminDashboardRole>().AddEntityFrameworkStores<AdminDashboardDbContext>();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddRazorPages();
            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IRestaurantDeletableEntityRepositoryDecorator<>), typeof(RestaurantDeletableEntityRepositoryDecorator<>));
            services.AddScoped(typeof(IRestaurantRepositoryDecorator<>), typeof(RestaurantRepositoryDecorator<>));
            services.AddScoped(typeof(IAdminDashboardDeletableEntityRepositoryDecorator<>), typeof(AdminDashboardDeletableEntityRepositoryDecorator<>));
            services.AddScoped(typeof(IAdminDashboardRepositoryDecorator<>), typeof(AdminDashboardRepositoryDecorator<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));
            services.AddTransient<IMealService, MealService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ITableService, TableService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IUserMessageService, UserMessageService>();
            services.AddTransient<IPagedItemsModelCreator, PagedItemsModelCreator>();
        }

        private static void Configure(WebApplication app)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, typeof(AddAddressModel).GetTypeInfo().Assembly);

            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AdminDashboardDbContext>();
                dbContext.Database.Migrate();

                new AdminDashboardDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/StatusCodeError", "?errorCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}

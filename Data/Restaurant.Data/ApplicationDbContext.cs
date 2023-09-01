namespace Restaurant.Data
{
    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Common;
    using Restaurant.Data.Models;

    public class ApplicationDbContext : DbContextBase<ApplicationUser, ApplicationRole>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<MealOrder> MealOrders { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<MealImage> MealImages { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<UserMessage> UserMessages { get; set; }

        public DbSet<UserMessageCategory> UserMessageCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.ConfigureUserIdentityRelations(builder);
            builder.Entity<MealOrder>().HasKey(x => new { x.MealId, x.OrderId });
        }

        protected void ConfigureUserIdentityRelations(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}

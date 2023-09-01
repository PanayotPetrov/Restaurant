namespace AdminDashboard.Data
{
    using AdminDashboard.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Common;

    public class AdminDashboardDbContext : DbContextBase<AdminDashboardUser, AdminDashboardRole>
    {
        public AdminDashboardDbContext(DbContextOptions<AdminDashboardDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.ConfigureUserIdentityRelations(builder);
        }

        protected void ConfigureUserIdentityRelations(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}

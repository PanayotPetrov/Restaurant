namespace AdminDashboard.Data
{
    using Restaurant.Data.Common;

    public class AdminDashboardDbContextFactory : DesignTimeDbContextFactory<AdminDashboardDbContext>
    {
        public AdminDashboardDbContextFactory()
            : base("DefaultConnection")
        {
        }
    }
}

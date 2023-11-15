namespace Restaurant.Data
{
    using Restaurant.Data.Common;

    public class ApplicationDbContextFactory : DesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContextFactory()
            : base("DefaultConnection")
        {
        }
    }
}

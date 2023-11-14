namespace AdminDashboard.Data.Repositories
{
    using Restaurant.Data.Common.Repositories;

    public class AdminDashboardRepositoryDecorator<TEntity> : EfRepositoryBase<TEntity>, IAdminDashboardRepositoryDecorator<TEntity>
        where TEntity : class
    {
        public AdminDashboardRepositoryDecorator(AdminDashboardDbContext context)
            : base(context)
        {
        }
    }
}

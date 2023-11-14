namespace AdminDashboard.Data.Repositories
{
    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Common.Repositories;

    public class AdminDashboardDeletableEntityRepositoryDecorator<TEntity> : EfDeletableEntityRepositoryBase<TEntity>, IAdminDashboardDeletableEntityRepositoryDecorator<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public AdminDashboardDeletableEntityRepositoryDecorator(AdminDashboardDbContext context)
            : base(context)
        {
        }
    }
}

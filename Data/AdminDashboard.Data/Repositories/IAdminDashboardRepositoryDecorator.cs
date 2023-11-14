namespace AdminDashboard.Data.Repositories
{
    using Restaurant.Data.Common.Repositories;

    public interface IAdminDashboardRepositoryDecorator<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
    }
}

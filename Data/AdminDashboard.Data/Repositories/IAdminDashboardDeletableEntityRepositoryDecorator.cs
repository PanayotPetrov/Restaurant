namespace AdminDashboard.Data.Repositories
{
    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Common.Repositories;

    public interface IAdminDashboardDeletableEntityRepositoryDecorator<TEntity> : IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
    }
}

namespace Restaurant.Data.Repositories
{
    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Common.Repositories;

    public interface IRestaurantDeletableEntityRepositoryDecorator<TEntity> : IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
    }
}

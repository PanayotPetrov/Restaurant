namespace Restaurant.Data.Repositories
{
    using Restaurant.Data.Common.Repositories;

    public interface IRestaurantRepositoryDecorator<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
    }
}

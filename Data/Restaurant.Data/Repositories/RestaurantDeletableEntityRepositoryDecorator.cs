namespace Restaurant.Data.Repositories
{
    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Common.Repositories;

    public class RestaurantDeletableEntityRepositoryDecorator<TEntity> : EfDeletableEntityRepositoryBase<TEntity>, IRestaurantDeletableEntityRepositoryDecorator<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public RestaurantDeletableEntityRepositoryDecorator(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}

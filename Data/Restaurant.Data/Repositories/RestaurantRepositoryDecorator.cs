namespace Restaurant.Data.Repositories
{
    using Restaurant.Data.Common.Repositories;

    public class RestaurantRepositoryDecorator<TEntity> : EfRepositoryBase<TEntity>, IRestaurantRepositoryDecorator<TEntity>
        where TEntity : class
    {
        public RestaurantRepositoryDecorator(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}

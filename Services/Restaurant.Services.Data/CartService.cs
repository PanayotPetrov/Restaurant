namespace Restaurant.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IRepository<CartItem> cartItemRepository;

        public CartService(IDeletableEntityRepository<Cart> cartRepository, IRepository<CartItem> cartItemRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        public async Task AddToCartAsync(string userId, CartItemModel model)
        {
            var cart = this.cartRepository.All().FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart is null)
            {
                cart = new Cart { ApplicationUserId = userId };
            }

            var cartItem = this.cartItemRepository.All().FirstOrDefault(ci => ci.MealId == model.MealId && ci.Quantity == model.Quantity);

            if (cartItem is null)
            {
                cartItem = AutoMapperConfig.MapperInstance.Map<CartItem>(model);
            }

            cart.CartItems.Add(cartItem);
            await this.cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, CartItemModel model)
        {
            var cart = this.cartRepository.All().FirstOrDefault(c => c.ApplicationUserId == userId);

            var cartItem = this.cartItemRepository.All().FirstOrDefault(ci => ci.MealId == model.MealId && ci.Quantity == model.Quantity);

            cart.CartItems.Remove(cartItem);
            cartItem.Carts.Remove(cart);
            await this.cartItemRepository.SaveChangesAsync();
            await this.cartRepository.SaveChangesAsync();
        }

        public T GetCartByUserId<T>(string userId)
        {
            return this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).To<T>().FirstOrDefault();
        }

        public async Task<T> CreateCartForUserAsync<T>(string userId)
        {
            var cart = new Cart { ApplicationUserId = userId };
            await this.cartRepository.AddAsync(cart);
            await this.cartRepository.SaveChangesAsync();
            return AutoMapperConfig.MapperInstance.Map<T>(cart);
        }
    }
}

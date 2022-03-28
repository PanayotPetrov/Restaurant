namespace Restaurant.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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
            var cart = this.cartRepository.All().Include(c => c.CartItems).FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart is null)
            {
                cart = await this.CreateCartForUserAsync<Cart>(userId);
            }

            if (cart.CartItems.Any(ci => ci.MealId == model.MealId))
            {
                cart.CartItems.FirstOrDefault(ci => ci.MealId == model.MealId).Quantity += model.Quantity;
            }
            else
            {
                var cartItem = AutoMapperConfig.MapperInstance.Map<CartItem>(model);
                cart.CartItems.Add(cartItem);
            }

            await this.cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, CartItemModel model)
        {
            var cart = this.cartRepository.All().FirstOrDefault(c => c.ApplicationUserId == userId);

            var cartItem = this.cartItemRepository.All().FirstOrDefault(ci => ci.MealId == model.MealId && ci.Id == cart.Id);
            cart.CartItems.Remove(cartItem);
            this.cartItemRepository.Delete(cartItem);
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

        public decimal GetCartTotalPrice(string userId)
        {
            var cartId = this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.Id).FirstOrDefault();
            var sum = this.cartItemRepository.AllAsNoTracking().Where(ci => ci.CartId == cartId).Select(ci => ci.Meal.Price * ci.Quantity).ToList().Sum();

            return sum;
        }
    }
}

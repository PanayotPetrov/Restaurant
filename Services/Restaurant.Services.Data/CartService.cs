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
        private const decimal ShippingPrice = 3.90M;
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IRepository<CartItem> cartItemRepository;
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public CartService(IDeletableEntityRepository<Cart> cartRepository, IRepository<CartItem> cartItemRepository, IDeletableEntityRepository<Meal> mealRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.mealRepository = mealRepository;
        }

        public async Task AddToCartAsync(string userId, CartItemModel model)
        {
            var cart = this.cartRepository.All().Include(c => c.CartItems).FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart is null)
            {
                cart = await this.CreateCartForUserAsync<Cart>(userId);
            }

            var mealPrice = this.mealRepository.AllAsNoTracking().Where(m => m.Id == model.MealId).Select(m => m.Price).FirstOrDefault();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MealId == model.MealId);

            if (cartItem is not null)
            {
                cart.CartItems.FirstOrDefault(ci => ci.MealId == model.MealId).Quantity += model.Quantity;
            }
            else
            {
                cartItem = AutoMapperConfig.MapperInstance.Map<CartItem>(model);
                cart.CartItems.Add(cartItem);
                await this.cartItemRepository.SaveChangesAsync();
            }

            cartItem.ItemTotalPrice += mealPrice * model.Quantity;
            cart.SubTotal += mealPrice * model.Quantity;
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
            cart.ShippingPrice = ShippingPrice;
            await this.cartRepository.AddAsync(cart);
            await this.cartRepository.SaveChangesAsync();
            return AutoMapperConfig.MapperInstance.Map<T>(cart);
        }

        public decimal GetCartSubTotal(string userId)
        {
            return this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.SubTotal).FirstOrDefault();
        }

        public int GetItemQuantityPerCartLeft(string userId)
        {
            var cartId = this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.Id).FirstOrDefault();
            var itemQuantity = this.cartItemRepository.AllAsNoTracking().Where(ci => ci.CartId == cartId).Select(ci => ci.Quantity).Sum();
            return 100 - itemQuantity;
        }

        public async Task<T> ChangeItemQuantityAsync<T>(string userId, CartItemModel model)
        {
            var cart = this.cartRepository.All().Include(c => c.CartItems).FirstOrDefault(c => c.ApplicationUserId == userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MealId == model.MealId);
            cartItem.Quantity = model.Quantity;
            await this.cartRepository.SaveChangesAsync();
            return AutoMapperConfig.MapperInstance.Map<T>(cart);
        }
    }
}

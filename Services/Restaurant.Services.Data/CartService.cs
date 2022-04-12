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
        private const int AllowedItemQuantityPerCart = 100;

        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IRepository<CartItem> cartItemRepository;
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public CartService(IDeletableEntityRepository<Cart> cartRepository, IRepository<CartItem> cartItemRepository, IDeletableEntityRepository<Meal> mealRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.mealRepository = mealRepository;
        }

        public T GetCartByUserId<T>(string userId)
        {
            return this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).To<T>().FirstOrDefault();
        }

        public int GetCartId(string userId)
        {
            return this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.Id).FirstOrDefault();
        }

        public decimal GetCartSubTotal(string userId)
        {
            return this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.SubTotal).FirstOrDefault();
        }

        public Cart GetCartById<T>(int cartId)
        {
            return this.cartRepository.AllAsNoTracking().Include(c => c.CartItems).Where(c => c.Id == cartId).FirstOrDefault();
        }

        public int GetItemQuantityPerCartLeft(string userId)
        {
            var cartId = this.cartRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).Select(c => c.Id).FirstOrDefault();
            var currentCartItemsQuantity = this.cartItemRepository.AllAsNoTracking().Where(ci => ci.CartId == cartId).Select(ci => ci.Quantity).Sum();
            return AllowedItemQuantityPerCart - currentCartItemsQuantity;
        }

        public async Task RemoveFromCartAsync(CartItemModel model)
        {
            var cartItem = this.cartItemRepository.All().Include(ci => ci.Cart).FirstOrDefault(ci => ci.MealId == model.MealId);
            cartItem.Cart.SubTotal -= cartItem.ItemTotalPrice;
            this.cartItemRepository.Delete(cartItem);
            await this.cartItemRepository.SaveChangesAsync();
        }

        public async Task<T> CreateCartForUserAsync<T>(string userId)
        {
            var cart = new Cart { ApplicationUserId = userId };
            cart.ShippingPrice = ShippingPrice;
            await this.cartRepository.AddAsync(cart);
            await this.cartRepository.SaveChangesAsync();
            return AutoMapperConfig.MapperInstance.Map<T>(cart);
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

        public async Task<T> ChangeItemQuantityAsync<T>(CartItemModel model)
        {
            var cartItem = await this.cartItemRepository.All().Include(ci => ci.Meal).FirstOrDefaultAsync(ci => ci.MealId == model.MealId);

            var cart = await this.cartRepository.All().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == cartItem.CartId);
            var mealPrice = cartItem.Meal.Price;

            if (cartItem.Quantity < model.Quantity)
            {
                cartItem.ItemTotalPrice = mealPrice * model.Quantity;
            }
            else
            {
                cartItem.ItemTotalPrice = mealPrice * model.Quantity;
            }

            cart.SubTotal = cart.CartItems.Select(ci => ci.ItemTotalPrice).Sum();

            cartItem.Quantity = model.Quantity;
            await this.cartItemRepository.SaveChangesAsync();
            return AutoMapperConfig.MapperInstance.Map<T>(cartItem);
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = this.cartItemRepository.All().Where(ci => ci.CartId == cartId).ToList();

            for (int i = 0; i < cartItems.Count; i++)
            {
                var cartItem = cartItems[i];
                this.cartItemRepository.Delete(cartItem);
            }

            var cart = this.cartRepository.All().FirstOrDefault(c => c.Id == cartId);
            cart.SubTotal = 0;
            await this.cartItemRepository.SaveChangesAsync();
        }
    }
}

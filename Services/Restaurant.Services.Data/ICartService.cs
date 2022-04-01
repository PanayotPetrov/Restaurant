namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Services.Models;

    public interface ICartService
    {
        Task AddToCartAsync(string userId, CartItemModel cartItem);

        Task RemoveFromCartAsync(CartItemModel model);

        T GetCartByUserId<T>(string userId);

        Task<T> CreateCartForUserAsync<T>(string userId);

        decimal GetCartSubTotal(string userId);

        int GetItemQuantityPerCartLeft(string userId);

        Task<T> ChangeItemQuantityAsync<T>(CartItemModel cartItem);

        int GetCartId(string userId);

        Cart GetCartById<T>(int cartId);

        Task ClearCartAsync(int cartId);
    }
}

namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface ICartService
    {
        Task AddToCartAsync(string userId, CartItemModel cartItem);

        Task RemoveFromCartAsync(string userId, CartItemModel model);

        T GetCartByUserId<T>(string userId);

        Task<T> CreateCartForUserAsync<T>(string userId);

        decimal GetCartSubTotal(string userId);

        int GetItemQuantityPerCartLeft(string userId);

        Task<T> ChangeItemQuantityAsync<T>(string userId, CartItemModel cartItem);
    }
}

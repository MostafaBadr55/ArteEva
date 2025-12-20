using ArteEva.Models;

namespace ArtEva.Services.Interfaces
{
    public interface ICartService
    {
        public   Task<Cart> GetOrCreateCartAsync(int userId);
        

        public Task<Cart?> GetCartByUserIdAsync(int userId);
        

        public Task<Cart?> GetCartWithItemsAsync(int userId);
       

        public   Task AddItemAsync(
            Cart cart,
            int productId,
            decimal unitPriceSnapshot,
            int quantity);
         

        public Task UpdateItemQuantityAsync(
            Cart cart,
            int productId,
            int quantity);
        
        public   Task RemoveItemAsync(Cart cart, int productId);
       

        public   Task ClearCartAsync(Cart cart);

        public bool IsCartEmpty(Cart cart);
    

        public decimal CalculateCartTotal(Cart cart);
        

        public   Task SaveAsync();
        

    }
}

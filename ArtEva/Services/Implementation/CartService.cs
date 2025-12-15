using ArteEva.Models;
using ArteEva.Repositories;
using ArteEva.Services;

namespace ArtEva.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(userId);

            if (cart != null)
                return cart;

            cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChanges();

            return cart;
        }

        public Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }

        public Task<Cart?> GetCartWithItemsAsync(int userId)
        {
            return _cartRepository.GetCartWithItemsAsync(userId);
        }

        public async Task AddItemAsync(
            Cart cart,
            int productId,
            decimal unitPriceSnapshot,
            int quantity)
        {
            EnsureItemsLoaded(cart);

            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero.");

            var existing = cart.CartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                return;
            }

            cart.CartItems.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPriceSnapshot = unitPriceSnapshot
            });
        }

        public async Task UpdateItemQuantityAsync(
            Cart cart,
            int productId,
            int quantity)
        {
            EnsureItemsLoaded(cart);

            var item = cart.CartItems
                .FirstOrDefault(i => i.ProductId == productId)
                ?? throw new InvalidOperationException("Item not found in cart.");

            if (quantity <= 0)
            {
                cart.CartItems.Remove(item);
                return;
            }

            item.Quantity = quantity;
        }

        public async Task RemoveItemAsync(Cart cart, int productId)
        {
            EnsureItemsLoaded(cart);

            var item = cart.CartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
                cart.CartItems.Remove(item);
        }

        public async Task ClearCartAsync(Cart cart)
        {
            EnsureItemsLoaded(cart);
            cart.CartItems.Clear();
        }

        public bool IsCartEmpty(Cart cart)
        {
            EnsureItemsLoaded(cart);
            return !cart.CartItems.Any();
        }

        public decimal CalculateCartTotal(Cart cart)
        {
            EnsureItemsLoaded(cart);

            return cart.CartItems.Sum(i =>
                i.UnitPriceSnapshot * i.Quantity);
        }

        public async Task SaveAsync()
        {
            await _cartRepository.SaveChanges();
        }

        private static void EnsureItemsLoaded(Cart cart)
        {
            if (cart.CartItems == null)
                throw new InvalidOperationException(
                    "CartItems not loaded. Use GetCartWithItemsAsync.");
        }
    }
}

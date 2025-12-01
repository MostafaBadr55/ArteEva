using ArteEva.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace ArteEva.Data
{
    public class ApplicationDbContext : IdentityDbContext<
    User,
    Role,
    int,
    IdentityUserClaim<int>,
    UserRole,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<ShopFollower> ShopFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserRole relationships
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Cart)
                    .WithOne(c => c.User)
                    .HasForeignKey<Cart>(c => c.UserId);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();
            });

            // Shop configurations
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasOne(s => s.Owner)
                    .WithMany(u => u.Shops)
                    .HasForeignKey(s => s.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasIndex(s => s.Slug).IsUnique();

                entity.Property(s => s.RatingAverage).HasPrecision(3, 2);
            });

            // Category configurations
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // SubCategory configurations
            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasOne(sc => sc.Category)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(sc => sc.CategoryId);

                entity.HasIndex(sc => new { sc.CategoryId, sc.Name }).IsUnique();
            });

            // Product configurations
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Shop)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.ShopId);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.SubCategory)
                    .WithMany(sc => sc.Products)
                    .HasForeignKey(p => p.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(p => new { p.ShopId, p.SKU }).IsUnique();

                entity.Property(p => p.Price).HasPrecision(18, 2);
            });

            // ProductImage configurations
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(pi => pi.ProductId);
            });

            // CartItem configurations
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId);

                entity.HasOne(ci => ci.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();

                entity.Property(ci => ci.UnitPriceSnapshot).HasPrecision(18, 2);
            });

            // Address configurations
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Order configurations
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Shop)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(o => o.ShopId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ShippingAddress)
                    .WithMany(a => a.Orders)
                    .HasForeignKey(o => o.ShippingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(o => o.OrderNumber).IsUnique();

                entity.Property(o => o.Subtotal).HasPrecision(18, 2);
                entity.Property(o => o.ShippingFee).HasPrecision(18, 2);
                entity.Property(o => o.Discount).HasPrecision(18, 2);
                entity.Property(o => o.TaxTotal).HasPrecision(18, 2);
                entity.Property(o => o.GrandTotal).HasPrecision(18, 2);
            });

            // OrderItem configurations
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId);

                entity.HasOne(oi => oi.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(oi => oi.UnitPriceSnapshot).HasPrecision(18, 2);
                entity.Property(oi => oi.Subtotal).HasPrecision(18, 2);
            });

            // Payment configurations
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderId);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Payments)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Amount).HasPrecision(18, 2);
            });

            // Refund configurations
            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasOne(r => r.Order)
                    .WithMany(o => o.Refunds)
                    .HasForeignKey(r => r.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Payment)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(r => r.PaymentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(r => r.Amount).HasPrecision(18, 2);
            });

            // Shipment configurations
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasOne(s => s.Order)
                    .WithMany(o => o.Shipments)
                    .HasForeignKey(s => s.OrderId);

                entity.HasOne(s => s.ShippingAddress)
                    .WithMany(a => a.Shipments)
                    .HasForeignKey(s => s.ShippingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Review configurations
            modelBuilder.Entity<Review>(entity =>
            {
                // Reviewer (who wrote the review)
                entity.HasOne(r => r.Reviewer)
                    .WithMany(u => u.WrittenReviews)
                    .HasForeignKey(r => r.ReviewerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Product review (TargetType = Product)
                entity.HasOne(r => r.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.TargetId)
                    .HasPrincipalKey(p => p.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Shop review (TargetType = Shop)
                entity.HasOne(r => r.Shop)
                    .WithMany(s => s.Reviews)
                    .HasForeignKey(r => r.TargetId)
                    .HasPrincipalKey(s => s.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Buyer review (seller reviewing a customer)
                entity.HasOne(r => r.ReviewedBuyer)
                    .WithMany(u => u.ReceivedReviews)
                    .HasForeignKey(r => r.TargetId)
                    .HasPrincipalKey(u => u.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Optional: prevent duplicate reviews of the same target by the same reviewer
                entity.HasIndex(r => new { r.ReviewerUserId, r.TargetType, r.TargetId })
                      .IsUnique();
            });


            // Favorite configurations
            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Favorites)
                    .HasForeignKey(f => f.UserId);

                entity.HasOne(f => f.Product)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(f => f.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(f => new { f.UserId, f.ProductId }).IsUnique();
            });

            // UserNotification configurations
            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasOne(un => un.User)
                    .WithMany(u => u.UserNotifications)
                    .HasForeignKey(un => un.UserId);

                entity.HasOne(un => un.Notification)
                    .WithMany(n => n.UserNotifications)
                    .HasForeignKey(un => un.NotificationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Dispute configurations
            modelBuilder.Entity<Dispute>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(o => o.Disputes)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.Disputes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ShopFollower configurations
            modelBuilder.Entity<ShopFollower>(entity =>
            {
                entity.HasOne(sf => sf.Shop)
                    .WithMany(s => s.ShopFollowers)
                    .HasForeignKey(sf => sf.ShopId);

                entity.HasOne(sf => sf.User)
                    .WithMany(u => u.ShopFollowers)
                    .HasForeignKey(sf => sf.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(sf => new { sf.ShopId, sf.UserId }).IsUnique();
            });
        }
    }
}

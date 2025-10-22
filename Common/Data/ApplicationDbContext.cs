using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Shared.Entities;
namespace StoreManagement.API.Shared.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets - Đăng ký tất cả Entities
        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<Inventory> Inventory => Set<Inventory>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<PromotionProduct> PromotionProducts => Set<PromotionProduct>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<InventoryHistory> InventoryHistory => Set<InventoryHistory>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho MySQL với UUID
            ConfigureMySQLModels(modelBuilder);

            // Cấu hình quan hệ giữa các Entities
            ConfigureRelationships(modelBuilder);

        }
        private void ConfigureMySQLModels(ModelBuilder modelBuilder)
        {
            // Cấu hình tất cả ID là string (UUID)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null)
                {
                    idProperty.SetMaxLength(50);
                }
            }

            // Cấu hình decimal precision
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // Cấu hình enum properties thành string
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Gender)
                .HasConversion<string>()
                .HasMaxLength(10);

            modelBuilder.Entity<Customer>()
                .Property(c => c.CustomerType)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountType)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.ApplyFor)
                .HasConversion<string>()
                .HasMaxLength(30);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<InventoryHistory>()
                .Property(ih => ih.ChangeType)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // User Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Promotions)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PurchaseOrders)
                .WithOne(po => po.CreatedBy)
                .HasForeignKey(po => po.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.InventoryHistories)
                .WithOne(ih => ih.CreatedBy)
                .HasForeignKey(ih => ih.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer Relationships
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category Relationships
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductVariant Relationships
            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Inventory)
                .WithOne(i => i.ProductVariant)
                .HasForeignKey<Inventory>(i => i.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order Relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Promotion)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PromotionId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem Relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.ProductVariant)
                .WithMany(pv => pv.OrderItems)
                .HasForeignKey(oi => oi.VariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Promotion Relationships
            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionProducts)
                .WithOne(pp => pp.Promotion)
                .HasForeignKey(pp => pp.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            // PromotionProduct Relationships (Many-to-Many)
            modelBuilder.Entity<PromotionProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.PromotionProducts)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // InventoryHistory Relationships
            modelBuilder.Entity<InventoryHistory>()
                .HasOne(ih => ih.ProductVariant)
                .WithMany(pv => pv.InventoryHistories)
                .HasForeignKey(ih => ih.VariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder Relationships
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(po => po.PurchaseOrderItems)
                .WithOne(poi => poi.PurchaseOrder)
                .HasForeignKey(poi => poi.PoId)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseOrderItem Relationships
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(poi => poi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

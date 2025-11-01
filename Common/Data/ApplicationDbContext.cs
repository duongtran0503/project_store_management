using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Shared.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

     
        // =======================================================
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<ShiftHistory> ShiftHistories => Set<ShiftHistory>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<InventoryReceipt> InventoryReceipts => Set<InventoryReceipt>();
        public DbSet<ReceiptDetail> ReceiptDetails => Set<ReceiptDetail>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
        // =======================================================


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            ConfigureMySQLModels(modelBuilder);

       
            ConfigureRelationships(modelBuilder);

        }

        private void ConfigureMySQLModels(ModelBuilder modelBuilder)
        {
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null)
                {
                 
                    idProperty.SetMaxLength(36);
                }
            }

        
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
              
                property.SetPrecision(18);
                property.SetScale(2);
            }

           
            modelBuilder.Entity<Account>()
                .Property(u => u.RoleName)
                .HasMaxLength(50); 

            modelBuilder.Entity<Invoice>()
                .Property(o => o.PaymentMethod)
                .HasMaxLength(50); 

            modelBuilder.Entity<Voucher>()
                .Property(p => p.VoucherType)
                .HasMaxLength(50);

          
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Isbn)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
         

            // Account 1:N ShiftHistory (NV tạo nhiều ca làm việc)
            modelBuilder.Entity<ShiftHistory>()
                .HasOne(sh => sh.Staff)
                .WithMany(a => a.Shifts)
                .HasForeignKey(sh => sh.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account 1:N Invoices (NV thu ngân tạo nhiều hóa đơn)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.CashierStaff)
                .WithMany(a => a.CashierInvoices)
                .HasForeignKey(i => i.CashierStaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account 1:N InventoryReceipts (NV tạo nhiều phiếu nhập)
            modelBuilder.Entity<InventoryReceipt>()
                .HasOne(ir => ir.ReceivingStaff)
                .WithMany(a => a.ReceivedReceipts)
                .HasForeignKey(ir => ir.ReceivingStaffId)
                .OnDelete(DeleteBehavior.Restrict);

         

            // Category 1:N Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


           

            // Supplier 1:N InventoryReceipts
            modelBuilder.Entity<InventoryReceipt>()
                .HasOne(ir => ir.Supplier)
                .WithMany(s => s.InventoryReceipts)
                .HasForeignKey(ir => ir.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryReceipt 1:N ReceiptDetails
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Receipt)
                .WithMany(ir => ir.ReceiptDetails)
                .HasForeignKey(rd => rd.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Book 1:N ReceiptDetails
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Book)
                .WithMany(b => b.ReceiptDetails)
                .HasForeignKey(rd => rd.BookId)
                .OnDelete(DeleteBehavior.Restrict);


     

            // Voucher 1:N Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Voucher)
                .WithMany(v => v.Invoices)
                .HasForeignKey(i => i.VoucherCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice 1:N InvoiceDetails
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(id => id.Invoice)
                .WithMany(i => i.InvoiceDetails)
                .HasForeignKey(id => id.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Book 1:N InvoiceDetails
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(id => id.Book)
                .WithMany(b => b.InvoiceDetails)
                .HasForeignKey(id => id.BookId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
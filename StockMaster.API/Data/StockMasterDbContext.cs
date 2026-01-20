using Microsoft.EntityFrameworkCore;
using StockMaster.API.Models;

namespace StockMaster.API.Data
{
    public class StockMasterDbContext : DbContext
    {
        public StockMasterDbContext(DbContextOptions<StockMasterDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Movement> Movements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.HasIndex(e => e.SKU).IsUnique();

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Supplier configuration
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            // Movement configuration
            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electrónica", Description = "Productos electrónicos", Icon = "fa-laptop", Color = "blue" },
                new Category { Id = 2, Name = "Ropa", Description = "Vestimenta y accesorios", Icon = "fa-tshirt", Color = "purple" },
                new Category { Id = 3, Name = "Alimentos", Description = "Productos alimenticios", Icon = "fa-apple-alt", Color = "green" },
                new Category { Id = 4, Name = "Herramientas", Description = "Herramientas y equipos", Icon = "fa-tools", Color = "orange" },
                new Category { Id = 5, Name = "Deportes", Description = "Artículos deportivos", Icon = "fa-futbol", Color = "red" },
                new Category { Id = 6, Name = "Librería", Description = "Libros y útiles escolares", Icon = "fa-book", Color = "cyan" }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Tech Supplies SA", ContactName = "Juan Pérez", Email = "ventas@techsupplies.com", Phone = "+54 11 4567-8900", Address = "Av. Corrientes 1234", City = "Buenos Aires" },
                new Supplier { Id = 2, Name = "Distribuidora Central", ContactName = "María González", Email = "info@distcentral.com", Phone = "+54 11 5432-1098", Address = "Calle San Martín 567", City = "Córdoba" },
                new Supplier { Id = 3, Name = "Importadora Andina", ContactName = "Carlos Rodríguez", Email = "compras@impandina.com", Phone = "+54 261 423-9876", Address = "Av. San Martín 890", City = "Mendoza" },
                new Supplier { Id = 4, Name = "Almacén Logístico", ContactName = "Ana Martínez", Email = "contacto@almalog.com", Phone = "+54 341 456-7890", Address = "Bv. Oroño 234", City = "Rosario", IsActive = false }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, SKU = "PROD-001", Name = "Laptop Dell XPS 13", Description = "Laptop de alta gama", CategoryId = 1, SupplierId = 1, Price = 1200.00m, Stock = 15, MinStock = 5 },
                new Product { Id = 2, SKU = "PROD-002", Name = "Mouse Logitech MX Master", Description = "Mouse inalámbrico premium", CategoryId = 1, SupplierId = 1, Price = 89.99m, Stock = 45, MinStock = 10 },
                new Product { Id = 3, SKU = "PROD-003", Name = "Teclado Mecánico RGB", Description = "Teclado gaming", CategoryId = 1, SupplierId = 1, Price = 149.99m, Stock = 30, MinStock = 10 },
                new Product { Id = 4, SKU = "PROD-004", Name = "Camiseta Deportiva", Description = "Camiseta de algodón", CategoryId = 2, SupplierId = 2, Price = 29.99m, Stock = 120, MinStock = 30 },
                new Product { Id = 5, SKU = "PROD-005", Name = "Pantalón Jean", Description = "Jean clásico", CategoryId = 2, SupplierId = 2, Price = 59.99m, Stock = 80, MinStock = 20 }
            );
        }
    }
}

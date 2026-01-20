using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockMaster.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    MinStock = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "Icon", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "blue", new DateTime(2026, 1, 16, 16, 36, 32, 406, DateTimeKind.Local).AddTicks(7175), "Productos electrónicos", "fa-laptop", true, "Electrónica" },
                    { 2, "purple", new DateTime(2026, 1, 16, 16, 36, 32, 408, DateTimeKind.Local).AddTicks(6055), "Vestimenta y accesorios", "fa-tshirt", true, "Ropa" },
                    { 3, "green", new DateTime(2026, 1, 16, 16, 36, 32, 408, DateTimeKind.Local).AddTicks(6069), "Productos alimenticios", "fa-apple-alt", true, "Alimentos" },
                    { 4, "orange", new DateTime(2026, 1, 16, 16, 36, 32, 408, DateTimeKind.Local).AddTicks(6071), "Herramientas y equipos", "fa-tools", true, "Herramientas" },
                    { 5, "red", new DateTime(2026, 1, 16, 16, 36, 32, 408, DateTimeKind.Local).AddTicks(6072), "Artículos deportivos", "fa-futbol", true, "Deportes" },
                    { 6, "cyan", new DateTime(2026, 1, 16, 16, 36, 32, 408, DateTimeKind.Local).AddTicks(6073), "Libros y útiles escolares", "fa-book", true, "Librería" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "City", "ContactName", "Country", "CreatedAt", "Email", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "Av. Corrientes 1234", "Buenos Aires", "Juan Pérez", "Argentina", new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(1522), "ventas@techsupplies.com", true, "Tech Supplies SA", "+54 11 4567-8900" },
                    { 2, "Calle San Martín 567", "Córdoba", "María González", "Argentina", new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(2431), "info@distcentral.com", true, "Distribuidora Central", "+54 11 5432-1098" },
                    { 3, "Av. San Martín 890", "Mendoza", "Carlos Rodríguez", "Argentina", new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(2434), "compras@impandina.com", true, "Importadora Andina", "+54 261 423-9876" },
                    { 4, "Bv. Oroño 234", "Rosario", "Ana Martínez", "Argentina", new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(2436), "contacto@almalog.com", false, "Almacén Logístico", "+54 341 456-7890" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "MinStock", "Name", "Price", "SKU", "Stock", "SupplierId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(3120), "Laptop de alta gama", true, 5, "Laptop Dell XPS 13", 1200.00m, "PROD-001", 15, 1, null },
                    { 2, 1, new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(4252), "Mouse inalámbrico premium", true, 10, "Mouse Logitech MX Master", 89.99m, "PROD-002", 45, 1, null },
                    { 3, 1, new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(4258), "Teclado gaming", true, 10, "Teclado Mecánico RGB", 149.99m, "PROD-003", 30, 1, null },
                    { 4, 2, new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(4260), "Camiseta de algodón", true, 30, "Camiseta Deportiva", 29.99m, "PROD-004", 120, 2, null },
                    { 5, 2, new DateTime(2026, 1, 16, 16, 36, 32, 409, DateTimeKind.Local).AddTicks(4262), "Jean clásico", true, 20, "Pantalón Jean", 59.99m, "PROD-005", 80, 2, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ProductId",
                table: "Movements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}

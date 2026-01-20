using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMaster.API.Data;
using StockMaster.API.Models;

namespace StockMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StockMasterDbContext _context;

        public ProductsController(StockMasterDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/Products/search?q=query
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return await GetProducts();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.IsActive &&
                    (p.Name.Contains(q) ||
                     p.SKU.Contains(q) ||
                     p.Description.Contains(q)))
                .ToListAsync();

            return products;
        }

        // GET: api/Products/low-stock
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.IsActive && p.Stock <= p.MinStock)
                .ToListAsync();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Check if SKU already exists
            if (await _context.Products.AnyAsync(p => p.SKU == product.SKU))
            {
                return BadRequest(new { error = "El SKU ya existe" });
            }

            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            // Check if SKU already exists for another product
            if (await _context.Products.AnyAsync(p => p.SKU == product.SKU && p.Id != id))
            {
                return BadRequest(new { error = "El SKU ya existe" });
            }

            product.UpdatedAt = DateTime.Now;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Soft delete
            product.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Products/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetProductStats()
        {
            var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
            var lowStockCount = await _context.Products.CountAsync(p => p.IsActive && p.Stock <= p.MinStock);
            var totalValue = await _context.Products
                .Where(p => p.IsActive)
                .SumAsync(p => p.Price * p.Stock);
            var categories = await _context.Categories.CountAsync(c => c.IsActive);

            return new
            {
                totalProducts,
                lowStockCount,
                totalValue,
                categories
            };
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

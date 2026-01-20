using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMaster.API.Data;
using StockMaster.API.Models;

namespace StockMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly StockMasterDbContext _context;

        public SuppliersController(StockMasterDbContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ContactName,
                    s.Email,
                    s.Phone,
                    s.Address,
                    s.City,
                    s.Country,
                    s.IsActive,
                    s.CreatedAt,
                    ProductCount = s.Products.Count(p => p.IsActive),
                    TotalPurchases = s.Products.Where(p => p.IsActive).Sum(p => p.Price * p.Stock)
                })
                .ToListAsync();

            return suppliers;
        }

        // GET: api/Suppliers/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetActiveSuppliers()
        {
            return await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            if (await _context.Suppliers.AnyAsync(s => s.Email == supplier.Email))
            {
                return BadRequest(new { error = "Ya existe un proveedor con ese email" });
            }

            supplier.CreatedAt = DateTime.Now;
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            if (await _context.Suppliers.AnyAsync(s => s.Email == supplier.Email && s.Id != id))
            {
                return BadRequest(new { error = "Ya existe un proveedor con ese email" });
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            // Check if supplier has products
            if (await _context.Products.AnyAsync(p => p.SupplierId == id && p.IsActive))
            {
                return BadRequest(new { error = "No se puede eliminar un proveedor con productos asociados" });
            }

            // Soft delete
            supplier.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Suppliers/5/toggle-status
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleSupplierStatus(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            supplier.IsActive = !supplier.IsActive;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}

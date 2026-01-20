using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMaster.API.Data;
using StockMaster.API.Models;

namespace StockMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly StockMasterDbContext _context;

        public MovementsController(StockMasterDbContext context)
        {
            _context = context;
        }

        // GET: api/Movements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movement>>> GetMovements()
        {
            return await _context.Movements
                .Include(m => m.Product)
                .ThenInclude(p => p.Category)
                .OrderByDescending(m => m.Date)
                .Take(100)
                .ToListAsync();
        }

        // GET: api/Movements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movement>> GetMovement(int id)
        {
            var movement = await _context.Movements
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movement == null)
            {
                return NotFound();
            }

            return movement;
        }

        // GET: api/Movements/product/5
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Movement>>> GetMovementsByProduct(int productId)
        {
            return await _context.Movements
                .Include(m => m.Product)
                .Where(m => m.ProductId == productId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        // POST: api/Movements
        [HttpPost]
        public async Task<ActionResult<Movement>> PostMovement(Movement movement)
        {
            var product = await _context.Products.FindAsync(movement.ProductId);
            if (product == null)
            {
                return BadRequest(new { error = "Producto no encontrado" });
            }

            // Update product stock based on movement type
            switch (movement.Type.ToLower())
            {
                case "entrada":
                    product.Stock += movement.Quantity;
                    break;
                case "salida":
                    if (product.Stock < movement.Quantity)
                    {
                        return BadRequest(new { error = $"Stock insuficiente. Stock actual: {product.Stock}" });
                    }
                    product.Stock -= movement.Quantity;
                    break;
                case "ajuste":
                    product.Stock = movement.Quantity;
                    break;
                default:
                    return BadRequest(new { error = "Tipo de movimiento inválido" });
            }

            movement.Date = DateTime.Now;
            _context.Movements.Add(movement);
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovement), new { id = movement.Id }, movement);
        }

        // GET: api/Movements/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetMovementStats()
        {
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var todayMovements = await _context.Movements
                .Where(m => m.Date >= today)
                .CountAsync();

            var monthMovements = await _context.Movements
                .Where(m => m.Date >= thisMonth)
                .CountAsync();

            var todayEntries = await _context.Movements
                .Where(m => m.Date >= today && m.Type == "entrada")
                .SumAsync(m => m.Quantity);

            var todayExits = await _context.Movements
                .Where(m => m.Date >= today && m.Type == "salida")
                .SumAsync(m => m.Quantity);

            return new
            {
                todayMovements,
                monthMovements,
                todayEntries,
                todayExits
            };
        }
    }
}

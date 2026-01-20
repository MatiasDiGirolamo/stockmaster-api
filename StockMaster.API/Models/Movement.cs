namespace StockMaster.API.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string Type { get; set; } = string.Empty; // "entrada", "salida", "ajuste"
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string User { get; set; } = "Administrador";
        public DateTime Date { get; set; } = DateTime.Now;
    }
}

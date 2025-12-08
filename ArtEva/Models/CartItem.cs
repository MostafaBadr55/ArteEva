namespace ArteEva.Models
{
    public class CartItem : BaseModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPriceSnapshot { get; set; }

        // Navigation Properties
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}

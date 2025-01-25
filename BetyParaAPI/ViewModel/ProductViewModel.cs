namespace BetyParaAPI.ViewModel
{
    public class ProductViewModel
    {  
        public string QRCode { get; set; } 
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceForSale { get; set; }
        public Guid SupplierId { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryID { get; set; }
        public DateTime DateExp { get; set; }
 
    }
}

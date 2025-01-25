using Domaine.Entities;

namespace Core.Application.Dto_s;

public class ProductDto
{
    public Guid ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal PriceForSale { get; set; }
    public Guid SupplierId { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryID { get; set; }
    public DateTime DateExp { get; set; }
    public bool IsAvailable { get; set; } = true; // Corrected property name

    public string QRCode { get; set; } // Add QR code as a string (can be URL or encoded data)
}

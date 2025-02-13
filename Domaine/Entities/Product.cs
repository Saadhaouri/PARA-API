﻿namespace Domaine.Entities
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceForSale { get; set; } // New property for sale price
        public int Quantity { get; set; }
        public Guid CategoryID { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }
        public DateTime DateExp { get; set; }
        public bool IsAvailable { get; set; } // Corrected property name
        public ICollection<ProductPromotion> ProductPromotions { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }

        public ICollection<DebtProduct> DebtProducts { get; set; }

        public string QRCode { get; set; } // Stores QR code data (e.g., URL or product info)
    }
}

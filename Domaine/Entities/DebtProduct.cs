using System;
using Domaine.Entities;

namespace Domaine.Entities
{
    public class DebtProduct
    {
        public Guid DebtId { get; set; }
        public Debt Debt { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}

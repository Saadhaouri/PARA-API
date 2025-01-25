using Domaine.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Dto_s;

public class CreateDebtDto
{

    public Guid DebtID { get; set; }
    public Guid ClientID { get; set; }
    public DateTime LastDatePayee { get; set; }
 
    public decimal total { get; set; }
    public DateTime DateDebt { get; set; }
    public string Status { get; set; }
    public decimal avance { get; set; }
    public decimal rest { get; set; }

    public List<Guid> ProductIds { get; set; } // For associated products
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domaine.Entities;

public class Debt
{
    public Guid DebtID { get; set; }
    public Guid ClientID { get; set; }
    public DateTime LastDatePayee { get; set; }
    public ICollection<DebtProduct> DebtProducts { get; set; }
    public decimal total{ get; set; }
    public DateTime DateDebt { get; set; }
    public string Status { get; set; }
    public decimal avance { get; set; } 
    public  decimal rest { get; set; }    

}

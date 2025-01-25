using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BetyParaAPI.ViewModel
{
    public class CreateDebtViewModel
    {
   
        public Guid ClientID { get; set; }
        public DateTime DateDebt { get; set; }
        public DateTime LastDatePayee { get; set; }
        public decimal total { get; set; }
        public string Status { get; set; }
        public decimal avance { get; set; }

        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}

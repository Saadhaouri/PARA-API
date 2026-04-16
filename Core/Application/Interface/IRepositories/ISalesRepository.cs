using Domaine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Application.Interface.IRepositories
{
    public interface ISalesRepository
    {
        void Add(Sale sale);
        IQueryable<Sale> GetSales();
        IEnumerable<Sale> GetSalesPaged(int page, int pageSize);
        IEnumerable<Sale> GetLatestSales(int count);
        void Update(Sale sale);
        void Delete(Guid id);
        void DeleteAllSales();
        IEnumerable<MonthlyBenefit> GetMonthlyBenefits();
    }
}
using Core.Application.Interface.IRepositories;
using Domaine.Entities;
using Infra.DATA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly PrDbContext _context;

        public SalesRepository(PrDbContext context)
        {
            _context = context;
        }

        public void Add(Sale sale)
        {
            _context.Sales.Add(sale);
            _context.SaveChanges();
        }

        public IEnumerable<Sale> GetSales()
        {
            return _context.Sales.Include(s => s.Product).ToList();
        }

        public void DeleteAllSales()
        {
            var allSales = _context.Sales.ToList();
            _context.Sales.RemoveRange(allSales);
            _context.SaveChanges();
        }

        public IEnumerable<MonthlyBenefit> GetMonthlyBenefits()
        {
            var monthlyBenefits = _context.Sales
                .GroupBy(sale => new { sale.SaleDate.Year, sale.SaleDate.Month })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Benefit = group.Sum(sale => sale.Profit)
                })
                .ToList()
                .Select(group => new MonthlyBenefit
                {
                    Month = new DateTime(group.Year, group.Month, 1).ToString("MMMM", CultureInfo.CurrentCulture),
                    Benefit = group.Benefit
                })
                .OrderBy(mb => DateTime.ParseExact(mb.Month, "MMMM", CultureInfo.CurrentCulture).Month)
                .ToList();

            return monthlyBenefits;
        }
    }
}

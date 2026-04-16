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

        public IQueryable<Sale> GetSales()
        {
            return _context.Sales
                .Include(s => s.Product)
                .AsNoTracking()
                .AsQueryable();
        }

        public IEnumerable<Sale> GetSalesPaged(int page, int pageSize)
        {
            return _context.Sales
                .Include(s => s.Product)
                .AsNoTracking()
                .OrderByDescending(s => s.SaleDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Sale> GetLatestSales(int count)
        {
            return _context.Sales
                .Include(s => s.Product)
                .AsNoTracking()
                .OrderByDescending(s => s.SaleDate)
                .Take(count)
                .ToList();
        }

        public void Update(Sale sale)
        {
            var existingSale = _context.Sales.FirstOrDefault(s => s.Id == sale.Id);

            if (existingSale == null)
            {
                throw new Exception("Sale not found");
            }

            existingSale.ProductId = sale.ProductId;
            existingSale.Quantity = sale.Quantity;
            existingSale.Price = sale.Price;
            existingSale.Profit = sale.Profit;
            existingSale.SaleDate = sale.SaleDate;

            _context.Sales.Update(existingSale);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                throw new Exception("Sale not found");
            }

            _context.Sales.Remove(sale);
            _context.SaveChanges();
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
                .AsNoTracking()
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
                    Month = new DateTime(group.Year, group.Month, 1)
                        .ToString("MMMM", CultureInfo.CurrentCulture),
                    Benefit = group.Benefit
                })
                .OrderBy(mb => DateTime.ParseExact(
                    mb.Month,
                    "MMMM",
                    CultureInfo.CurrentCulture
                ).Month)
                .ToList();

            return monthlyBenefits;
        }
    }
}
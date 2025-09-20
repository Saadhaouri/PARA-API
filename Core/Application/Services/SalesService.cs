using AutoMapper;
using Core.Application.Dto_s;
using Core.Application.Interface.IService;
using Core.Application.Interface.IRepositories;
using Domaine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

public class SalesService : ISalesService
{
    private readonly IProductRepository _productRepository;
    private readonly ISalesRepository _salesRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IMapper _mapper;

    public SalesService(
        IProductRepository productRepository,
        ISalesRepository salesRepository,
        IPromotionRepository promotionRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _salesRepository = salesRepository;
        _promotionRepository = promotionRepository;
        _mapper = mapper;
    }

    public void AddSale(SaleDto saleDto)
    {
        var sale = _mapper.Map<Sale>(saleDto);
        sale.Id = Guid.NewGuid();
        sale.SaleDate = DateTime.UtcNow;
        var product = _productRepository.GetProductById(sale.ProductId);

        if (product == null || product.Quantity < sale.Quantity)
        {
            throw new InvalidOperationException("Product not available or insufficient stock.");
        }

        // Check for active promotions
        var promotion = _promotionRepository.GetActivePromotionForProduct(sale.ProductId);
        if (promotion != null)
        {
            var discount = (promotion.Discount / 100) * product.Price;
            var discountedPrice = product.Price - discount;
            sale.Price = discountedPrice * sale.Quantity;
        }
        else
        {
            sale.Price = product.Price * sale.Quantity;
        }

        sale.Profit = (product.PriceForSale - product.Price) * sale.Quantity;

        _salesRepository.Add(sale);

        // Update the stock
        product.Quantity -= sale.Quantity;
        _productRepository.Save();
    }

    public IEnumerable<SaleDto> GetDailySales()
    {
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == DateTime.UtcNow.Date);
        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetWeeklySales()
    {
        var startDate = DateTime.UtcNow.AddDays(-((int)DateTime.UtcNow.DayOfWeek + 1));
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetMonthlySales()
    {
        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public decimal GetTotalDailyProfit()
    {
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == DateTime.UtcNow.Date);
        return sales.Sum(s => s.Profit);
    }

    public decimal GetTotalWeeklyProfit()
    {
        var startDate = DateTime.UtcNow.AddDays(-((int)DateTime.UtcNow.DayOfWeek + 1));
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return sales.Sum(s => s.Profit);
    }

    public decimal GetTotalMonthlyProfit()
    {
        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return sales.Sum(s => s.Profit);
    }

    public IEnumerable<SaleDto> GetAllSales()
    {
        var sales = _salesRepository.GetSales();
        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public void DeleteAllSales()
    {
        _salesRepository.DeleteAllSales();
    }
    public IEnumerable<MonthlyBenefitDto> GetMonthlyBenefits()
    {
        var monthlyBenefits = _salesRepository.GetMonthlyBenefits();
        return monthlyBenefits.Select(mb => new MonthlyBenefitDto
        {
            Month = mb.Month,
            Benefit = mb.Benefit
        }).ToList();
    }

    public IEnumerable<CapitalBenefitDto> GetCapitalAndBenefits(int year, int? month)
    {
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Year == year && (month == null || s.SaleDate.Month == month));

        var result = sales
            .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
            .Select(g => new CapitalBenefitDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalCapital = g.Sum(s => s.Price - s.Profit ),  // Assuming you have a Capital field
                TotalBenefit = g.Sum(s => s.Profit)   // Assuming you have a Benefit field
            }).ToList();

        return result;
    }

    public decimal GetTotalDailyCapital()
    {
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == DateTime.UtcNow.Date);
        return sales.Sum(s => s.Price - s.Profit); // Capital = Total Sale Price - Profit
    }

    public decimal GetTotalWeeklyCapital()
    {
        var startDate = DateTime.UtcNow.AddDays(-((int)DateTime.UtcNow.DayOfWeek + 1));
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return sales.Sum(s => s.Price - s.Profit);
    }

    public decimal GetTotalMonthlyCapital()
    {
        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= DateTime.UtcNow.Date);
        return sales.Sum(s => s.Price - s.Profit);
    }


}

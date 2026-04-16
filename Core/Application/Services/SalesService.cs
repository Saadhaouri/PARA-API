using AutoMapper;
using Core.Application.Dto_s;
using Core.Application.Interface.IRepositories;
using Core.Application.Interface.IService;
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

        var promotion = _promotionRepository.GetActivePromotionForProduct(sale.ProductId);

        if (promotion != null)
        {
            var discount = (promotion.Discount / 100m) * product.Price;
            var discountedPrice = product.Price - discount;
            sale.Price = discountedPrice * sale.Quantity;
        }
        else
        {
            sale.Price = product.Price * sale.Quantity;
        }

        sale.Profit = (product.PriceForSale - product.Price) * sale.Quantity;

        _salesRepository.Add(sale);

        product.Quantity -= sale.Quantity;
        _productRepository.Save();
    }

    public void UpdateSale(Guid id, SaleDto saleDto)
    {
        var existingSale = _salesRepository.GetSales().FirstOrDefault(s => s.Id == id);

        if (existingSale == null)
        {
            throw new InvalidOperationException("Sale not found.");
        }

        var oldProduct = _productRepository.GetProductById(existingSale.ProductId);

        if (oldProduct == null)
        {
            throw new InvalidOperationException("Original product not found.");
        }

        oldProduct.Quantity += existingSale.Quantity;

        var newProductId = saleDto.ProductId;
        var newQuantity = saleDto.Quantity;

        var newProduct = _productRepository.GetProductById(newProductId);

        if (newProduct == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (newProduct.Quantity < newQuantity)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        decimal totalPrice;
        var promotion = _promotionRepository.GetActivePromotionForProduct(newProductId);

        if (promotion != null)
        {
            var discount = (promotion.Discount / 100m) * newProduct.Price;
            var discountedPrice = newProduct.Price - discount;
            totalPrice = discountedPrice * newQuantity;
        }
        else
        {
            totalPrice = newProduct.Price * newQuantity;
        }

        existingSale.ProductId = newProductId;
        existingSale.Quantity = newQuantity;
        existingSale.Price = totalPrice;
        existingSale.Profit = (newProduct.PriceForSale - newProduct.Price) * newQuantity;

        _salesRepository.Update(existingSale);

        newProduct.Quantity -= newQuantity;
        _productRepository.Save();
    }

    public void DeleteSale(Guid id)
    {
        var existingSale = _salesRepository.GetSales().FirstOrDefault(s => s.Id == id);

        if (existingSale == null)
        {
            throw new InvalidOperationException("Sale not found.");
        }

        var product = _productRepository.GetProductById(existingSale.ProductId);

        if (product != null)
        {
            product.Quantity += existingSale.Quantity;
            _productRepository.Save();
        }

        _salesRepository.Delete(id);
    }

    public IEnumerable<SaleDto> GetDailySales()
    {
        var today = DateTime.UtcNow.Date;

        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == today)
            .OrderByDescending(s => s.SaleDate);

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetWeeklySales()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-((int)today.DayOfWeek + 1));

        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .OrderByDescending(s => s.SaleDate);

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetMonthlySales()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = new DateTime(today.Year, today.Month, 1);

        var sales = _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .OrderByDescending(s => s.SaleDate);

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetAllSales(int page, int pageSize)
    {
        var sales = _salesRepository
            .GetSales() // IQueryable
            .OrderByDescending(s => s.SaleDate) // ✅ HERE
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public IEnumerable<SaleDto> GetLatestSales(int count)
    {
        var sales = _salesRepository
            .GetSales()
            .OrderByDescending(s => s.SaleDate) // ✅ HERE
            .Take(count);

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public decimal GetTotalDailyProfit()
    {
        var today = DateTime.UtcNow.Date;

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == today)
            .Sum(s => s.Profit);
    }

    public decimal GetTotalWeeklyProfit()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-((int)today.DayOfWeek + 1));

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .Sum(s => s.Profit);
    }

    public decimal GetTotalMonthlyProfit()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = new DateTime(today.Year, today.Month, 1);

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .Sum(s => s.Profit);
    }

    public decimal GetTotalDailyCapital()
    {
        var today = DateTime.UtcNow.Date;

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date == today)
            .Sum(s => s.Price - s.Profit);
    }

    public decimal GetTotalWeeklyCapital()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-((int)today.DayOfWeek + 1));

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .Sum(s => s.Price - s.Profit);
    }

    public decimal GetTotalMonthlyCapital()
    {
        var today = DateTime.UtcNow.Date;
        var startDate = new DateTime(today.Year, today.Month, 1);

        return _salesRepository.GetSales()
            .Where(s => s.SaleDate.Date >= startDate && s.SaleDate.Date <= today)
            .Sum(s => s.Price - s.Profit);
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

        return sales
            .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
            .Select(g => new CapitalBenefitDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalCapital = g.Sum(s => s.Price - s.Profit),
                TotalBenefit = g.Sum(s => s.Profit)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();
    }
}

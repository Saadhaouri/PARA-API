using Core.Application.Dto_s;
using System;
using System.Collections.Generic;

namespace Core.Application.Interface.IService
{
    public interface ISalesService
    {
        void AddSale(SaleDto saleDto);
        void UpdateSale(Guid id, SaleDto saleDto);
        void DeleteSale(Guid id);

        IEnumerable<SaleDto> GetDailySales();
        IEnumerable<SaleDto> GetWeeklySales();
        IEnumerable<SaleDto> GetMonthlySales();
        IEnumerable<SaleDto> GetAllSales(int page, int pageSize);
        IEnumerable<SaleDto> GetLatestSales(int count);

        decimal GetTotalDailyProfit();
        decimal GetTotalWeeklyProfit();
        decimal GetTotalMonthlyProfit();

        decimal GetTotalDailyCapital();
        decimal GetTotalWeeklyCapital();
        decimal GetTotalMonthlyCapital();

        void DeleteAllSales();

        IEnumerable<MonthlyBenefitDto> GetMonthlyBenefits();
        IEnumerable<CapitalBenefitDto> GetCapitalAndBenefits(int year, int? month);
    }
}
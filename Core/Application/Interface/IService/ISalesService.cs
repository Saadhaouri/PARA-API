using Core.Application.Dto_s;
using System.Collections.Generic;

namespace Core.Application.Interface.IService;

public interface ISalesService
{
    void AddSale(SaleDto saleDto);
    IEnumerable<SaleDto> GetDailySales();
    IEnumerable<SaleDto> GetWeeklySales();
    IEnumerable<SaleDto> GetMonthlySales();
    decimal GetTotalDailyProfit();

    decimal GetTotalWeeklyProfit();
    decimal GetTotalMonthlyProfit();
    decimal GetTotalDailyCapital();
    decimal GetTotalWeeklyCapital();
    decimal GetTotalMonthlyCapital();
    IEnumerable<SaleDto> GetAllSales();
    void DeleteAllSales();
    IEnumerable<MonthlyBenefitDto> GetMonthlyBenefits();
    IEnumerable<CapitalBenefitDto> GetCapitalAndBenefits(int year, int? month);
}

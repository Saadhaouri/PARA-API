using Domaine.Entities;

namespace Core.Application.Interface.IRepositories
{
    public interface ISalesRepository
    {
        void Add(Sale sale);
        IEnumerable<Sale> GetSales();
        void DeleteAllSales();
        IEnumerable<MonthlyBenefit> GetMonthlyBenefits();  // Method to get monthly benefits
    }
}

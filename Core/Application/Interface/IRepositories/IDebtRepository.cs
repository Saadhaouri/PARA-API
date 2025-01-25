using Domaine.Entities;


namespace Core.Application.Interface.IRepositories;

public interface IDebtRepository
{
   
    Debt AddDebt(Debt debt, List<Guid> productIds);
    Debt GetDebtById(Guid debtId);
    IEnumerable<Debt> GetAllDebts();
    void UpdateDebt(Debt debt, List<Guid> productIds);
    void DeleteDebt(Guid debtId);
}

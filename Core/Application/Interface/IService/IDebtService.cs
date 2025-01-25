using Core.Application.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interface.IService
{
    public interface IDebtService
    {
       
        DebtDto CreateDebt(CreateDebtDto createDebtDto);

        DebtDto GetDebtById(Guid debtId);

        /// <summary>
        /// Retrieves all debts.
        /// </summary>
        /// <returns>A collection of all debts as DTOs.</returns>
        IEnumerable<DebtDto> GetAllDebts();

        /// <summary>
        /// Updates an existing debt.
        /// </summary>
        /// <param name="debtId">The ID of the debt to update.</param>
        /// <param name="updateDebtDto">The DTO containing the updated details of the debt.</param>
        void UpdateDebt(Guid debtId, CreateDebtDto updateDebtDto);

        /// <summary>
        /// Deletes a debt by its ID.
        /// </summary>
        /// <param name="debtId">The ID of the debt to delete.</param>
        void DeleteDebt(Guid debtId);
    }
}

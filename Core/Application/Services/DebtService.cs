    using AutoMapper;
using Core.Application.Dto_s;
using Core.Application.Interface.IRepositories;
using Core.Application.Interface.IService;
using Core.Application.Interface.IServices;
using Domaine.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Services
{
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtRepository;
        private readonly IMapper _mapper;

        public DebtService(IDebtRepository debtRepository, IMapper mapper)
        {
            _debtRepository = debtRepository;
            _mapper = mapper;
        }

        public DebtDto CreateDebt(CreateDebtDto createDebtDto)
        {
            var debt = _mapper.Map<Debt>(createDebtDto);
            debt.DebtID = Guid.NewGuid();
            debt.DateDebt= DateTime.UtcNow;

            var addedDebt = _debtRepository.AddDebt(debt, createDebtDto.ProductIds);

            return _mapper.Map<DebtDto>(addedDebt);
        }

        public DebtDto GetDebtById(Guid debtId)
        {
            var debt = _debtRepository.GetDebtById(debtId);

            if (debt == null)
                throw new ArgumentNullException(nameof(debt));

            return _mapper.Map<DebtDto>(debt);
        }

        public IEnumerable<DebtDto> GetAllDebts()
        {
            var debts = _debtRepository.GetAllDebts();
            return _mapper.Map<IEnumerable<DebtDto>>(debts);
        }

        public void UpdateDebt(Guid debtId, CreateDebtDto updateDebtDto)
        {
            var debt = _debtRepository.GetDebtById(debtId);

            if (debt == null)
                throw new ArgumentNullException(nameof(debt));

            // Map the updated properties to the existing debt
            debt.ClientID = updateDebtDto.ClientID;
            debt.total = updateDebtDto.total;
            debt.DateDebt = updateDebtDto.DateDebt;
            debt.Status = updateDebtDto.Status;
            debt.LastDatePayee = updateDebtDto.LastDatePayee;
            debt.avance = updateDebtDto.avance;
            debt.rest = updateDebtDto.rest; 
            

            debt.rest = debt.total - debt.avance;

            // Update DebtProducts
            debt.DebtProducts.Clear();
            foreach (var productId in updateDebtDto.ProductIds)
            {
                debt.DebtProducts.Add(new DebtProduct { DebtId = debt.DebtID, ProductId = productId });
            }

            _debtRepository.UpdateDebt(debt, updateDebtDto.ProductIds);
        }


        public void DeleteDebt(Guid debtId)
        {
            _debtRepository.DeleteDebt(debtId);
        }
    }
}

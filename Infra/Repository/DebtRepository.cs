using Core.Application.Interface.IRepositories;
using Domaine.Entities;
using Infra.DATA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra.Repository
{
    public class DebtRepository : IDebtRepository
    {
        private readonly PrDbContext _context;

        public DebtRepository(PrDbContext context)
        {
            _context = context;
        }

        public Debt AddDebt(Debt debt, List<Guid> productIds)
        {
            _context.Debts.Add(debt);

            foreach (var productId in productIds)
            {
                _context.DebtProducts.Add(new DebtProduct { Debt = debt, ProductId = productId });
            }

            _context.SaveChanges();
            return debt;
        }

        public Debt GetDebtById(Guid debtId)
        {
            return _context.Debts
                .Include(d => d.DebtProducts)
                .FirstOrDefault(d => d.DebtID == debtId);
        }

        public IEnumerable<Debt> GetAllDebts()
        {
            return _context.Debts
                .Include(d => d.DebtProducts)
                .ToList();
        }

        public void UpdateDebt(Debt debt, List<Guid> productIds)
        {
            var existingDebt = _context.Debts
                .Include(d => d.DebtProducts)
                .FirstOrDefault(d => d.DebtID == debt.DebtID);

            if (existingDebt != null)
            {
                existingDebt.ClientID = debt.ClientID;
                existingDebt.total = debt.total;
                existingDebt.DateDebt = debt.DateDebt;
                existingDebt.LastDatePayee = debt.LastDatePayee;
                existingDebt.Status = debt.Status;
                existingDebt.LastDatePayee = debt.LastDatePayee;
                existingDebt.avance = debt.avance;

                existingDebt.rest = existingDebt.total - existingDebt.avance;


                foreach (var dp in existingDebt.DebtProducts.ToList())
                {
                    var trackedEntity = _context.DebtProducts.Local
                        .FirstOrDefault(ep => ep.DebtId == dp.DebtId && ep.ProductId == dp.ProductId);
                    if (trackedEntity != null)
                    {
                        _context.Entry(trackedEntity).State = EntityState.Detached;
                    }
                }

                // Clear existing debt products
                _context.DebtProducts.RemoveRange(existingDebt.DebtProducts);

                // Add new debt products
                foreach (var productId in productIds)
                {
                    var newDebtProduct = new DebtProduct { DebtId = existingDebt.DebtID, ProductId = productId };

                    // Avoid reattaching any locally tracked entities
                    var trackedEntity = _context.DebtProducts.Local
                        .FirstOrDefault(dp => dp.DebtId == newDebtProduct.DebtId && dp.ProductId == newDebtProduct.ProductId);
                    if (trackedEntity != null)
                    {
                        _context.Entry(trackedEntity).State = EntityState.Detached;
                    }

                    _context.DebtProducts.Add(newDebtProduct);
                }

                // Save changes to the database
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Debt not found.");
            }
        }

        public void DeleteDebt(Guid debtId)
        {
            var debt = _context.Debts.Find(debtId);
            if (debt != null)
            {
                _context.Debts.Remove(debt);
                _context.SaveChanges();
            }
        }
    }

}

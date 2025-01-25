using AutoMapper;
using BetyParaAPI.ViewModel;
using Core.Application.Dto_s;
using Core.Application.Interface.IService;
using Core.Application.Interface.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BetyParaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebtController : ControllerBase
    {
        private readonly IDebtService _debtService;
        private readonly IMapper _mapper;

        public DebtController(IDebtService debtService, IMapper mapper)
        {
            _debtService = debtService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateDebt([FromBody] CreateDebtViewModel createDebtViewModel)
        {
            if (createDebtViewModel == null)
                return BadRequest();

            var createDebtDto = _mapper.Map<CreateDebtDto>(createDebtViewModel);
            var createdDebt = _debtService.CreateDebt(createDebtDto);

            var debtViewModel = _mapper.Map<DebtViewModel>(createdDebt);

            return Ok(debtViewModel);
        }

        [HttpGet("{debtId}")]
        public IActionResult GetDebtById(Guid debtId)
        {
            var debt = _debtService.GetDebtById(debtId);

            if (debt == null)
                return NotFound();

            var debtViewModel = _mapper.Map<DebtViewModel>(debt);

            return Ok(debtViewModel);
        }

        [HttpGet]
        public IActionResult GetAllDebts()
        {
            var debts = _debtService.GetAllDebts();
            var debtViewModels = _mapper.Map<List<DebtViewModel>>(debts);
            return Ok(debtViewModels);
        }

        [HttpPut("{debtId}")]
        public IActionResult UpdateDebt(Guid debtId, [FromBody] CreateDebtViewModel debtViewModel)
        {
            if (debtViewModel == null)
                return BadRequest();

            var debtDto = _mapper.Map<CreateDebtDto>(debtViewModel);
            _debtService.UpdateDebt(debtId, debtDto);

            return Ok("Debt updated successfully");
        }

        [HttpDelete("{debtId}")]
        public IActionResult DeleteDebt(Guid debtId)
        {
            _debtService.DeleteDebt(debtId);
            return NoContent();
        }
    }
}



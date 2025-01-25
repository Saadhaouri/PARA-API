﻿using AutoMapper;
using BetyParaAPI.ViewModel;
using Core.Application.Dto_s;
using Core.Application.Interface.IService;
using Microsoft.AspNetCore.Mvc;

namespace BetyParaAPI.Controllers;
[ApiController]
[Route("[controller]")]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var signUpUserDto = _mapper.Map<SignUpUserDto>(registerViewModel);
        var result = await _accountService.SignUpAsync(signUpUserDto);

        if (result.Succeeded)
        {
           
            return Ok(new { Message = "Registration successful" });
        }
        else
        {
            
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInViewModel signInViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var signInUserDto = new SignInUserDto
        {
            UsernameOrEmail = signInViewModel.UsernameOrEmail,
            Password = signInViewModel.Password
        };

        var token = await _accountService.LoginAsync(signInUserDto);

        if (token == null)
        {
            // Authentication failed
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        // Authentication succeeded, return the token
        return Ok(new { Token = token });
    }

    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _accountService.ChangePasswordAsync(changePasswordViewModel.UserId, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);

        if (result.Succeeded)
        {
            // Password change successful
            return Ok(new { Message = "Password changed successfully" });
        }
        else
        {
            // Password change failed, return error messages
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }
    }

    [HttpPost("generatepasswordresettoken")]
    public async Task<IActionResult> GeneratePasswordResetToken([FromBody] GeneratePasswordResetTokenViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _accountService.GeneratePasswordResetTokenAsync(model.Email);
        if (token == null)
        {
            return BadRequest(new { Message = "Failed to generate password reset token." });
        }

        return Ok(new { Token = token });
    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _accountService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Password reset successful" });
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }
    }
}

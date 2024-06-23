﻿using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto_s;

public class SignUpUserDto
{
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}

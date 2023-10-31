﻿using HotelManagement.Models.Constants;
using HotelManagement.Models.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.ViewModels;

public class UserViewModel
{
    public Guid? Id { get; set; }

    [Required]
    [RegularExpression(@"[a-zA-Z0-9]{4,30}$", ErrorMessage = "Usernames must be alphanumeric and must be between 4 and 30 characters")]
    public string Username { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Passwords must be between 8 and 30 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%^&*!])[^\s]+$", ErrorMessage = "Passwords can " +
        "not contain white spaces and must have at least 1 lowercase letter, 1 digit and 1 special character")]
    public string Password { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required]
    [DateAfter1900(ErrorMessage = "Date should be after 1900")]
    public DateOnly BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [StringLength(100, ErrorMessage = "Address can have at most 100 characters")]
    public string? Address { get; set; }

    [StringLength(1000, ErrorMessage = "Bio can have at most 1000 characters")]
    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public string ActivationToken { get; set; }

    public DateTime TokenGenerationTime { get; set; }
    [ProfilePicture(ErrorMessage = "File format not supported!")]
    [FileSize(ErrorMessage = "File size is too big!")]
    public IFormFile ProfilePicture { get; set; }

    public DataModels.DBRole Role { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);

        Validator.TryValidateObject(this, context, validationResults, validateAllProperties: true);

        return validationResults;
    }
}

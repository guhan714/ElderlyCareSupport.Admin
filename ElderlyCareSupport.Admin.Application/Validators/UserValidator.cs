using ElderlyCareSupport.Admin.Domain.Models;
using FluentValidation;

namespace ElderlyCareSupport.Admin.Application.Validators;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(rule => rule.FirstName).NotEmpty().WithMessage("First name cannot be empty");
        RuleFor(rule => rule.LastName).NotEmpty().WithMessage("Last name cannot be empty");
        RuleFor(rule => rule.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
        RuleFor(rule => rule.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must be between 8 and 8 symbols");
        RuleFor(rule => rule.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty");
        RuleFor(rule => rule.Gender)
            .NotEmpty().WithMessage("Gender cannot be empty");
        RuleFor(rule => rule.City)
            .NotEmpty().WithMessage("City cannot be empty");
        RuleFor(rule => rule.Address)
            .NotEmpty().WithMessage("Address cannot be empty");
        RuleFor(rule => rule.Country)
            .NotEmpty().WithMessage("Country cannot be empty");
        RuleFor(rule => rule.Region)
            .NotEmpty().WithMessage("Region cannot be empty");
        RuleFor(rule => rule.PostalCode)
            .NotEmpty().WithMessage("PostalCode cannot be empty");
        RuleFor(rule => rule.IsActive)
            .NotEmpty().WithMessage("IsActive cannot be empty");
        RuleFor(rule => rule.UserType)
            .NotEmpty().WithMessage("UserType cannot be empty");
    }
}
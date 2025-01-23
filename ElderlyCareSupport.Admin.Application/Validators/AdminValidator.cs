using FluentValidation;

namespace ElderlyCareSupport.Admin.Application.Validators;

public sealed class AdminValidator : AbstractValidator<Contracts.Request.Admin>
{
    public AdminValidator()
    {
        RuleFor(rule => rule.Username)
            .NotEmpty().WithMessage("Username is required.")
            .NotNull().WithMessage("Username is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(rule => rule.Password)
            .NotEmpty().WithMessage("Password is required.")
            .NotNull().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

       }
}
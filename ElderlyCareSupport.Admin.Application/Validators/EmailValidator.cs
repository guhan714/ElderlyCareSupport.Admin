using FluentValidation;

namespace ElderlyCareSupport.Admin.Application.Validators;

public sealed class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is invalid.");
    }
}
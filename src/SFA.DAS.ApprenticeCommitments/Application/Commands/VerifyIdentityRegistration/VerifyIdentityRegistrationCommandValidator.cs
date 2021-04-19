using FluentValidation;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration
{
    public class VerifyIdentityRegistrationCommandValidator : AbstractValidator<VerifyIdentityRegistrationCommand>
    {
        public VerifyIdentityRegistrationCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("The Registration Id must be valid");
            RuleFor(model => model.UserIdentityId).Must(id => id != default).WithMessage("The User Identity Id must be valid");
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("The First name is required");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("The Last name is required");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("Date of birth is required");
            RuleFor(model => model.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required");
        }
    }
}

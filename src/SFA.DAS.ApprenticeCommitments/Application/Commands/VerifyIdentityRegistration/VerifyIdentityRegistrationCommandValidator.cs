using FluentValidation;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration
{
    public class VerifyIdentityRegistrationCommandValidator : AbstractValidator<VerifyIdentityRegistrationCommand>
    {
        public VerifyIdentityRegistrationCommandValidator()
        {
            RuleFor(model => model.RegistrationId).Must(id => id != default).WithMessage("The Registration Id must be valid");
            RuleFor(model => model.UserIdentityId).Must(id => id != default).WithMessage("The User Identity Id must be valid");
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("The First name is required");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("The Last name is required");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("Date of birth is required");
            RuleFor(model => model.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required");
            RuleFor(model => model.NationalInsuranceNumber).NotEmpty().WithMessage("National insurance number is required");
            RuleFor(model => model.NationalInsuranceNumber).Matches(@"^(?!BG|GB|NK|KN|TN|NT|ZZ)[ABCEGHJ-PRSTW-Z][ABCEGHJ-NPRSTW-Z]\s*\d{2}\s*\d{2}\s*\d{2}\s*[A-D]$").WithMessage("National insurance number format is wrong");
        }
    }
}

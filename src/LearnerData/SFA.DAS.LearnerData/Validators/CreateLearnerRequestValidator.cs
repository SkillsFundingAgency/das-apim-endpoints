using FluentValidation;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Validators;

public class CreateLearnerRequestValidator : AbstractValidator<CreateLearnerRequest>
{
    public CreateLearnerRequestValidator()
    {
        RuleFor(model => model.Learner.Uln).Must(uln => ValidateUln(uln))
            .WithMessage(model => $"Learner data contains incorrect ULN {model.Learner.Uln}");

        RuleForEach(model => model.Delivery.OnProgramme.First().Costs)
            .ChildRules(cost =>
            {
                cost.RuleFor(c => c.EpaoPrice).GreaterThanOrEqualTo(0)
                    .WithMessage(c => $"Learner data contains a negative EpaoPrice {c.EpaoPrice}");

                cost.RuleFor(c => c.TrainingPrice).GreaterThanOrEqualTo(0)
                    .WithMessage(c => $"Learner data contains a negative TrainingPrice {c.TrainingPrice}");
            });

        RuleFor(model => model.Delivery.OnProgramme.First().StandardCode).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative StandardCode {model.Delivery.OnProgramme.First().StandardCode}");

        RuleFor(model => model.ConsumerReference)
           .MaximumLength(100)
           .WithMessage("ConsumerReference cannot be more then 100 characters long");

        RuleFor(model => model.Learner.FirstName)
            .NotEmpty()
            .WithMessage("Firstname is required")
            .MaximumLength(100)
            .WithMessage("Firstname cannot be more then 100 characters long");

        RuleFor(model => model.Learner.LastName)
            .NotEmpty()
            .WithMessage("Lastname is required")
            .MaximumLength(100)
            .WithMessage("Lastname cannot be more then 100 characters long");

        RuleFor(model => model.Learner.Dob)
            .NotNull()
            .WithMessage("Dob is required");

        RuleFor(model => model.Learner.Email)
            .MaximumLength(200)
            .WithMessage("Email cannot be more then 200 characters long")
            .EmailAddress().When(model => model.Learner.Email != null);

        RuleFor(model => model.Delivery.OnProgramme.First().AgreementId)
            .MaximumLength(20)
            .WithMessage("OnProgramme AgreementId cannot be more then 20 characters long");

        RuleFor(model => model.Delivery.OnProgramme.First().StartDate)
            .NotNull()
            .WithMessage("OnProgramme StartDate is required");

        RuleFor(model => model.Delivery.OnProgramme.First().ExpectedEndDate)
            .NotNull()
            .WithMessage("OnProgramme ExpectedEndDate is required");

        RuleFor(model => model.Delivery.OnProgramme.First().IsFlexiJob)
            .NotNull()
            .WithMessage("OnProgramme IsFlexiJob is required");
    }

    private bool ValidateUln(string uln)
    {
        if(long.TryParse(uln, out var ulnAsLong))
        {
            return ulnAsLong is > 1000000000 or > 999999999;
        }
        else
        {
            return false;
        }
    } 
}

using FluentValidation;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Validators;

public class LearnerDataRequestValidator : AbstractValidator<LearnerDataRequest>
{
    public LearnerDataRequestValidator(long providerId, int academicYear)
    {
        // Commenting until agreed move forwards: https://skillsfundingagency.atlassian.net/browse/APPMAN-1697
        // RuleFor(model => model.StartDate).Must(startdate => startdate.IsInAcademicYear(academicYear))
        //     .WithMessage(model => $"Learner data contains a StartDate {model.StartDate} that is not in the academic year {academicYear}");

        RuleFor(model => model.ULN).Must(uln => uln is > 1000000000 or > 9999999999)
            .WithMessage(model => $"Learner data contains incorrect ULN {model.ULN}");

        RuleFor(model => model.UKPRN).Must(ukprn => providerId == ukprn)
            .WithMessage($"Learner data contains different UKPRN to {providerId}");
        RuleFor(model => model.UKPRN).Must(ukprn => ukprn is > 10000000 and < 9999999999)
            .WithMessage(model => $"Learner data contains incorrect UKPRN {model.UKPRN}");

        RuleFor(model => model.EpaoPrice).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative EpaoPrice {model.EpaoPrice}");

        RuleFor(model => model.TrainingPrice).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative TrainingPrice {model.TrainingPrice}");

        RuleFor(model => model.PlannedOTJTrainingHours).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative PlannedOTJTrainingHours {model.PlannedOTJTrainingHours}");

        RuleFor(model => model.StandardCode).GreaterThanOrEqualTo(0).When(model => model.StandardCode.HasValue)
            .WithMessage(model => $"Learner data contains a negative StandardCode {model.StandardCode}");

        //RuleFor(model => model.LarsCode).NotEmpty().When(model => !model.StandardCode.HasValue)
        //    .WithMessage(model => "Learner data must contains a LarsCode when StandardCode is null");

        RuleFor(model => model.ConsumerReference)
            .MaximumLength(100)
            .WithMessage("ConsumerReference cannot be more then 100 characters long");

        RuleFor(model => model.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required")
            .MaximumLength(100)
            .WithMessage("FirstName cannot be more then 100 characters long");

        RuleFor(model => model.LastName)
            .NotEmpty()
            .WithMessage("LastName is required")
            .MaximumLength(100)
            .WithMessage("LastName cannot be more then 100 characters long");

        RuleFor(model => model.LearnerEmail)
            .MaximumLength(200)
            .WithMessage("Email cannot be more then 200 characters long")
            .EmailAddress().When(model => model.LearnerEmail != null);

        RuleFor(model => model.AgreementId)
            .MaximumLength(20)
            .WithMessage("AgreementId cannot be more then 20 characters long");
    }
}

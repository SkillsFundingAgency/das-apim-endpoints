using FluentValidation;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.AcademicYearService;

namespace SFA.DAS.LearnerData.Validators;

public class LearnerDataRequestValidator : AbstractValidator<LearnerDataRequest>
{
    public LearnerDataRequestValidator(long providerId, int academicYear)
    {
        RuleFor(model => model.StartDate).Must(startdate => startdate.IsInAcademicYear(academicYear))
            .WithMessage(model => $"Learner data contains a StartDate {model.StartDate} that is not in the academic year {academicYear}");

        RuleFor(model => model.ULN).Must(uln => uln > 1000000000 || uln > 9999999999)
            .WithMessage(model => $"Learner data contains incorrect ULN {model.ULN}");

        RuleFor(model => model.UKPRN).Must(ukprn => providerId == ukprn)
            .WithMessage($"Learner data contains different UKPRN to {providerId}");
        RuleFor(model => model.UKPRN).Must(ukprn => ukprn > 10000000 && ukprn < 9999999999)
            .WithMessage(model => $"Learner data contains incorrect UKPRN {model.UKPRN}");

        RuleFor(model => model.EpaoPrice).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative EpaoPrice {model.EpaoPrice}");

        RuleFor(model => model.TrainingPrice).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative TrainingPrice {model.TrainingPrice}");

        RuleFor(model => model.PlannedOTJTrainingHours).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative PlannedOTJTrainingHours {model.PlannedOTJTrainingHours}");

        RuleFor(model => model.StandardCode).GreaterThanOrEqualTo(0)
            .WithMessage(model => $"Learner data contains a negative StandardCode {model.StandardCode}");

        RuleFor(model => model.ConsumerReference).MaximumLength(100);

        RuleFor(model => model.FirstName).NotEmpty();
        RuleFor(model => model.LastName).NotEmpty();
        RuleFor(model => model.LearnerEmail).EmailAddress().When(model=> model.LearnerEmail != null);
    }
}

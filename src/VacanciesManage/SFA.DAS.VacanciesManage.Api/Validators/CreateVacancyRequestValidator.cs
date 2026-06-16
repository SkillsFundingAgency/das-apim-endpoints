using FluentValidation;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Services;
using SFA.DAS.VacanciesManage.Api.Extensions;

namespace SFA.DAS.VacanciesManage.Api.Validators;

public class CreateVacancyRequestValidator : AbstractValidator<CreateVacancyRequest>
{
    public CreateVacancyRequestValidator(IHtmlValidatorService htmlValidator)
    {
        RuleFor(x => x.ShortDescription)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.Description)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.EmployerDescription)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.OutcomeDescription)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.TrainingDescription)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.RecruitingNationallyDetails)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.AdditionalTrainingDescription)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.Wage.CompanyBenefitsInformation)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.AdditionalQuestion1)
            .MustBeValidHtml(htmlValidator);

        RuleFor(x => x.AdditionalQuestion2)
            .MustBeValidHtml(htmlValidator);
    }
}
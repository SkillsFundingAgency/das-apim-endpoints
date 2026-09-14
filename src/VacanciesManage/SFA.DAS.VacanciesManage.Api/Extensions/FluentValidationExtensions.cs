using System.Linq;
using FluentValidation;
using SFA.DAS.VacanciesManage.Services;

namespace SFA.DAS.VacanciesManage.Api.Extensions;

public static class FluentValidationExtensions
{
    public static void MustBeValidHtml<T>(this IRuleBuilder<T, string> rule,
        IHtmlValidatorService htmlValidator)
    {
        rule.Custom((value, context) =>
        {
            var result = htmlValidator.ValidateHtml(value ?? "");

            if (result.IsValid)
                return;

            var message = string.Join("; ",
                result.Errors.Select(e => e.ErrorMessage));

            context.AddFailure(context.DisplayName, message);
        });
    }
}
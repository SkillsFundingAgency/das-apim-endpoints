using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace SFA.DAS.Aodp.Validation;
public sealed class QualificationNumberAttribute : ValidationAttribute
{
    public const string DefaultErrorMessage =
        "Enter a qualification number in the format 12345678, 1234567X, 123/4567/8 or 123/4567/X";

    public QualificationNumberAttribute() : base(DefaultErrorMessage) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var s = value as string;

        if (string.IsNullOrWhiteSpace(s))
            return ValidationResult.Success;

        return RegexCache.QanRegex.IsMatch(s.Trim())
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage);
    }
}

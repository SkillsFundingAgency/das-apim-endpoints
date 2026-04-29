using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AllowedCharactersAttribute : ValidationAttribute
{
    private readonly TextCharacterProfile _profile;

    public AllowedCharactersAttribute(TextCharacterProfile profile)
        : base("{0} contains invalid characters")
    {
        _profile = profile;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string s || string.IsNullOrWhiteSpace(s))
            return ValidationResult.Success;

        s = s.Trim();

        bool isValid = _profile switch
        {
            TextCharacterProfile.PersonName =>
                RegexCache.PersonNameRegex.IsMatch(s),

            TextCharacterProfile.Title =>
                RegexCache.TitleRegex.IsMatch(s),

            TextCharacterProfile.FreeText =>
                TextValidationRules.IsFreeTextValid(s),

            _ => true
        };

        return isValid
            ? ValidationResult.Success
            : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
   
}

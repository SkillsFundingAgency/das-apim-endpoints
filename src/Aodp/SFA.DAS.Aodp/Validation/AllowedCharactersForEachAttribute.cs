using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation
{
    public class AllowedCharactersForEachAttribute : ValidationAttribute
    {
        private readonly TextCharacterProfile _profile;

        public AllowedCharactersForEachAttribute(TextCharacterProfile profile)
        {
            _profile = profile;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is not IEnumerable<string> list)
                return ValidationResult.Success;

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var s = item.Trim();

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

                if (!isValid)
                    return new ValidationResult($"{context.MemberName} contains invalid values");
            }

            return ValidationResult.Success;
        }
    }
}

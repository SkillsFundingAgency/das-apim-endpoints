using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation
{
    public class AllowedValuesAttribute : ValidationAttribute
    {
        private readonly HashSet<string> _allowedValues;

        public AllowedValuesAttribute(params string[] allowedValues)
        {
            _allowedValues = new HashSet<string>(allowedValues);
        }

        protected virtual bool AllowNull => true;
        protected virtual bool AllowEmpty => true;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return AllowNull
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} is required");

            if (value is not string s)
                return new ValidationResult($"{validationContext.MemberName} must be a string");

            if (string.IsNullOrWhiteSpace(s))
                return AllowEmpty
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} cannot be empty");

            return _allowedValues.Contains(s)
                ? ValidationResult.Success
                : new ValidationResult($"{validationContext.MemberName} has invalid value");
        }
    }
}
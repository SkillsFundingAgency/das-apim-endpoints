using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.UnitTests.Validation
{
    public static class ValidationTestHelper
    {
        public static bool Validate(object model, out List<string> errors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(
                model,
                context,
                results,
                validateAllProperties: true);

            errors = results.Select(r => r.ErrorMessage ?? "").ToList();
            return isValid;
        }
    }
}

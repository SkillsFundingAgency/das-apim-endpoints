namespace SFA.DAS.SharedOuterApi.Types.Validation;

public interface IValidator<in T>
{
    Task<ValidationResult> ValidateAsync(T item);
}
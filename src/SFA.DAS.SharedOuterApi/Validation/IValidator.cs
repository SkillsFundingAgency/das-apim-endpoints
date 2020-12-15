using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Validation
{
    public interface IValidator<in T>
    {
        Task<ValidationResult> ValidateAsync(T item);
    }
}
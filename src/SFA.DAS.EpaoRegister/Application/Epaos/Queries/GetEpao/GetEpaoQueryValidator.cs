using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao
{
    public class GetEpaoQueryValidator : EpaoIdValidator, IValidator<GetEpaoQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetEpaoQuery item)
        {
            var result = new ValidationResult();
            
            ValidateEpaoId(item.EpaoId, ref result);

            return Task.FromResult(result);
        }
    }
}
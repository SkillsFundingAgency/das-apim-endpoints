using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesQueryValidator : EpaoIdValidator, IValidator<GetEpaoCoursesQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetEpaoCoursesQuery item)
        {
            var result = new ValidationResult();

            ValidateEpaoId(item.EpaoId, ref result);

            return Task.FromResult(result);
        }
    }
}
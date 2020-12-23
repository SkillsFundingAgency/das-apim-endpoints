using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoQueryValidator : EpaoIdValidator, IValidator<GetCourseEpaoQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetCourseEpaoQuery item)
        {
            var result = new ValidationResult();
            
            ValidateEpaoId(item.EpaoId, ref result);

            return Task.FromResult(result);
        }
    }
}
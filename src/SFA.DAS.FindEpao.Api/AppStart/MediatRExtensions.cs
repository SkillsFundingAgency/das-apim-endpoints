using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.FindEpao.Api.AppStart
{
    public static class MediatRExtensions
    {
        public static void AddMediatRValidation(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<GetCourseEpaoQuery>), typeof(GetCourseEpaoQueryValidator));
        }
    }
}
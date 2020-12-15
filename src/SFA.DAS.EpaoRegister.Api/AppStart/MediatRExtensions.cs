using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.EpaoRegister.Api.AppStart
{
    public static class MediatRExtensions
    {
        public static void AddMediatRValidation(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<GetEpaoQuery>), typeof(GetEpaoQueryValidator));
            services.AddScoped(typeof(IValidator<GetEpaoCoursesQuery>), typeof(GetEpaoCoursesQueryValidator));
        }
    }
}
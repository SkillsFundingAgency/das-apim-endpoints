using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesQueryHandler : IRequestHandler<GetEpaoCoursesQuery, GetEpaoCoursesResult>
    {
        private const int ExpirationInHours = 1;
        private readonly IValidator<GetEpaoCoursesQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetEpaoCoursesQueryHandler(
            IValidator<GetEpaoCoursesQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICacheStorageService cacheStorageService)
        {
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<GetEpaoCoursesResult> Handle(GetEpaoCoursesQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult, null, null);
            }

            var epaoCourses = (await _assessorsApiClient.GetAll<GetEpaoCoursesListItem>(
                new GetEpaoCoursesRequest(request.EpaoId)))
                ?.ToList();

            if (epaoCourses == null || epaoCourses.Count == 0)
            {
                return new GetEpaoCoursesResult
                {
                    EpaoId = request.EpaoId,
                    Courses = new List<GetStandardResponse>()
                };
            }

            var courses = await _cacheStorageService.RetrieveFromCache<GetStandardsListResponse>(
                    nameof(GetAllStandardsListRequest));

            if (courses == default)
            {
                courses = await _coursesApiClient.Get<GetStandardsListResponse>(
                    new GetAllStandardsListRequest());
                await _cacheStorageService.SaveToCache(nameof(GetAllStandardsListRequest), courses, ExpirationInHours);
            }

            var matchingCourses = courses.Standards
                .Where(response => epaoCourses
                    .Select(item => item.StandardCode)
                    .Contains(response.LarsCode))
                .ToList();
            
            return new GetEpaoCoursesResult
            {
                EpaoId = request.EpaoId,
                Courses = matchingCourses
            };
        }
    }
}

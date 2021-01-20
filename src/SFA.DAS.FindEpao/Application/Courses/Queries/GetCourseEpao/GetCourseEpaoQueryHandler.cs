using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoQueryHandler : IRequestHandler<GetCourseEpaoQuery, GetCourseEpaoResult>
    {
        private readonly ILogger<GetCourseEpaoQueryHandler> _logger;
        private readonly IValidator<GetCourseEpaoQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICachedDeliveryAreasService _cachedDeliveryAreasService;
        private readonly ICourseEpaoIsValidFilterService _courseEpaoIsValidFilterService;

        public GetCourseEpaoQueryHandler(
            ILogger<GetCourseEpaoQueryHandler> logger,
            IValidator<GetCourseEpaoQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICachedDeliveryAreasService cachedDeliveryAreasService,
            ICourseEpaoIsValidFilterService courseEpaoIsValidFilterService)
        {
            _logger = logger;
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
            _cachedDeliveryAreasService = cachedDeliveryAreasService;
            _courseEpaoIsValidFilterService = courseEpaoIsValidFilterService;
        }

        public async Task<GetCourseEpaoResult> Handle(GetCourseEpaoQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult, null, null);
            }

            var epaoTask = _assessorsApiClient.Get<GetEpaoResponse>(new GetEpaoRequest(request.EpaoId));
            var courseEpaosTask = _assessorsApiClient.GetAll<GetCourseEpaoListItem>(
                new GetCourseEpaosRequest {CourseId = request.CourseId});
            var areasTask = _cachedDeliveryAreasService.GetDeliveryAreas();
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            
            await Task.WhenAll(epaoTask, courseEpaosTask, areasTask, courseTask);

            if (epaoTask.Result == default)
            {
                throw new NotFoundException<GetCourseEpaoResult>();
            }

            var filteredCourseEpaos = courseEpaosTask.Result
                .Where(_courseEpaoIsValidFilterService.IsValidCourseEpao)
                .ToList();

            if (!filteredCourseEpaos.Any(item => string.Equals(item.EpaoId,
                    request.EpaoId, StringComparison.CurrentCultureIgnoreCase)))
            {
                _logger.LogInformation($"Course [{request.CourseId}], EPAO [{request.EpaoId}] not active.");
                throw new NotFoundException<GetCourseEpaoResult>();
            }

            var courseEpao = filteredCourseEpaos.Single(item => 
                string.Equals(item.EpaoId, request.EpaoId, StringComparison.CurrentCultureIgnoreCase));

            return new GetCourseEpaoResult
            {
                Epao = epaoTask.Result,
                Course = courseTask.Result,
                EpaoDeliveryAreas = courseEpao.DeliveryAreas,
                CourseEpaosCount = filteredCourseEpaos.Count,
                DeliveryAreas = areasTask.Result,
                EffectiveFrom = courseEpao.CourseEpaoDetails.EffectiveFrom.Value
            };
        }
    }
}

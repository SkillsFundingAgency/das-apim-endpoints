using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoQueryHandler : IRequestHandler<GetCourseEpaoQuery, GetCourseEpaoResult>
    {
        private const int DeliveryAreaCacheDurationInHours = 1;

        private readonly IValidator<GetCourseEpaoQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetCourseEpaoQueryHandler(IValidator<GetCourseEpaoQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICacheStorageService cacheStorageService)
        {
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
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
            var areasTask = GetDeliveryAreas();
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            
            await Task.WhenAll(epaoTask, courseEpaosTask, areasTask, courseTask);

            if (epaoTask.Result == default)
            {
                throw new NotFoundException<GetCourseEpaoResult>();
            }

            return new GetCourseEpaoResult
            {
                Epao = epaoTask.Result,
                Course = courseTask.Result,
                EpaoDeliveryAreas = courseEpaosTask.Result.Single(item => item.EpaoId == request.EpaoId).DeliveryAreas,
                CourseEpaosCount = courseEpaosTask.Result.Count(),
                DeliveryAreas = areasTask.Result
            };
        }

        private async Task<IEnumerable<GetDeliveryAreaListItem>> GetDeliveryAreas()
        {
            var deliveryAreas = await _cacheStorageService
                .RetrieveFromCache<IEnumerable<GetDeliveryAreaListItem>>(nameof(GetDeliveryAreasRequest));
            
            if (deliveryAreas != null) 
                return deliveryAreas;
            
            deliveryAreas = await _assessorsApiClient.GetAll<GetDeliveryAreaListItem>(new GetDeliveryAreasRequest());
            await _cacheStorageService.SaveToCache(nameof(GetDeliveryAreasRequest), deliveryAreas, DeliveryAreaCacheDurationInHours);

            return deliveryAreas;
        }
    }
}

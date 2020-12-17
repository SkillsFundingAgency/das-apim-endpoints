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
        private readonly IValidator<GetCourseEpaoQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetCourseEpaoQueryHandler(IValidator<GetCourseEpaoQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> CoursesApiClient)
        {
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = CoursesApiClient;
        }

        public async Task<GetCourseEpaoResult> Handle(GetCourseEpaoQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult, null, null);
            }

            var epao = await _assessorsApiClient.Get<GetEpaoResponse>(new GetEpaoRequest(request.EpaoId));
            var courseEpaos = await _assessorsApiClient.GetAll<GetCourseEpaoListItem>(
                new GetCourseEpaosRequest {CourseId = request.CourseId});
            var course = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));

            if (epao == default)
            {
                throw new NotFoundException<GetCourseEpaoResult>();
            }

            return new GetCourseEpaoResult
            {
                Epao = epao,
                Course = course,
                EpaoDeliveryAreas = courseEpaos.Single(item => item.EpaoId == request.EpaoId).DeliveryAreas,
                CourseEpaosCount = courseEpaos.Count()
            };
        }
    }
}

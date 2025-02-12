using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetCourseProvidersQuery, GetCourseProvidersResponse>
    {

        public async Task<GetCourseProvidersResponse> Handle(GetCourseProvidersQuery request,
            CancellationToken cancellationToken)
        {

            var response =
                await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProvidersResponse>(
                    new GetProvidersByCourseIdRequest()
                    {
                        CourseId = request.Id,
                        OrderBy = request.OrderBy,
                        Distance = request.Distance,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        DeliveryModes = request.DeliveryModes,
                        EmployerProviderRatings = request.EmployerProviderRatings,
                        ApprenticeProviderRatings = request.ApprenticeProviderRatings,
                        Qar = request.Qar,
                        Page = request.Page,
                        PageSize = request.PageSize
                    });

            return ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode)
                    ? response.Body
                    : new GetCourseProvidersResponse();
        }
    }
}
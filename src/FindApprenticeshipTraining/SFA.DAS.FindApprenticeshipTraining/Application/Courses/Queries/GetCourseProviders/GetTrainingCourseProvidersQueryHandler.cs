using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetTrainingCourseProvidersQuery, GetProvidersListFromCourseIdResponse>
    {

        public async Task<GetProvidersListFromCourseIdResponse> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {

            var getProvidersListFromCourseIdResponse = await _roatpCourseManagementApiClient.Get<GetProvidersListFromCourseIdResponse>(new GetProvidersByCourseIdRequest(
                request.Id, request.OrderBy, request.Distance, request.Latitude, request.Longitude, request.DeliveryModes, request.EmployerProviderRatings, request.ApprenticeProviderRatings,
                request.Qar, request.Page, request.PageSize));

            return getProvidersListFromCourseIdResponse;
        }
    }
}
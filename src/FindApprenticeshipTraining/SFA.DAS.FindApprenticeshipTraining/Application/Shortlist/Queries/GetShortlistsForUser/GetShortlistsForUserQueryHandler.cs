using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;

public class GetShortlistsForUserQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetShortlistsForUserQuery, GetShortlistsForUserResponse>
{
    public async Task<GetShortlistsForUserResponse> Handle(GetShortlistsForUserQuery request, CancellationToken cancellationToken)
    {
        var response = await _roatpCourseManagementApiClient.Get<GetShortlistsForUserResponse>(new GetShortlistsForUserRequest(request.UserId));

        return response;
    }
}
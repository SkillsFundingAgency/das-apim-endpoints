using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;

public class GetShortlistCountForUserQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetShortlistCountForUserQuery, GetShortlistCountForUserQueryResult>
{
    public async Task<GetShortlistCountForUserQueryResult> Handle(GetShortlistCountForUserQuery request, CancellationToken cancellationToken)
    {
        var response = await _roatpCourseManagementApiClient.GetWithResponseCode<GetShortlistCountForUserQueryResult>(new GetShortlistCountForUserRequest(request.UserId));
        return response.Body;
    }
}

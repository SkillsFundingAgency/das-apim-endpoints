using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;

public class CreateShortlistForUserCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<CreateShortlistForUserCommand, PostShortListResponse>
{
    public async Task<PostShortListResponse> Handle(CreateShortlistForUserCommand request, CancellationToken cancellationToken)
    {
        ApiResponse<PostShortListResponse> response = await _roatpCourseManagementApiClient.PostWithResponseCode<PostShortListResponse>(
            new PostShortlistForUserRequest
            {
                Data = new PostShortlistData
                {
                    Latitude = request.Lat,
                    Longitude = request.Lon,
                    Ukprn = request.Ukprn,
                    LocationDescription = request.LocationDescription,
                    LarsCode = request.StandardId,
                    UserId = request.ShortlistUserId
                }
            },
            false);

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}

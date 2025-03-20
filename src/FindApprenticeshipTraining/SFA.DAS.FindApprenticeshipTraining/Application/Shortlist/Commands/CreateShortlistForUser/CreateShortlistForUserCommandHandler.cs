using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;

public class CreateShortlistForUserCommandHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
    ICachedLocationLookupService _cachedLocationLookupService
) : IRequestHandler<CreateShortlistForUserCommand, PostShortListResponse>
{
    public async Task<PostShortListResponse> Handle(CreateShortlistForUserCommand request, CancellationToken cancellationToken)
    {
        LocationItem locationItem = await _cachedLocationLookupService.GetCachedLocationInformation(request.LocationDescription);

        ApiResponse<PostShortListResponse> response = await _roatpCourseManagementApiClient.PostWithResponseCode<PostShortListResponse>(
            new PostShortlistForUserRequest
            {
                Data = new PostShortlistData
                {
                    Latitude = (float?)locationItem?.Latitude,
                    Longitude = (float?)locationItem?.Longitude,
                    Ukprn = request.Ukprn,
                    LocationDescription = request.LocationDescription,
                    LarsCode = request.LarsCode,
                    UserId = request.ShortlistUserId
                }
            },
            false);

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}

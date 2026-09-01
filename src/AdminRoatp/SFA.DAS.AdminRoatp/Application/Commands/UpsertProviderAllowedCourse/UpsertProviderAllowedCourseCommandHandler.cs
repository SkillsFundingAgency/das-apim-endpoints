using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;

public class UpsertProviderAllowedCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, IRoatpServiceApiClient<RoatpConfiguration> _roatpServiceApiClient) : IRequestHandler<UpsertProviderAllowedCourseCommand>
{
    public async Task Handle(UpsertProviderAllowedCourseCommand command, CancellationToken cancellationToken)
    {
        var model = new UpsertProviderAllowedCourseModel()
        {
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName,
            LastDateStarts = command.LastDateStarts,
            IsStartRestricted = command.IsStartRestricted,
        };

        var apiRequest = new UpsertProviderAllowedCourseRequest(command.Ukprn, command.LarsCode, model);

        var response = await _courseManagementApiClient.PostWithResponseCode<Unit>(apiRequest);

        response.EnsureSuccessStatusCode();

        var standard = await _courseManagementApiClient.GetWithResponseCode<StandardModel>(new GetStandardByLarsCodeRequest(command.LarsCode));

        standard.EnsureSuccessStatusCode();

        if (standard.Body.CourseType == CourseType.ShortCourse)
        {
            var updateCourseTypes = new UpdateCourseTypesModel([2], command.UserId);

            var updateCourseTypesrequest = new UpdateCourseTypesRequest(command.Ukprn, updateCourseTypes);

            var updateCourseTypesresponse = await _roatpServiceApiClient.PutWithResponseCode<NullResponse>(updateCourseTypesrequest);

            updateCourseTypesresponse.EnsureSuccessStatusCode();
        }
    }
}

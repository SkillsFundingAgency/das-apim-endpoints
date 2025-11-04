using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenDeletingManyNotifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_Delete_Message_Is_Sent_Correctly(
        List<long> ids,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        var response = new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null); 
        DeleteNotificationsByIdsRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.DeleteWithResponseCode<NullResponse>(It.IsAny<DeleteNotificationsByIdsRequest>(), false))
            .Callback<IDeleteApiRequest, bool>((x, _) => capturedRequest = x as DeleteNotificationsByIdsRequest)
            .ReturnsAsync(response);
        
        var expectedUrl = QueryHelpers.AddQueryString("api/notifications", [
            new KeyValuePair<string, StringValues>("ids", ids.Select(x => $"{x}").ToArray())
        ]);

        // act
        var result = await sut.DeleteMany(recruitApiClient.Object, ids) as StatusCodeHttpResult;

        // assert
        result!.StatusCode.Should().Be((int)response.StatusCode);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.DeleteUrl.Should().Be(expectedUrl);
    }
}
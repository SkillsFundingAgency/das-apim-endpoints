using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitJobs.Api.Controllers;
using System.Collections.Generic;
using System.Net;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenDeletingManyNotifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_Delete_Message_Is_Sent_Correctly(
        List<long> ids,
        Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        var response = new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null); 
        recruitApiClient
            .Setup(x => x.DeleteWithResponseCode<NullResponse>(It.IsAny<DeleteNotificationsApiRequest>(), false))
            .ReturnsAsync(response);
        
        // act
        var result = await sut.DeleteMany(recruitApiClient.Object, ids) as StatusCodeHttpResult;

        // assert
        result!.StatusCode.Should().Be((int)response.StatusCode);
    }
}
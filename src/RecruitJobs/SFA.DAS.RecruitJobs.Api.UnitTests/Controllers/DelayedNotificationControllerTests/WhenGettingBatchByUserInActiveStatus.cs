using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Enums;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;
using SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using NotificationEmail = SFA.DAS.RecruitJobs.Api.Models.NotificationEmail;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenGettingBatchByUserInActiveStatus
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
        GetDelayedNotificationsByUserStatusResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        GetDelayedNotificationsByUserStatusRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByUserStatusResponse>(It.IsAny<GetDelayedNotificationsByUserStatusRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetDelayedNotificationsByUserStatusRequest)
            .ReturnsAsync(response);

        var expectedUrl = QueryHelpers.AddQueryString("api/notifications/batch/by/userStatus", "status", nameof(UserStatus.Inactive));

        // act
        await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be(expectedUrl);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        GetDelayedNotificationsByUserStatusResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByUserStatusResponse>(It.IsAny<GetDelayedNotificationsByUserStatusRequest>()))
            .ReturnsAsync(response);

        // act
        var result = await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object) as Ok<List<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response.Emails);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Response_Then_Empty_Results_Are_Returned(
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByUserStatusResponse>(It.IsAny<GetDelayedNotificationsByUserStatusRequest>()))
            .ReturnsAsync((GetDelayedNotificationsByUserStatusResponse)null!);

        // act
        var result = await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object) as Ok<List<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new List<NotificationEmail>());
    }
}
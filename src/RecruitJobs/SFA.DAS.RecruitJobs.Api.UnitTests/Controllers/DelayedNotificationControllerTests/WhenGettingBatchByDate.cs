using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;
using SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using NotificationEmail = SFA.DAS.RecruitJobs.Api.Models.NotificationEmail;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenGettingBatchByDate
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
        DateTime dateTime,
        GetDelayedNotificationsByDateResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        GetDelayedNotificationsByDateRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByDateResponse>(It.IsAny<GetDelayedNotificationsByDateRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetDelayedNotificationsByDateRequest)
            .ReturnsAsync(response);

        var expectedUrl = QueryHelpers.AddQueryString("api/notifications/batch/by/date", "dateTime", dateTime.ToString("s"));

        // act
        await sut.GetBatchByDate(recruitApiClient.Object, dateTime);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be(expectedUrl);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        DateTime dateTime,
        GetDelayedNotificationsByDateResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByDateResponse>(It.IsAny<GetDelayedNotificationsByDateRequest>()))
            .ReturnsAsync(response);

        // act
        var result = await sut.GetBatchByDate(recruitApiClient.Object, dateTime) as Ok<List<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response.Emails);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Response_Then_Empty_Results_Are_Returned(
        DateTime dateTime,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetDelayedNotificationsByDateResponse>(It.IsAny<GetDelayedNotificationsByDateRequest>()))
            .ReturnsAsync((GetDelayedNotificationsByDateResponse)null!);

        // act
        var result = await sut.GetBatchByDate(recruitApiClient.Object, dateTime) as Ok<List<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new List<NotificationEmail>());
    }
}
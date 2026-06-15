using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenGettingBatchByDate
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
        DateTime dateTime,
        GetNotificationsBatchByDateResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        GetNotificationsBatchByDateApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetNotificationsBatchByDateResponse>(It.IsAny<GetNotificationsBatchByDateApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetNotificationsBatchByDateApiRequest)
            .ReturnsAsync(response);

        // act
        await sut.GetBatchByDate(recruitApiClient.Object, dateTime);

        // assert
        capturedRequest.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        DateTime dateTime,
        GetNotificationsBatchByDateResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetNotificationsBatchByDateResponse>(It.IsAny<GetNotificationsBatchByDateApiRequest>()))
            .ReturnsAsync(response);

        // act
        var result = await sut.GetBatchByDate(recruitApiClient.Object, dateTime) as Ok<ICollection<NotificationEmail>>;

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
            .Setup(x => x.Get<GetNotificationsBatchByDateResponse>(It.IsAny<GetNotificationsBatchByDateApiRequest>()))
            .ReturnsAsync((GetNotificationsBatchByDateResponse)null!);

        // act
        var result = await sut.GetBatchByDate(recruitApiClient.Object, dateTime) as Ok<ICollection<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new List<NotificationEmail>());
    }
}
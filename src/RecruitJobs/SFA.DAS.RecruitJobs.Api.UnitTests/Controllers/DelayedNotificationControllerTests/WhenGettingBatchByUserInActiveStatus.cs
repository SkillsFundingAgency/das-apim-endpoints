using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Collections.Generic;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenGettingBatchByUserInActiveStatus
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
        GetNotificationsBatchByUserStatusResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        GetNotificationsBatchByUserstatusApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetNotificationsBatchByUserStatusResponse>(It.IsAny<GetNotificationsBatchByUserstatusApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetNotificationsBatchByUserstatusApiRequest)
            .ReturnsAsync(response);

        // act
        await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object);

        // assert
        capturedRequest.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        GetNotificationsBatchByUserStatusResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetNotificationsBatchByUserStatusResponse>(It.IsAny<GetNotificationsBatchByUserstatusApiRequest>()))
            .ReturnsAsync(response);

        // act
        var result = await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object) as Ok<ICollection<NotificationEmail>>;

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
            .Setup(x => x.Get<GetNotificationsBatchByUserStatusResponse>(It.IsAny<GetNotificationsBatchByUserstatusApiRequest>()))
            .ReturnsAsync((GetNotificationsBatchByUserStatusResponse)null!);

        // act
        var result = await sut.GetBatchByUserInActiveStatus(recruitApiClient.Object) as Ok<ICollection<NotificationEmail>>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new List<NotificationEmail>());
    }
}
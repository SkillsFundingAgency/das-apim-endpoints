using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;
using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenGettingBatchByDate
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        DateTime dateTime,
        GetNotificationsBatchByDateResponse response,
        Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
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
        Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
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
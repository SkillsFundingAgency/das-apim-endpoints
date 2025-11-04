using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Handlers;

public class WhenHandlingSharedApplicationReviewedEventHandler
{
    [Test, MoqAutoData]
    public async Task Then_The_Call_Is_Made_To_Create_The_Notifications(
        SharedApplicationReviewedEvent @event,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<List<NotificationEmailDto>>([], HttpStatusCode.OK, null);
        PostCreateApplicationReviewNotificationsRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode<List<NotificationEmailDto>>(
                It.IsAny<PostCreateApplicationReviewNotificationsRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, _) => capturedRequest = x as PostCreateApplicationReviewNotificationsRequest)
            .ReturnsAsync(apiResponse);
        
        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.PostUrl.Should().Be($"api/applicationreviews/{@event.ApplicationReviewId}/create-notifications");
    }
}

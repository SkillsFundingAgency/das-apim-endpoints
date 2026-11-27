using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.Client;
using SFA.DAS.RecruitJobs.Api.Controllers;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.ReportsControllerTests;

public class WhenCallingPostCleanupReports
{
    [Test, MoqAutoData]
    public async Task Then_The_Inner_Api_Request_Should_Be_Made(
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitClient,
        [Greedy] ReportsController controller)
    {
        // arrange
        recruitClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostReportsCleanupApiRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null!));
        
        // act
        var result = await controller.PostCleanupReports(recruitClient.Object) as Ok;

        // assert
        result.Should().NotBeNull();
        recruitClient.Verify(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostReportsCleanupApiRequest>(), false), Times.Once);
    }
}
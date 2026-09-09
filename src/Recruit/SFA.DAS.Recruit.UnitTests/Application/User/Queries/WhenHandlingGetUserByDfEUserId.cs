using SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Queries;

[TestFixture]
internal class WhenHandlingGetUserByDfEUserId
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetUserByDfeUserIdQuery query,
        GetUserByDfeUserIdResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetUserByDfeUserIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetUserByDfeUserIdRequest(query.DfeUserId);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByDfeUserIdResponse>(
                It.Is<GetUserByDfeUserIdRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(new ApiResponse<GetUserByDfeUserIdResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.GetWithResponseCode<GetUserByDfeUserIdResponse>(It.IsAny<GetUserByDfeUserIdRequest>()), Times.Once);
    }
}
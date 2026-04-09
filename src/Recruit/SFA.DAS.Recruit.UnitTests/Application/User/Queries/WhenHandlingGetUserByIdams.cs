using SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Queries;

[TestFixture]
internal class WhenHandlingGetUserByIdams
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetUserByIdamsIdQuery query,
        GetUserByIdamsIdResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetUserByIdamsIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetUserByIdamsIdRequest(query.IdamsId);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByIdamsIdResponse>(
                It.Is<GetUserByIdamsIdRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(new ApiResponse<GetUserByIdamsIdResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.GetWithResponseCode<GetUserByIdamsIdResponse>(It.IsAny<GetUserByIdamsIdRequest>()), Times.Once);
    }
}
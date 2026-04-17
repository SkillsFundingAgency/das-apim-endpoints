using SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Queries;

[TestFixture]
internal class WhenHandlingGetUsersByEmployerAccountId
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetUsersByEmployerAccountIdQuery query,
        GetUsersByEmployerAccountIdResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetUsersByEmployerAccountIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetUsersByEmployerAccountIdRequest(query.EmployerAccountId);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetUsersByEmployerAccountIdResponse>(
                It.Is<GetUsersByEmployerAccountIdRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(new ApiResponse<GetUsersByEmployerAccountIdResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Users.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.GetWithResponseCode<GetUsersByEmployerAccountIdResponse>(It.IsAny<GetUsersByEmployerAccountIdRequest>()), Times.Once);
    }
}
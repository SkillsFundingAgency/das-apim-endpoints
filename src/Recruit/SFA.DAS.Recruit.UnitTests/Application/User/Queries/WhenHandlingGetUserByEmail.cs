using SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Queries;

[TestFixture]
internal class WhenHandlingGetUserByEmail
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetUserByEmailQuery query,
        GetUserByEmailResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetUserByEmailQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetUserByEmailRequest(query.Email, query.UserType);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.PostUrl.Equals(expectedGetUrl.PostUrl))))
            .ReturnsAsync(new ApiResponse<GetUserByEmailResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.PostWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>(), true), Times.Once);
    }
}
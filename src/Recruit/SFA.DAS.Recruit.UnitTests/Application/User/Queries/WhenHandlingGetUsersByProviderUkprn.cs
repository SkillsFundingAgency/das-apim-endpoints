using SFA.DAS.Recruit.Application.User.Queries.GetUsersByProviderUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Queries;

[TestFixture]
internal class WhenHandlingGetUsersByProviderUkprn
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetUsersByProviderUkprnQuery query,
        GetUsersByProviderUkprnResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetUsersByProviderUkprnQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetUsersByProviderUkprnRequest(query.Ukprn);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetUsersByProviderUkprnResponse>(
                It.Is<GetUsersByProviderUkprnRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(new ApiResponse<GetUsersByProviderUkprnResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Users.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.GetWithResponseCode<GetUsersByProviderUkprnResponse>(It.IsAny<GetUsersByProviderUkprnRequest>()), Times.Once);
    }
}
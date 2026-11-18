using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.GetProfanityWords;

[TestFixture]
internal class WhenHandingGetProfanity
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetProfanityListQuery query,
        List<string> apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetProfanityListQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetProhibitedcontentByContentTypeApiRequest(ProhibitedContentType.Profanity);
        recruitApiClient
            .Setup(x => x.Get<List<string>>(
                It.Is<GetProhibitedcontentByContentTypeApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ProfanityList.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<List<string>>(It.IsAny<GetProhibitedcontentByContentTypeApiRequest>()), Times.Once);
    }
}
using SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.GetBannedPhrases;

[TestFixture]
internal class WhenGettingBannedPhrases
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetBannedPhrasesQuery query,
        List<string> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetBannedPhrasesQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetBannedPhrasesListApiRequest();
        recruitApiClient
            .Setup(x => x.Get<List<string>>(
                It.Is<GetBannedPhrasesListApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.BannedPhraseList.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<List<string>>(It.IsAny<GetBannedPhrasesListApiRequest>()), Times.Once);
    }
}
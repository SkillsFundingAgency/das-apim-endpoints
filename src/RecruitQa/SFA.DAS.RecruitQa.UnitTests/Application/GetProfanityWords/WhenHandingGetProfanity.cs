using SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.GetProfanityWords;

[TestFixture]
internal class WhenHandingGetProfanity
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetProfanityListQuery query,
        List<string> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetProfanityListQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetProfanityListApiRequest();
        recruitApiClient
            .Setup(x => x.Get<List<string>>(
                It.Is<GetProfanityListApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ProfanityList.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<List<string>>(It.IsAny<GetProfanityListApiRequest>()), Times.Once);
    }
}
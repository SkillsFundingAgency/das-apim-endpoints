using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.ProhibitedContent;

[TestFixture]
internal class WhenGettingProfanity
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        GetProfanityListQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProhibitedContentController sut,
        CancellationToken token)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetProfanityListQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var result = await sut.GetAllProfanityList(token);
        var payload = (result as Ok<List<string>>)?.Value;

        // assert
        mockMediator.Verify(mediator => mediator.Send(
            It.IsAny<GetProfanityListQuery>(),
            It.IsAny<CancellationToken>()), Times.Once);
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(mediatorResponse.ProfanityList, options => options.ExcludingMissingMembers());
    }
}
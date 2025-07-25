using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers;

public class WhenGettingByLarsCode
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        int larsCode,
        GetProvidersByLarsCodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController sut)
    {
        // arrange
        GetProvidersByLarsCodeQuery? capturedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetProvidersByLarsCodeQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetProvidersByLarsCodeQueryResult>, CancellationToken>((x, _) => capturedQuery = x as GetProvidersByLarsCodeQuery)
            .ReturnsAsync(queryResult);
        
        // act
        var result = await sut.GetByLarsCode(larsCode, CancellationToken.None) as OkObjectResult;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().Be(queryResult);
        capturedQuery.Should().NotBeNull();
        capturedQuery!.LarsCode.Should().Be(larsCode);
    }
}